namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.ViewModels.Teams;

    [Authorize]
    public class UploadController : BaseController
    {
        private readonly IDbRepository<Team> teamsRepository;

        public UploadController(IDbRepository<Team> teamsRepository)
        {
            this.teamsRepository = teamsRepository;
        }

        [HttpGet]
        public ActionResult Index(int id)
        {
            // TODO: Replace this.User.Identity.Name with identity provider for easier unit testing
            var team =
                this.teamsRepository.All()
                    .Where(x => x.Id == id && x.TeamMembers.Any(tm => tm.User.UserName == this.User.Identity.Name))
                    .ProjectTo<TeamInfoViewModel>()
                    .FirstOrDefault();

            if (team == null)
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.Forbidden,
                    "You do not have permissions to upload files for this team!");
            }

            return this.View(team);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }
    }
}
