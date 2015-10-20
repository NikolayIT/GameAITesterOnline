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
