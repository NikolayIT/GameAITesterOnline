// <copyright file="SingleGameResult.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator.GamesSimulator
{
    using OnlineGames.Data.Models;

    public class SingleGameResult
    {
        public SingleGameResult(BattleGameWinner winner, string report)
        {
            this.Winner = winner;
            this.Report = report;
        }

        public BattleGameWinner Winner { get; }

        public string Report { get; }
    }
}
