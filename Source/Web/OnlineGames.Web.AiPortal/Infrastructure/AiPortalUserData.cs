// <copyright file="AiPortalUserData.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.Infrastructure
{
    using System.Collections.Generic;

    public class AiPortalUserData
    {
        // ReSharper disable once UnusedMember.Global (required for JSON de-serializing)
        public AiPortalUserData()
        {
        }

        public AiPortalUserData(string userName, ICollection<string> roles)
        {
            this.UserName = userName;
            this.Roles = roles;
        }

        public string UserName { get; set; }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global (required for JSON de-serializing)
        // ReSharper disable once MemberCanBePrivate.Global (required for JSON de-serializing)
        public ICollection<string> Roles { get; set; }
    }
}
