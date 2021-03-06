﻿// <copyright file="BattlesSimulatorJob.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator
{
    using System;
    using System.Linq;
    using System.Threading;

    using log4net;

    using OnlineGames.Data;
    using OnlineGames.Data.Models;
    using OnlineGames.Workers.BattlesSimulator.GamesSimulator;

    public class BattlesSimulatorJob : IJob
    {
        private readonly ILog logger;

        private readonly SynchronizedHashtable processingBattleIds;

        private readonly IGamesSimulator gamesSimulator;

        private bool stopping;

        public BattlesSimulatorJob(string name, SynchronizedHashtable processingBattleIds)
        {
            this.Name = name;

            this.logger = LogManager.GetLogger(name);
            this.logger.Info("BattlesSimulatorJob initializing...");

            this.stopping = false;
            this.processingBattleIds = processingBattleIds;

            this.gamesSimulator = new GamesSimulator.GamesSimulator();

            this.logger.Info("BattlesSimulatorJob initialized.");
        }

        public string Name { get; set; }

        public void Start()
        {
            this.logger.Info("BattlesSimulatorJob starting...");

            while (!this.stopping)
            {
                var data = new AiPortalDbContext();
                Battle battle;
                try
                {
                    battle =
                        data.Battles.Where(x => !x.IsFinished)
                            .OrderBy(x => x.FirstTeam.CompetitionId) // Temporary, remove it.
                            .ThenBy(x => Guid.NewGuid())
                            .FirstOrDefault();
                }
                catch (Exception exception)
                {
                    this.logger.FatalFormat("Unable to get battle for processing. Exception: {0}", exception);
                    throw;
                }

                if (battle == null)
                {
                    // No battle available. Wait 10 seconds and try again.
                    Thread.Sleep(10000);
                    continue;
                }

                if (!this.processingBattleIds.Add(battle.Id))
                {
                    // Other thread is processing the same battle. Wait 1 second and choose another battle for simulating.
                    Thread.Sleep(1000);
                    continue;
                }

                try
                {
                    foreach (var battleGameResult in battle.BattleGameResults.ToList())
                    {
                        data.BattleGameResults.Remove(battleGameResult);
                    }

                    this.ProcessBattle(data, battle);
                    battle.IsFinished = true;
                }
                catch (Exception exception)
                {
                    this.logger.ErrorFormat("ProcessBattle on battle №{0} has thrown an exception: {1}", battle.Id, exception);
                    battle.Comment = $"Exception in ProcessBattle: {exception.Message}";
                }

                try
                {
                    data.SaveChanges();
                }
                catch (Exception exception)
                {
                    this.logger.ErrorFormat("Unable to save changes to the battle №{0}! Exception: {1}", battle.Id, exception);
                }

                try
                {
                    this.UpdateTeamPoints(data, battle.FirstTeam);
                    this.UpdateTeamPoints(data, battle.SecondTeam);
                }
                catch (Exception exception)
                {
                    this.logger.ErrorFormat("Unable to update team points! Exception: {0}", exception);
                }

                // Next line removes the battle from the list. Fixes problem with retesting battles.
                this.processingBattleIds.Remove(battle.Id);
            }

            this.logger.Info("BattlesSimulatorJob stopped.");
        }

        public void Stop()
        {
            this.stopping = true;
        }

        private void ProcessBattle(AiPortalDbContext data, Battle battle)
        {
            this.logger.InfoFormat("Processing battle №{0} started.", battle.Id);

            var firstUpload = battle.FirstTeam.Uploads.OrderByDescending(x => x.Id).FirstOrDefault();
            var secondUpload = battle.SecondTeam.Uploads.OrderByDescending(x => x.Id).FirstOrDefault();

            if (firstUpload == null && secondUpload == null)
            {
                battle.Comment = "Both teams didn't upload any file.";
            }
            else if (firstUpload == null)
            {
                battle.Comment = $"Team {battle.FirstTeam.Name} didn't upload any file.";
            }
            else if (secondUpload == null)
            {
                battle.Comment = $"Team {battle.SecondTeam.Name} didn't upload any file.";
            }
            else
            {
                var simulationResult = this.gamesSimulator.SimulateGames(
                    firstUpload.FileContents,
                    secondUpload.FileContents,
                    battle.FirstTeam.Competition.GamesExecutorClassName);
                foreach (var gameResult in simulationResult.GameResults)
                {
                    var battleGameResult = new BattleGameResult
                                               {
                                                   BattleId = battle.Id,
                                                   BattleGameWinner = gameResult.Winner,
                                                   Report = gameResult.Report,
                                               };
                    battle.BattleGameResults.Add(battleGameResult);
                }

                battle.Comment = simulationResult.BattleComment;
            }

            this.logger.InfoFormat("Processing battle №{0} ended.", battle.Id);
        }

        private void UpdateTeamPoints(AiPortalDbContext data, Team team)
        {
            if (!data.Battles.Where(x => x.FirstTeamId == team.Id || x.SecondTeamId == team.Id).All(x => x.IsFinished))
            {
                // Update player points only when all battles are finished
                return;
            }

            var teamHasUploadedFile = team.Uploads.Any();
            var battles =
                data.Battles.Where(x => x.FirstTeamId == team.Id || x.SecondTeamId == team.Id)
                    .Select(
                        x =>
                        new
                            {
                                TeamIsFirst = x.FirstTeamId == team.Id,
                                OpponentHasUploadedFile = x.FirstTeamId == team.Id ? x.SecondTeam.Uploads.Any() : x.FirstTeam.Uploads.Any(),
                                BattlesWonByTeam = x.BattleGameResults.Count(
                                    res => res.BattleGameWinner == (x.FirstTeamId == team.Id ? BattleGameWinner.First : BattleGameWinner.Second)),
                            });

            var points = 0;

            foreach (var battle in battles)
            {
                if (teamHasUploadedFile && battle.OpponentHasUploadedFile)
                {
                    // Both players have submitted file, so the result from battles will be used
                    points += battle.BattlesWonByTeam;
                }
                else if (teamHasUploadedFile && !battle.OpponentHasUploadedFile)
                {
                    // The other player does not have uploaded file and current player has
                    points += 0;
                }
                else if (!teamHasUploadedFile && battle.OpponentHasUploadedFile)
                {
                    // The other player has uploaded file and current player hasn't
                    points += 0;
                }
                else if (!teamHasUploadedFile && !battle.OpponentHasUploadedFile)
                {
                    // Both players hasn't uploaded file
                    points += 0;
                }
            }

            // TODO: What about multithreading?
            // TODO: Depend on max games when scoring
            team.Points = points;
            this.logger.InfoFormat("Points for {0} updated.", team.Name);

            data.SaveChanges();
        }
    }
}
