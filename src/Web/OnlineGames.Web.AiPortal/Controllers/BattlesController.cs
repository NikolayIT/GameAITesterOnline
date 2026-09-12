// <copyright file="BattlesController.cs" company="Nikolay Kostov (Nikolay.IT)">
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
    using OnlineGames.Web.AiPortal.ViewModels.Battles;

    public class BattlesController : Controller
    {
        private readonly IDbRepository<Battle> battlesRepository;

        public BattlesController(IDbRepository<Battle> battlesRepository)
        {
            this.battlesRepository = battlesRepository;
        }

        public ActionResult Info(int id)
        {
            var model = this.battlesRepository.All().Where(x => x.Id == id).ProjectTo<BattleInfoViewModel>().FirstOrDefault();
            if (model == null)
            {
                return this.HttpNotFound("Battle not found!");
            }

            return this.View(model);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public ActionResult Restart(int id)
        {
            var battle = this.battlesRepository.GetById(id);
            battle.IsFinished = false;
            this.battlesRepository.Save();
            this.TempData["Info"] = "Battle restarted.";
            return this.RedirectToAction(nameof(this.Info), new { id });
        }

        public ActionResult All()
        {
            var model = this.battlesRepository.All().ProjectTo<BattleSimpleInfoViewModel>().ToList();
            return this.View(model);
        }
    }
}
