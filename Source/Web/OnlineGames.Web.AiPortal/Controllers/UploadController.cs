namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;
    using AutoMapper.QueryableExtensions;

    using OnlineGames.Web.AiPortal.ViewModels.Teams;

    [Authorize]
    public class UploadController : BaseController
    {
        private readonly IDbRepository<Team> teamsRepository;

        public UploadController(IDbRepository<Team> teamsRepository)
        {
            this.teamsRepository = teamsRepository;
        }

        public ActionResult Index(int id)
        {
            var team = this.teamsRepository.All().Where(x => x.Id == id).ProjectTo<TeamInfoViewModel>().FirstOrDefault();
            return this.View(team);
        }
    }
}
