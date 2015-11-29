// <copyright file="BaseGamesExecutor.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator.GamesExecutors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using OnlineGames.Workers.BattlesSimulator.GamesSimulator;

    public abstract class BaseGamesExecutor : IGamesExecutor
    {
        public abstract IEnumerable<SingleGameResult> SimulateGames(Assembly firstAssembly, Assembly secondAssembly, int count);

        protected T LoadPlayer<T>(Assembly assembly)
            where T : class
        {
            var playerClasses =
                assembly.GetTypes()
                    .Where(x => x.IsPublic && x.IsClass && !x.IsAbstract && typeof(T).IsAssignableFrom(x)).ToList();
            if (playerClasses.Count > 1)
            {
                throw new GameSimulationException($"More than one public inheritant of IPlayer found in {assembly.FullName}");
            }

            if (playerClasses.Count == 0)
            {
                throw new GameSimulationException($"More than one public inheritant of IPlayer found in {assembly.FullName}");
            }

            var playerClass = playerClasses[0];

            // TODO: Time limit (if available use decorator from the library)
            // TODO: var sandbox = Sandbox.CreateSandbox();
            // TODO: firstInstance = sandbox.CreateInstanceFromAndUnwrap(playerClass.Assembly.FullName, playerClass.FullName) as IPlayer;
            // TODO: secondInstance = sandbox.CreateInstanceFromAndUnwrap(playerClass.Assembly.FullName, playerClass.FullName) as IPlayer;
            var instance = Activator.CreateInstance(playerClass) as T;
            return instance;
        }
    }
}
