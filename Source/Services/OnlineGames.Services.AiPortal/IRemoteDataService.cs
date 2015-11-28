// <copyright file="IRemoteDataService.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Services.AiPortal
{
    using System.Threading.Tasks;

    using OnlineGames.Services.AiPortal.Models;

    public interface IRemoteDataService
    {
        Task<CheckUserLoginApiModel> Login(string username, string password);
    }
}
