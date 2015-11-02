namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.ViewModels.Competitions;

    public class CompetitionsController : BaseController
    {
        private readonly IDbRepository<Competition> competitionsRepository;

        public CompetitionsController(IDbRepository<Competition> competitionsRepository)
        {
            this.competitionsRepository = competitionsRepository;
        }

        [HttpGet]
        public ActionResult Info(int id)
        {
            var activeCompetitions =
                this.competitionsRepository.All()
                    .Where(x => x.IsActive && x.Id == id)
                    .ProjectTo<CompetitionViewModel>()
                    .FirstOrDefault();
            return this.View(activeCompetitions);
        }
    }
}
