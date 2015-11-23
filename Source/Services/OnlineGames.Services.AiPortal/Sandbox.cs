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
