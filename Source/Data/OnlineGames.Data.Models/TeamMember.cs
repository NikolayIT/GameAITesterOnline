// <copyright file="TeamMember.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Data.Models
{
    using OnlineGames.Data.Common.Models;

    public class TeamMember : BaseModel<int>
    {
        public int TeamId { get; set; }

        public Team Team { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
