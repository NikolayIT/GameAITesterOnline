// <copyright file="BattleGameResultViewModel.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.ViewModels.Battles
{
    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.Infrastructure.Mapping;

    public class BattleGameResultViewModel : IMapFrom<BattleGameResult>
    {
        public int Id { get; set; }

        public string Report { get; set; }

        public BattleGameWinner BattleGameWinner { get; set; }
    }
}
