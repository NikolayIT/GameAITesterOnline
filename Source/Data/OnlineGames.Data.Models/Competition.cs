namespace OnlineGames.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using OnlineGames.Data.Common.Models;

    public class Competition : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }

        public string Descriptions { get; set; }

        public bool IsActive { get; set; }

        public int MinimumParticipants { get; set; }

        public int MaximumParticipants { get; set; }
    }
}
