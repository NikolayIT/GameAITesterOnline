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

        public string CreatedById { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        public virtual ICollection<TeamMember> TeamMembers { get; set; }
    }
}
