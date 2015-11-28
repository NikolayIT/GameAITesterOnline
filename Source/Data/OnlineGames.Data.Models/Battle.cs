// <copyright file="Battle.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>

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
