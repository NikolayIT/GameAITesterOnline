namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.Web.Mvc;

    public class ExternalLoginController : Controller
    {
        [HttpGet]
        public ActionResult LoginWithTelerikAcademy()
        {
            return this.View();
        }
    }
}
