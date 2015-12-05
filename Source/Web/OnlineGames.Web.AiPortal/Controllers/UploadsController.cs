// <copyright file="UploadsController.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.ViewModels.Upload;

    public class UploadsController : Controller
    {
        private readonly IDbRepository<Upload> uploadsRepository;

        public UploadsController(IDbRepository<Upload> uploadsRepository)
        {
            this.uploadsRepository = uploadsRepository;
        }

        public ActionResult All()
        {
            var model = this.uploadsRepository.All().ProjectTo<UploadInListViewModel>().ToList();
            return this.View(model);
        }
    }
}
