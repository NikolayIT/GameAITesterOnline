// <copyright file="SantaseGamesExecutor.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator.GamesExecutors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using OnlineGames.Data.Models;
    using OnlineGames.Workers.BattlesSimulator.GamesSimulator;

    using Santase.Logic;
    using Santase.Logic.GameMechanics;
    using Santase.Logic.Players;

    public class SantaseGamesExecutor : IGamesExecutor
    {
        public IEnumerable<SingleGameResult> SimulateGames(Assembly firstAssembly, Assembly secondAssembly, int count)
        {
            // TODO: Decorate with time and memory limit
            // TODO: What if someone crashes?
            var firstPlayer = this.LoadPlayer(firstAssembly);
            var secondPlayer = this.LoadPlayer(firstAssembly);

            var gameResults = new List<SingleGameResult>();
            for (var i = 0; i < count; i++)
            {
                var game = new SantaseGame(firstPlayer, secondPlayer);
                var firstToPlay = i % 2 == 0 ? PlayerPosition.FirstPlayer : PlayerPosition.SecondPlayer;
                var gameWinner = game.Start(firstToPlay);

                var report = $"First: {firstToPlay}; Result: {game.FirstPlayerTotalPoints} - {game.SecondPlayerTotalPoints} (in {game.RoundsPlayed} rounds)";
                var gameResult =
                    new SingleGameResult(
                        gameWinner == PlayerPosition.FirstPlayer ? BattleGameWinner.First : BattleGameWinner.Second,
                        report);

                gameResults.Add(gameResult);
            }

            return gameResults;
        }

        private IPlayer LoadPlayer(Assembly assembly)
        {
            var playerClasses =
                assembly.GetTypes()
                    .Where(x => x.IsPublic && x.IsClass && !x.IsAbstract && typeof(IPlayer).IsAssignableFrom(x)).ToList();
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
            var instance = Activator.CreateInstance(playerClass) as IPlayer;
            return instance;
        }
    }
}
