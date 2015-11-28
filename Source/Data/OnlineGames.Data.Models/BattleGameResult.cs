namespace OnlineGames.Data.Models
{
    using OnlineGames.Data.Common.Models;

    public class BattleGameResult : BaseModel<int>
    {
        public int BattleId { get; set; }

        public Battle Battle { get; set; }

        public string Report { get; set; }

        public BattleGameWinner BattleGameWinner { get; set; }
    }
}
