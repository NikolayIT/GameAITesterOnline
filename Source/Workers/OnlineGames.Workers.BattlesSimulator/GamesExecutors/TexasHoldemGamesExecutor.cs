// <copyright file="TexasHoldemGamesExecutor.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator.GamesExecutors
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    using OnlineGames.Data.Models;
    using OnlineGames.Workers.BattlesSimulator.GamesSimulator;

    using TexasHoldem.Logic.GameMechanics;
    using TexasHoldem.Logic.Players;

    public class TexasHoldemGamesExecutor : BaseGamesExecutor
    {
        public override IEnumerable<SingleGameResult> SimulateGames(Assembly firstAssembly, Assembly secondAssembly, int count)
        {
            var firstPlayer = new TexasHoldemPlayerDirector(this.LoadPlayer<IPlayer>(firstAssembly));
            var secondPlayer = new TexasHoldemPlayerDirector(this.LoadPlayer<IPlayer>(secondAssembly));

            var gameResults = new List<SingleGameResult>();
            for (var i = 0; i < count; i++)
            {
                var game = i % 2 == 0
                               ? new TwoPlayersTexasHoldemGame(firstPlayer, secondPlayer)
                               : new TwoPlayersTexasHoldemGame(secondPlayer, firstPlayer);

                var stopwatch = Stopwatch.StartNew();
                var winner = game.Start();
                var elapsed = stopwatch.Elapsed;

                var firstToPlay = i % 2 == 0 ? "FirstPlayer" : "SecondPlayer";
                var winnerAsString = winner.Name == firstPlayer.Name ? "FirstPlayer" : "SecondPlayer";
                var report = $"First: {firstToPlay}; Winner: {winnerAsString} ({game.HandsPlayed} hands); Time: {elapsed}; Crashes: {firstPlayer.Crashes} - {secondPlayer.Crashes}; Time limits: {firstPlayer.TimeOuts} - {secondPlayer.TimeOuts}";
                if (firstPlayer.FirstCrash != null)
                {
                    report += $"; First player first exception: {firstPlayer.FirstCrash}";
                }

                if (secondPlayer.FirstCrash != null)
                {
                    report += $"; Second player first exception: {secondPlayer.FirstCrash}";
                }

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
