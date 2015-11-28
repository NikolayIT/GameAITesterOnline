// <copyright file="IBattlesGenerator.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Services.AiPortal.Battles
{
    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;

    public interface IBattlesGenerator
    {
        int GenerateBattles(IDbRepository<Team> teamsRepository, IDbRepository<Battle> battlesRepository, int competitionId);
    }
}
