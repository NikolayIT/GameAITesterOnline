namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.ViewModels.Home;
    using OnlineGames.Web.AiPortal.ViewModels.Teams;

    public class HomeController : Controller
    {
        private readonly IDbRepository<Competition> competitionsRepository;

        private readonly IDbRepository<Team> teamsRepository;

        public HomeController(IDbRepository<Competition> competitionsRepository, IDbRepository<Team> teamsRepository)
        {
            this.competitionsRepository = competitionsRepository;
            this.teamsRepository = teamsRepository;
        }

        public ActionResult Index()
        {
            var model = new IndexViewModel();
            model.ActiveCompetitions =
                this.competitionsRepository.All().Where(x => x.IsActive).ProjectTo<IndexCompetitionViewModel>();
            model.CurrentUserTeams =
                this.teamsRepository.All()
                    .Where(x => x.TeamMembers.Any(tm => tm.User.UserName == this.User.Identity.Name))
                    .ProjectTo<TeamInfoViewModel>();
            return this.View(model);
        }
    }
}
