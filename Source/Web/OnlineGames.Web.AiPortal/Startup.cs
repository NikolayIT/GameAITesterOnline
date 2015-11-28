// <copyright file="Startup.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineGames.Web.AiPortal.Startup))]
namespace OnlineGames.Web.AiPortal
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
