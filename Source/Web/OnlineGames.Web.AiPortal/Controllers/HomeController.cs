namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

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
            // TODO: Replace with automapper
            var model = new IndexViewModel
                            {
                                ActiveCompetitions =
                                    this.competitionsRepository.All()
                                    .Where(x => x.IsActive)
                                    .Select(
                                        x =>
                                        new IndexCompetitionViewModel
                                            {
                                                Name = x.Name,
                                                Id = x.Id,
                                                Description = x.Description
                                            }),
                                CurrentUserTeams =
                                    this.teamsRepository.All()
                                    .Where(
                                        x =>
                                        x.TeamMembers.Any(
                                            tm => tm.User.UserName == this.User.Identity.Name))
                                    .Select(
                                        x =>
                                        new TeamInfoViewModel
                                            {
                                                Id = x.Id,
                                                Name = x.Name,
                                                TeamMembers =
                                                    x.TeamMembers.Select(
                                                        tm =>
                                                        new TeamMemberViewModel
                                                            {
                                                                UserName = tm.User.UserName,
                                                                AvatarUrl = tm.User.AvatarUrl
                                                            })
                                            })
                            };
            return this.View(model);
        }
    }
}
