namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.Web.Mvc;

    public class CompetitionsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
