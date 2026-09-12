// <copyright file="CheckUserLoginApiModel.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Services.AiPortal.Models
{
    public class CheckUserLoginApiModel
    {
        public bool IsValid { get; set; }

        public string UserName { get; set; }

        public string SmallAvatarUrl { get; set; }

        public bool IsAdmin { get; set; }
    }
}
