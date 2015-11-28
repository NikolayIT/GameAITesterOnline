// <copyright file="FileUploadInputModel.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.ViewModels.Upload
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class FileUploadInputModel
    {
        public int Id { get; set; }

        // TODO: Add URL validation
        [Required]
        public string SourceCodeRepository { get; set; }

        public HttpPostedFileBase AiFile { get; set; }
    }
}
