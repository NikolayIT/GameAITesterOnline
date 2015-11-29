// <copyright file="TexasHoldemGamesExecutor.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator.GamesExecutors
{
    using System.Collections.Generic;
    using System.Reflection;

    using OnlineGames.Data.Models;
    using OnlineGames.Workers.BattlesSimulator.GamesSimulator;

    using TexasHoldem.Logic.GameMechanics;
    using TexasHoldem.Logic.Players;

    public class TexasHoldemGamesExecutor : BaseGamesExecutor
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
                var game = i % 2 == 0
                               ? new TwoPlayersTexasHoldemGame(firstPlayer, secondPlayer)
                               : new TwoPlayersTexasHoldemGame(secondPlayer, firstPlayer);

                var winner = game.Start();

                var firstToPlay = i % 2 == 0 ? "FirstPlayer" : "SecondPlayer";
                var winnerAsString = winner.Name == firstPlayer.Name ? "FirstPlayer" : "SecondPlayer";
                var report = $"First: {firstToPlay}; Winner: {winnerAsString} (in {game.HandsPlayed} hands)";

                var gameResult =
                    new SingleGameResult(
                        winner.Name == firstPlayer.Name ? BattleGameWinner.First : BattleGameWinner.Second,
                        report);

                gameResults.Add(gameResult);
            }

            return gameResults;
        }
    }
}
