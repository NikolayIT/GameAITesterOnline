// <copyright file="BattlesSimulatorService.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator
{
    using System.Collections.Generic;
    using System.ServiceProcess;
    using System.Threading;

    using log4net;

    public class BattlesSimulatorService : ServiceBase
    {
        private static ILog logger;
        private readonly IList<Thread> threads;
        private readonly IList<IJob> jobs;

        public BattlesSimulatorService()
        {
            logger = LogManager.GetLogger("BattlesSimulatorService");
            logger.Info("BattlesSimulatorService initializing...");

            this.threads = new List<Thread>();
            this.jobs = new List<IJob>();

            // Shared among jobs
            var processingBattleIds = new SynchronizedHashtable();

            for (int i = 1; i <= Settings.ThreadsCount; i++)
            {
                IJob job = new BattlesSimulatorJob($"Job №{i}", processingBattleIds);
                var thread = new Thread(job.Start) { Name = $"Thread №{i}" };
                this.jobs.Add(job);
                this.threads.Add(thread);
            }

            logger.Info("BattlesSimulatorService initialized.");
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("BattlesSimulatorService starting...");

            foreach (var thread in this.threads)
            {
                logger.InfoFormat("Starting {0}...", thread.Name);
                thread.Start();
                logger.InfoFormat("{0} started.", thread.Name);
                Thread.Sleep(234);
            }

            logger.Info("BattlesSimulatorService started.");
        }

        protected override void OnStop()
        {
            logger.Info("BattlesSimulatorService stopping...");

            foreach (var job in this.jobs)
            {
                job.Stop();
                logger.InfoFormat("{0} stopped.", job.Name);
            }

            Thread.Sleep(10000);

            foreach (var thread in this.threads)
            {
                thread.Abort();
                logger.InfoFormat("{0} aborted.", thread.Name);
            }

            logger.Info("BattlesSimulatorService stopped.");
        }
    }
}
