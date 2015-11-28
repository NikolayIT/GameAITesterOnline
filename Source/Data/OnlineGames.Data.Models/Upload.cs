// <copyright file="Upload.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using OnlineGames.Data.Common.Models;

    public class Upload : BaseModel<int>
    {
        public int TeamId { get; set; }

        public virtual Team Team { get; set; }

        public string FileName { get; set; }

        [Required]
        public byte[] FileContents { get; set; }
    }
}
