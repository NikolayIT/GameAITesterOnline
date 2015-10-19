using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineGames.Web.AiPortal.Startup))]
namespace OnlineGames.Web.AiPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
