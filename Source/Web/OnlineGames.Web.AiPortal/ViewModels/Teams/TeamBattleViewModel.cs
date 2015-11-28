// <copyright file="TeamBattleViewModel.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.ViewModels.Teams
{
    using System;

    public class TeamBattleViewModel
    {
        public int Id { get; set; }

        public int OpponentTeamId { get; set; }

        public string OpponentTeam { get; set; }

        public bool Finished { get; set; }

        public int TeamWins { get; set; }

        public int OpponentWins { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string Comment { get; set; }
    }
}
