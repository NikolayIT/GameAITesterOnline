// <copyright file="CreateTeamViewModel.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.ViewModels.Teams
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateTeamViewModel
    {
        public CreateTeamViewModel()
        {
            this.TeamMembers = new List<string>();
        }

        [Display(Name = "Competition")]
        public int CompetitionId { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 6)]
        [RegularExpression(@"[0-9a-zA-Z_\-.]+")]
        [Display(Name = "Team name")]
        public string Name { get; set; }

        public IList<string> TeamMembers { get; set; }
    }
}
