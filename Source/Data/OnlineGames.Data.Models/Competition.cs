// <copyright file="Competition.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using OnlineGames.Data.Common.Models;

    public class Competition : BaseModel<int>
    {
        public Competition()
        {
            this.Teams = new HashSet<Team>();
        }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int MinimumParticipants { get; set; }

        public int MaximumParticipants { get; set; }

        [DefaultValue(1000)]
        public int GamesToPlayForEachBattle { get; set; }

        public string LibraryValidatorClassName { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
