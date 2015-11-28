// <copyright file="BattleGameResult.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>

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
