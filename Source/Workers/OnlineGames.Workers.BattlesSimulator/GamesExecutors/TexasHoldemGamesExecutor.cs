// <copyright file="TexasHoldemGamesExecutor.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator.GamesExecutors
{
    using System.Collections.Generic;
    using System.Reflection;

    using OnlineGames.Workers.BattlesSimulator.GamesSimulator;

    public class TexasHoldemGamesExecutor : IGamesExecutor
    {
        public IEnumerable<SingleGameResult> SimulateGames(Assembly firstAssembly, Assembly secondAssembly, int count)
        {
            var gameResults = new List<SingleGameResult>();
            return gameResults;
        }
    }
}
