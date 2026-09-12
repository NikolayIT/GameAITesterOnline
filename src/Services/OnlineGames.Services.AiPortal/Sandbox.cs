// <copyright file="Sandbox.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Services.AiPortal
{
    using System;
    using System.Security;
    using System.Security.Permissions;

    public static class Sandbox
    {
        public static AppDomain CreateSandbox()
        {
            var setup = new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase };
            var permissionSet = new PermissionSet(PermissionState.None);
            permissionSet.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.NoFlags));
            permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            permissionSet.AddPermission(new FileIOPermission(PermissionState.Unrestricted)); // TODO: !!!
            var appDomain = AppDomain.CreateDomain("Sandbox", null, setup, permissionSet);
            return appDomain;
        }
    }
}
