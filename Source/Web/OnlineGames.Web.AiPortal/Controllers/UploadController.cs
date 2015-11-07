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
    using OnlineGames.Services.AiPortal.Uploads;
    using OnlineGames.Web.AiPortal.ViewModels.Teams;
    using OnlineGames.Web.AiPortal.ViewModels.Upload;

    [Authorize]
    public class UploadController : BaseController
    {
        private readonly IDbRepository<Team> teamsRepository;

        private readonly IUploadFileValidator uploadFileValidator;

        public UploadController(IDbRepository<Team> teamsRepository, IUploadFileValidator uploadFileValidator)
        {
            this.teamsRepository = teamsRepository;
            this.uploadFileValidator = uploadFileValidator;
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
        public ActionResult Index(FileUploadInputModel model)
        {
            // TODO: Replace this.User.Identity.Name with identity provider for easier unit testing
            var team =
                this.teamsRepository.All()
                    .Where(
                        x => x.Id == model.Id && x.TeamMembers.Any(tm => tm.User.UserName == this.User.Identity.Name))
                    .ProjectTo<TeamInfoViewModel>()
                    .FirstOrDefault();

            if (team == null)
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.Forbidden,
                    "You do not have permissions to upload files for this team!");
            }

            var validateFileResult = this.uploadFileValidator.ValidateFile(
                model.AiFile.FileName,
                model.AiFile.ContentLength,
                model.AiFile.InputStream);
            if (!validateFileResult.IsValid)
            {
                this.ViewBag.Error = validateFileResult.Error;
                return this.View(team);
            }
            else
            {
                // TODO: Save in database
                return this.RedirectToAction("Index");
            }
        }
    }
}
