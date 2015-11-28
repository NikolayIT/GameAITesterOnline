// <copyright file="IndexViewModel.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.ViewModels.Home
{
    using System.Collections.Generic;

    using OnlineGames.Web.AiPortal.ViewModels.Teams;

    public class IndexViewModel
    {
        public IEnumerable<TeamInfoViewModel> CurrentUserTeams { get; set; }

        public IEnumerable<IndexCompetitionViewModel> ActiveCompetitions { get; set; }
    }
}
