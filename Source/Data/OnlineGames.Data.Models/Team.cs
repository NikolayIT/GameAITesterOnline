// <copyright file="Team.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Data.Models
{
    using System.Collections.Generic;

    using OnlineGames.Data.Common.Models;

    public class Team : BaseModel<int>
    {
        public Team()
        {
            this.TeamMembers = new HashSet<TeamMember>();
            this.Uploads = new HashSet<Upload>();
        }

        public string Name { get; set; }

        public int CompetitionId { get; set; }

        // TODO: Always update this cache value
        public int Points { get; set; }

        public virtual Competition Competition { get; set; }

        public virtual ICollection<TeamMember> TeamMembers { get; set; }

        public virtual ICollection<Upload> Uploads { get; set; }
    }
}
