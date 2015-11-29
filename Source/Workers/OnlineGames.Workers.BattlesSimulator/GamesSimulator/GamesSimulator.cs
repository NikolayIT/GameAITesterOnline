// <copyright file="GamesSimulator.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator.GamesSimulator
{
    public class GamesSimulator : IGamesSimulator
    {
        public GamesSimulatorResult SimulateGames(byte[] firstAssemblyAsBytes, byte[] secondAssemblyAsBytes)
        {
            return new GamesSimulatorResult("Simulation not implemented!");
        }
    }
}
