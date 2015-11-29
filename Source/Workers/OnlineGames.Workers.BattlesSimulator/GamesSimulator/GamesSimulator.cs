// <copyright file="GamesSimulator.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator.GamesSimulator
{
    using System;
    using System.Reflection;

    using OnlineGames.Workers.BattlesSimulator.GamesExecutors;

    public class GamesSimulator : IGamesSimulator
    {
        public GamesSimulatorResult SimulateGames(byte[] firstAssemblyAsBytes, byte[] secondAssemblyAsBytes, string gamesExecutorClassName)
        {
            // Create assemblies
            Assembly firstAssembly;
            Assembly secondAssembly;
            try
            {
                firstAssembly = Assembly.Load(firstAssemblyAsBytes);
                secondAssembly = Assembly.Load(secondAssemblyAsBytes);
            }
            catch (Exception ex)
            {
                return new GamesSimulatorResult($"Could not load one or more of the assemblies: {ex}");
            }

            IGamesExecutor gamesExecutor;
            try
            {
                gamesExecutor = this.CreateGamesExecutor(gamesExecutorClassName);
            }
            catch (Exception ex)
            {
                return new GamesSimulatorResult($"Could not load the games executor class: {ex}");
            }

            return new GamesSimulatorResult("Simulation not implemented!");
        }

        private IGamesExecutor CreateGamesExecutor(string fullClassName)
        {
            var libraryValidator = fullClassName != null
                                       ? Activator.CreateInstance(
                                           typeof(IGamesExecutor).Assembly.FullName,
                                           fullClassName).Unwrap() as IGamesExecutor
                                       : null;
            return libraryValidator;
        }
    }
}
