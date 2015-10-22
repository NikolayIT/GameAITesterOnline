namespace OnlineGames.Web.AiPortal
{
    using System;
    using System.Data.Entity;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Script.Serialization;
    using System.Web.Security;

    using OnlineGames.Data;
    using OnlineGames.Data.Migrations;
    using OnlineGames.Web.AiPortal.Infrastructure;
    using OnlineGames.Web.AiPortal.Infrastructure.Mapping;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name must match first type name", Justification = "File name must be Global.asax.cs")]
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AiPortalDbContext, Configuration>());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute();
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = this.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var serializer = new JavaScriptSerializer();
                var userData = serializer.Deserialize<AiPortalUserData>(authTicket.UserData);
                HttpContext.Current.User = new AiPortalPrincipal(userData.UserName, userData.Roles);
            }
        }
    }
}
