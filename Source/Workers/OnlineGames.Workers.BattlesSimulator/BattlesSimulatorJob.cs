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
                    this.ProcessBattle(battle);
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

        private void ProcessBattle(Battle battle)
        {
            //// TODO: Update team points (multithreaded?)
            battle.IsFinished = true;
            battle.Comment = "Ready";

            //// TODO: Add game simulations
            battle.BattleGameResults.Clear();
        }
    }
}
