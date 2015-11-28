// <copyright file="CompetitionsController.cs" company="Nikolay Kostov (Nikolay.IT)">
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
    using OnlineGames.Web.AiPortal.ViewModels.Competitions;

    public class CompetitionsController : BaseController
    {
        private readonly IDbRepository<Competition> competitionsRepository;

        public CompetitionsController(IDbRepository<Competition> competitionsRepository)
        {
            this.competitionsRepository = competitionsRepository;
        }

        [HttpGet]
        public ActionResult Info(int id)
        {
            var activeCompetitions =
                this.competitionsRepository.All()
                    .Where(x => x.IsActive && x.Id == id)
                    .ProjectTo<CompetitionViewModel>()
                    .FirstOrDefault();
            return this.View(activeCompetitions);
        }
    }
}
