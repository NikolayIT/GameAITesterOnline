// <copyright file="AdminDashboardController.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.Web.Mvc;

    using OnlineGames.Common;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class AdminDashboardController : Controller
    {
        public ActionResult Index()
        {
            return null;
        }
    }
}
