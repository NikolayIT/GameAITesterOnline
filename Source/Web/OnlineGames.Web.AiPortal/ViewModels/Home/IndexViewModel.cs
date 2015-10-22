namespace OnlineGames.Web.AiPortal.ViewModels.Home
{
    using System.Collections.Generic;

    using OnlineGames.Web.AiPortal.ViewModels.Teams;

    public class IndexViewModel
    {
        public IEnumerable<TeamInfoViewModel> CurrentUserTeams { get; set; }

        public IEnumerable<IndexCompetitionViewModel> ActiveCompetitions { get; set; }
    }
}
