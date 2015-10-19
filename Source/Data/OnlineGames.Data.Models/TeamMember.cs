namespace OnlineGames.Data.Models
{
    using OnlineGames.Data.Common.Models;

    public class TeamMember : BaseModel<int>
    {
        public int TeamId { get; set; }

        public Team Team { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
