// <copyright file="TeamInfoViewModel.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.ViewModels.Teams
{
    using System.Collections.Generic;

    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.Infrastructure.Mapping;

    public class TeamInfoViewModel : IMapFrom<Team>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Points { get; set; }

        public int UnfinishedBattles { get; set; }

        public IEnumerable<TeamMemberViewModel> TeamMembers { get; set; }

        public IEnumerable<UploadViewModel> Uploads { get; set; }

        public IEnumerable<TeamBattleViewModel> Battles { get; set; }
    }
}
