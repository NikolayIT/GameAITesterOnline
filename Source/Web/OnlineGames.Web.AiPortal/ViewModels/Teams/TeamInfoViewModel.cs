namespace OnlineGames.Web.AiPortal.ViewModels.Teams
{
    using System.Collections.Generic;

    public class TeamInfoViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<TeamMemberViewModel> TeamMembers { get; set; }
    }
}
