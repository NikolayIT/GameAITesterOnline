namespace OnlineGames.Data.Models
{
    using System.Collections.Generic;

    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<TeamMember> TeamMembers { get; set; }
    }

    public class TeamMember
    {
    }
}
