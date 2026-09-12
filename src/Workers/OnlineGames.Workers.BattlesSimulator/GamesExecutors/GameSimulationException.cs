// <copyright file="GameSimulationException.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator.GamesExecutors
{
    using System;

    public class GameSimulationException : Exception
    {
        public GameSimulationException(string message)
            : base(message)
        {
        }
    }
}
