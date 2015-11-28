// <copyright file="AiPortalPrincipal.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.Infrastructure
{
    using System.Collections.Generic;
    using System.Security.Principal;

    public class AiPortalPrincipal : IPrincipal
    {
        private readonly ICollection<string> roles;

        public AiPortalPrincipal(string userName, ICollection<string> roles)
        {
            this.roles = roles;
            this.Identity = new GenericIdentity(userName);
        }

        public IIdentity Identity { get; }

        public bool IsInRole(string role)
        {
            return this.roles.Contains(role);
        }
    }
}
