// <copyright file="UploadController.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;
    using OnlineGames.Services.AiPortal.Battles;
    using OnlineGames.Services.AiPortal.Uploads;
    using OnlineGames.Services.AiPortal.Uploads.LibraryValidators;
    using OnlineGames.Web.AiPortal.Infrastructure;
    using OnlineGames.Web.AiPortal.ViewModels.Teams;
    using OnlineGames.Web.AiPortal.ViewModels.Upload;

    [Authorize]
    public class UploadController : BaseController
    {
        private readonly IDbRepository<Team> teamsRepository;

        private readonly IDbRepository<Upload> uploadRepository;

        private readonly IDbRepository<Battle> battlesRepository;

        private readonly IUploadFileValidator uploadFileValidator;

        private readonly IBattlesGenerator battlesGenerator;

        public UploadController(
            IDbRepository<Team> teamsRepository,
            IDbRepository<Upload> uploadRepository,
            IDbRepository<Battle> battlesRepository,
            IUploadFileValidator uploadFileValidator,
            IBattlesGenerator battlesGenerator)
        {
            this.teamsRepository = teamsRepository;
            this.uploadRepository = uploadRepository;
            this.battlesRepository = battlesRepository;
            this.uploadFileValidator = uploadFileValidator;
            this.battlesGenerator = battlesGenerator;
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
            // TODO: 6/12 hours restriction
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
            var libraryValidator = this.uploadFileValidator.CreateLibraryValidator(libraryValidatorClassName);
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

            // Save in the database
            var upload = new Upload
                             {
                                 TeamId = team.Id,
                                 FileContents = validateFileResult.FileContent,
                                 FileName = model.AiFile.FileName
                             };
            this.uploadRepository.Add(upload);
            this.uploadRepository.Save();

            this.battlesGenerator.RestartBattlesForTeam(this.battlesRepository, team.Id);

            this.TempData["Info"] = "File uploaded successfully!";
            return this.RedirectToAction("Info", "Teams", new { id = team.Id });
        }

        [HttpGet]
        public ActionResult Download(int id)
        {
            var upload =
                this.uploadRepository.All()
                    .Where(x => x.Id == id)
                    .Select(
                        x =>
                        new
                            {
                                TeamMembers = x.Team.TeamMembers.Select(tm => tm.User.UserName),
                                x.FileContents,
                                x.CreatedOn,
                                x.FileName
                            })
                    .FirstOrDefault();
            if (upload == null)
            {
                return this.HttpNotFound("Upload file not found.");
            }

            if (!upload.TeamMembers.Contains(this.User.Identity.Name, StringComparer.InvariantCultureIgnoreCase)
                && !this.User.IsAdmin())
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.Forbidden,
                    "You do not have permissions to download this file!");
            }

            return this.File(upload.FileContents, "application/x-msdownload", $"{upload.FileName}_{upload.CreatedOn.ToLocalTime()}.dll");
        }
    }
}
