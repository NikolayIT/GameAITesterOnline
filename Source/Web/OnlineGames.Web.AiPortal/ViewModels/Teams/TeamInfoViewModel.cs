namespace OnlineGames.Web.AiPortal.ViewModels.Teams
{
    using System.Collections.Generic;

    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.Infrastructure.Mapping;

    public class TeamInfoViewModel : IMapFrom<Team>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<TeamMemberViewModel> TeamMembers { get; set; }
    }
}
