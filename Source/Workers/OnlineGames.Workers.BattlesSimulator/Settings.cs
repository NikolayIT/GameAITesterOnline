// <copyright file="Settings.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator
{
    using System;
    using System.Configuration;

    using log4net;

    public static class Settings
    {
        private static readonly ILog Logger;

        static Settings()
        {
            Logger = LogManager.GetLogger("Settings");
        }

        public static int ThreadsCount => GetSettingOrDefault("ThreadsCount", 2);

        private static string GetSetting(string settingName)
        {
            if (ConfigurationManager.AppSettings[settingName] == null)
            {
                Logger.FatalFormat("{0} setting not found in App.config file!", settingName);
                throw new Exception($"{settingName} setting not found in App.config file!");
            }

            return ConfigurationManager.AppSettings[settingName];
        }

        private static T GetSettingOrDefault<T>(string settingName, T defaultValue)
        {
            if (ConfigurationManager.AppSettings[settingName] == null)
            {
                return defaultValue;
            }

            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[settingName], typeof(T));
        }
    }
}
