// <copyright file="HomeController.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.ViewModels.Home;
    using OnlineGames.Web.AiPortal.ViewModels.Teams;

    public class HomeController : BaseController
    {
        private readonly IDbRepository<Competition> competitionsRepository;

        private readonly IDbRepository<Team> teamsRepository;

        public HomeController(IDbRepository<Competition> competitionsRepository, IDbRepository<Team> teamsRepository)
        {
            this.competitionsRepository = competitionsRepository;
            this.teamsRepository = teamsRepository;
        }

        public ActionResult Index()
        {
            var model = new IndexViewModel
                            {
                                ActiveCompetitions =
                                    this.competitionsRepository.All().Where(x => x.IsActive).ProjectTo<IndexCompetitionViewModel>(),
                                CurrentUserTeams =
                                    this.teamsRepository.All()
                                    .Where(x => x.TeamMembers.Any(tm => tm.User.UserName == this.User.Identity.Name))
                                    .ProjectTo<TeamInfoViewModel>()
                            };
            return this.View(model);
        }
    }
}
