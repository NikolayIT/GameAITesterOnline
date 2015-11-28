// <copyright file="Program.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace OnlineGames.Workers.BattlesSimulator
{
    using System;
    using System.Diagnostics;
    using System.ServiceProcess;

    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            try
            {
                var servicesToRun = new ServiceBase[] { new Service1() };
                ServiceBase.Run(servicesToRun);
            }
            catch (Exception exception)
            {
                const string Source = "OnlineGames.Workers.BattlesSimulator";
                if (!EventLog.SourceExists(Source))
                {
                    EventLog.CreateEventSource(Source, "Application");
                }

                EventLog.WriteEntry(Source, exception.ToString(), EventLogEntryType.Error);
            }
        }
    }
}
