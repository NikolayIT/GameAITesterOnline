﻿// <copyright file="TeamMember.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Data.Models
{
    using OnlineGames.Data.Common.Models;

    public class TeamMember : BaseModel<int>
    {
        public int TeamId { get; set; }

        public virtual Team Team { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
