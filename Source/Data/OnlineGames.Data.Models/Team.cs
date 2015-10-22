namespace OnlineGames.Data.Models
{
    using System.Collections.Generic;

    using OnlineGames.Data.Common.Models;

    public class Team : BaseModel<int>
    {
        public Team()
        {
            this.TeamMembers = new HashSet<TeamMember>();
        }

        public string Name { get; set; }

        public virtual ICollection<TeamMember> TeamMembers { get; set; }

        public int CompetitionId { get; set; }

        public virtual Competition Competition { get; set; }
    }
}
