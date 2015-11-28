// <copyright file="UploadViewModel.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.ViewModels.Teams
{
    using System;

    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.Infrastructure.Mapping;

    public class UploadViewModel : IMapFrom<Upload>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FileName { get; set; }
    }
}
