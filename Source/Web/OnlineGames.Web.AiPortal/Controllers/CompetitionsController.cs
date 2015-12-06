// <copyright file="CompetitionsController.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using OnlineGames.Common;
    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;
    using OnlineGames.Services.AiPortal.Battles;
    using OnlineGames.Web.AiPortal.ViewModels.Competitions;

    public class CompetitionsController : BaseController
    {
        private readonly IDbRepository<Competition> competitionsRepository;

        private readonly IDbRepository<Team> teamsRepository;

        private readonly IDbRepository<Battle> battlesRepository;

        private readonly IBattlesGenerator battlesGenerator;

        public CompetitionsController(
            IDbRepository<Competition> competitionsRepository,
            IDbRepository<Team> teamsRepository,
            IDbRepository<Battle> battlesRepository,
            IBattlesGenerator battlesGenerator)
        {
            this.competitionsRepository = competitionsRepository;
            this.teamsRepository = teamsRepository;
            this.battlesRepository = battlesRepository;
            this.battlesGenerator = battlesGenerator;
        }

        [HttpGet]
        public ActionResult Info(int id)
        {
            var competition =
                this.competitionsRepository.All()
                    .Where(x => x.IsActive && x.Id == id)
                    .ProjectTo<CompetitionViewModel>()
                    .FirstOrDefault();
            if (competition == null)
            {
                return this.HttpNotFound("Competition not found!");
            }

            foreach (var team in competition.Teams)
            {
                team.UnfinishedBattles =
                    this.battlesRepository.All()
                        .Count(x => !x.IsFinished && (x.FirstTeamId == team.Id || x.SecondTeamId == team.Id));
            }

            return this.View(competition);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public ActionResult CreateBattles(int id)
        {
            var newBattles = this.battlesGenerator.GenerateBattles(this.teamsRepository, this.battlesRepository, id);
            this.TempData["Info"] = $"{newBattles} new battles created successfully!";
            return this.RedirectToAction(nameof(this.Info), new { id });
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public ActionResult RestartBattles(int id)
        {
            var restartedBattles = this.battlesGenerator.RestartBattlesForCompetition(this.battlesRepository, id);
            this.TempData["Info"] = $"{restartedBattles} battles restarted successfully!";
            return this.RedirectToAction(nameof(this.Info), new { id });
        }
    }
}
