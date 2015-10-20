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
    using OnlineGames.Data;
    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;
    using OnlineGames.Services.AiPortal;
    using OnlineGames.Web.AiPortal.Infrastructure;
    using OnlineGames.Web.AiPortal.ViewModels.ExternalLogin;

    public class AccountController : Controller
    {
        private readonly IDbRepository<User> usersRepository;
        private readonly IDbRepository<Role> rolesRepository;

        // TODO: Replace with Ninject
        public AccountController()
            : this(new DbRepository<User>(new AiPortalDbContext()), new DbRepository<Role>(new AiPortalDbContext()))
        {
        }

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

            var dbUser = this.usersRepository.All().FirstOrDefault(x => x.UserName == remoteResult.UserName);
            if (dbUser == null)
            {
                dbUser = new User
                             {
                                 UserName = remoteResult.UserName,
                                 AvatarUrl = remoteResult.SmallAvatarUrl,
                                 Provider = UserProvider.TelerikAcademyUser
                             };
                if (remoteResult.IsAdmin)
                {
                    var adminRole =
                        this.rolesRepository.All().FirstOrDefault(x => x.Name == GlobalConstants.AdministratorRoleName);
                    dbUser.Roles.Add(adminRole);
                }

                this.usersRepository.Add(dbUser);
                this.usersRepository.Save();
            }

            var roles = dbUser.Roles.Select(x => x.Name).ToList();
            var userDataObject = new AiPortalUserData(dbUser.UserName, roles);

            var userDataAsString = JsonConvert.SerializeObject(userDataObject);
            var authTicket = new FormsAuthenticationTicket(
                1,
                model.UserName,
                DateTime.Now,
                DateTime.Now.AddDays(1), // Expire after
                false,
                userDataAsString);
            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            this.Response.Cookies.Add(faCookie);

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
