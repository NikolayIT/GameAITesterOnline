// <copyright file="SantaseGamesExecutor.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator.GamesExecutors
{
    using System.Collections.Generic;
    using System.Reflection;

    using OnlineGames.Data.Models;
    using OnlineGames.Workers.BattlesSimulator.GamesSimulator;

    using Santase.Logic;
    using Santase.Logic.GameMechanics;
    using Santase.Logic.Players;

    public class SantaseGamesExecutor : BaseGamesExecutor
    {
        public override IEnumerable<SingleGameResult> SimulateGames(Assembly firstAssembly, Assembly secondAssembly, int count)
        {
            // TODO: Decorate with time and memory limit
            // TODO: What if someone crashes?
            var firstPlayer = this.LoadPlayer<IPlayer>(firstAssembly);
            var secondPlayer = this.LoadPlayer<IPlayer>(secondAssembly);

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
    }
}
