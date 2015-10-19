namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.Web.Mvc;

    using OnlineGames.Data;
    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var repo = new DbRepository<Team>(new AiPortalDbContext());
            repo.Add(new Team { Name = "YoYo" });
            repo.Save();
            return this.View();
        }
    }
}
