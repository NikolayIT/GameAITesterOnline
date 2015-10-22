namespace OnlineGames.Web.AiPortal.ViewModels.Competitions
{
    using System.Collections.Generic;

    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.Infrastructure.Mapping;
    using OnlineGames.Web.AiPortal.ViewModels.Teams;

    public class CompetitionViewModel : IMapFrom<Competition>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<TeamInfoViewModel> Teams { get; set; }
    }
}
