// <copyright file="IGamesSimulator.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator.GamesSimulator
{
    using OnlineGames.Data.Models;

    public interface IGamesSimulator
    {
        GamesSimulatorResult SimulateGames(byte[] firstAssemblyAsBytes, byte[] secondAssemblyAsBytes, string gamesExecutorClassName);
    }
}
