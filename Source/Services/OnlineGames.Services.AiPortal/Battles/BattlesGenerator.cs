// <copyright file="BattlesGenerator.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Services.AiPortal.Battles
{
    using System.Linq;

    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;

    public class BattlesGenerator : IBattlesGenerator
    {
        public void GenerateBattles(IDbRepository<Team> teamsRepository, IDbRepository<Battle> battlesRepository, int competitionId)
        {
            var teams = teamsRepository.All().Where(x => x.CompetitionId == competitionId).Select(x => new { x.Id }).ToList();
            for (var i = 0; i < teams.Count; i++)
            {
                for (int j = i + 1; j < teams.Count; j++)
                {
                    var firstTeam = teams[i];
                    var secondTeam = teams[j];
                    if (battlesRepository.All().Any(
                        x =>
                        (x.FirstTeamId == firstTeam.Id && x.SecondTeamId == secondTeam.Id)
                        || (x.FirstTeamId == secondTeam.Id && x.FirstTeamId == firstTeam.Id)))
                    {
                        // Battle already exists
                        continue;
                    }

                    battlesRepository.Add(new Battle { FirstTeamId = firstTeam.Id, SecondTeamId = secondTeam.Id });
                }
            }

            battlesRepository.Save();
        }
    }
}
