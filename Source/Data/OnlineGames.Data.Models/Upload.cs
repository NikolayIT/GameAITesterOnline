namespace OnlineGames.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using OnlineGames.Data.Common.Models;

    public class Upload : BaseModel<int>
    {
        [Required]
        public Team Team { get; set; }

        [Required]
        public byte[] FileContents { get; set; }
    }
}
