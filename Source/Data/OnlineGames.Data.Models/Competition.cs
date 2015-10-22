namespace OnlineGames.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using OnlineGames.Data.Common.Models;

    public class Competition : BaseModel<int>
    {
        public Competition()
        {
            this.Teams = new HashSet<Team>();
        }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int MinimumParticipants { get; set; }

        public int MaximumParticipants { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
