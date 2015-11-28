// <copyright file="IOrderable.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Data.Common.Models
{
    public interface IOrderable
    {
        int OrderBy { get; set; }
    }
}
