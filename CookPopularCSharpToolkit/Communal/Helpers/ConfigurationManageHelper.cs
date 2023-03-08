/*
 * Description：ConfigurationManageHelper 
 * Author： Chance.Zheng
 * Create Time: 2023-02-23 11:23:06
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CookPopularCSharpToolkit.Communal
{
    public class ConfigurationManageHelper
    {
        public static void AddItem(string key, string value, string sectionName = "appSettings")
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings.Add(key, value);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(sectionName);
        }

        public static void DeleteItem(string key, string sectionName = "appSettings")
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings.Remove(key);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(sectionName);
        }

        public static void ModifyItem(string key, string value, string sectionName = "appSettings")
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(sectionName);
        }

        public static string ReadItem(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}
