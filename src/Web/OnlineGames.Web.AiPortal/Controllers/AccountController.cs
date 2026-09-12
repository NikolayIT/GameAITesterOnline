// <copyright file="AccountController.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using Newtonsoft.Json;

    using OnlineGames.Common;
    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;
    using OnlineGames.Services.AiPortal;
    using OnlineGames.Web.AiPortal.Infrastructure;
    using OnlineGames.Web.AiPortal.ViewModels.Account;

    public class AccountController : BaseController
    {
        private readonly IDbRepository<User> usersRepository;
        private readonly IDbRepository<Role> rolesRepository;

        public AccountController(IDbRepository<User> usersRepository, IDbRepository<Role> rolesRepository)
        {
            this.usersRepository = usersRepository;
            this.rolesRepository = rolesRepository;
        }

        [HttpGet]
        public ActionResult LoginWithTelerikAcademy()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginWithTelerikAcademy(ExternalLoginViewModel model, string returnUrl)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var remoteResult = await new RemoteDataService().Login(model.UserName, model.Password);
            if (remoteResult == null || !remoteResult.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return this.View(model);
            }

            var databaseUser = this.usersRepository.All().FirstOrDefault(x => x.UserName == remoteResult.UserName);
            if (databaseUser == null)
            {
                databaseUser = new User
                             {
                                 UserName = remoteResult.UserName,
                                 AvatarUrl = remoteResult.SmallAvatarUrl,
                                 Provider = UserProvider.TelerikAcademyUser,
                             };
                if (remoteResult.IsAdmin)
                {
                    var adminRole =
                        this.rolesRepository.All().FirstOrDefault(x => x.Name == GlobalConstants.AdministratorRoleName);
                    databaseUser.Roles.Add(adminRole);
                }

                this.usersRepository.Add(databaseUser);
                this.usersRepository.Save();
            }

            var roles = databaseUser.Roles.Select(x => x.Name).ToList();
            var userDataObject = new AiPortalUserData(databaseUser.UserName, roles);

            var userDataAsString = JsonConvert.SerializeObject(userDataObject);
            var authTicket = new FormsAuthenticationTicket(
                1,
                model.UserName,
                DateTime.Now,
                DateTime.Now.AddDays(1), // Expire after
                false,
                userDataAsString);
            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            this.Response.Cookies.Add(cookie);

            return this.RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return this.RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }
    }
}
