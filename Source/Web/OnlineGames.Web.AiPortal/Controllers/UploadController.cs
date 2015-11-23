namespace OnlineGames.Web.AiPortal.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;
    using OnlineGames.Services.AiPortal.Uploads;
    using OnlineGames.Services.AiPortal.Uploads.LibraryValidators;
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
            // TODO: 12 hours restriction
            // TODO: Replace this.User.Identity.Name with identity provider for easier unit testing
            var teamQuery =
                this.teamsRepository.All()
                    .Where(
                        x => x.Id == model.Id && x.TeamMembers.Any(tm => tm.User.UserName == this.User.Identity.Name));
            var team = teamQuery.ProjectTo<TeamInfoViewModel>().FirstOrDefault();

            if (team == null)
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.Forbidden,
                    "You do not have permissions to upload files for this team!");
            }

            var libraryValidatorClassName = teamQuery.Select(x => x.Competition.LibraryValidatorClassName).FirstOrDefault();
            var libraryValidator = libraryValidatorClassName != null
                                       ? Activator.CreateInstance(
                                           typeof(ILibraryValidator).Assembly.FullName,
                                           libraryValidatorClassName).Unwrap() as ILibraryValidator
                                       : null;
            var validateFileResult = this.uploadFileValidator.ValidateFile(
                model.AiFile.FileName,
                model.AiFile.ContentLength,
                model.AiFile.InputStream,
                libraryValidator);
            if (!validateFileResult.IsValid)
            {
                this.ViewBag.Error = validateFileResult.Error;
                return this.View(team);
            }
            else
            {
                // TODO: Save in database
                // TODO: Initiate AI battles
                this.TempData["Info"] = "File uploaded successfully!";
                return this.RedirectToAction("Info", "Teams", new { id = team.Id });
            }
        }
    }
}
