namespace OnlineGames.Data.Models
{
    using System.Collections.Generic;

    using OnlineGames.Data.Common.Models;

    public class Battle : BaseModel<int>
    {
        public Battle()
        {
            this.BattleGameResults = new HashSet<BattleGameResult>();
        }

        public int FirstTeamId { get; set; }

        public Team FirstTeam { get; set; }

        public int SecondTeamId { get; set; }

        public Team SecondTeam { get; set; }

        public virtual ICollection<BattleGameResult> BattleGameResults { get; set; }
    }
}
