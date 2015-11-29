// <copyright file="BattlesSimulatorJob.cs" company="Nikolay Kostov (Nikolay.IT)">
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

    public class BattlesSimulatorJob : IJob
    {
        private readonly ILog logger;

        private readonly SynchronizedHashtable processingBattleIds;

        private bool stopping;

        public BattlesSimulatorJob(string name, SynchronizedHashtable processingBattleIds)
        {
            this.Name = name;

            this.logger = LogManager.GetLogger(name);
            this.logger.Info("BattlesSimulatorJob initializing...");

            this.stopping = false;
            this.processingBattleIds = processingBattleIds;

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
                    battle = data.Battles.Where(x => !x.IsFinished).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
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
                    this.ProcessBattle(data, battle);
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

            battle.IsFinished = true;
            battle.BattleGameResults.Clear();

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
                this.SimulateGames(firstUpload, secondUpload);
                this.UpdateTeamPoints(data, battle.FirstTeam);
                this.UpdateTeamPoints(data, battle.SecondTeam);
                battle.Comment = "Ready";
            }

            this.logger.InfoFormat("Processing battle №{0} ended.", battle.Id);
        }

        private void SimulateGames(Upload firstUpload, Upload secondUpload)
        {
            //// TODO: Add/return game simulations
        }

        private void UpdateTeamPoints(AiPortalDbContext data, Team team)
        {
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
                    // The other player does not have uploaded file and current player has => 1000 - 0
                    points += 1000;
                }
                else if (!teamHasUploadedFile && battle.OpponentHasUploadedFile)
                {
                    // The other player has uploaded file and current player hasn't => 0 - 1000
                    points += 0;
                }
                else if (!teamHasUploadedFile && !battle.OpponentHasUploadedFile)
                {
                    // Both players hasn't uploaded file => 500 - 500
                    points += 500;
                }
            }

            // TODO: What about multithreading
            // TODO: Depend on max games when scoring
            team.Points = points;
            this.logger.InfoFormat("Points for {0} updated.", team.Name);
        }
    }
}
