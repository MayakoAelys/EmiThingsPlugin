﻿using Dalamud.Configuration;
using Dalamud.Plugin;
using System.Collections.Generic;

namespace EmiThingsPlugin
{
    public class Configuration : IPluginConfiguration
    {
        int IPluginConfiguration.Version { get; set; }

        public bool AutoFateSync { get; set; } = false;
        public bool AutoFateSyncAutoStance { get; set; } = false;

        #region Test configuration values

        public string TestString { get; set; }
        public string TestStringWithDefaultValue { get; set; } = "Default value";
        public int TestInt { get; set; }
        public bool TestBool { get; set; }
        public List<string> TestListString { get; set; } = new List<string>() { "aaa", "bbb", "ccc" };
        
        #endregion

        private readonly DalamudPluginInterface pluginInterface;

        public Configuration(DalamudPluginInterface pi)
        {
            this.pluginInterface = pi;
        }

        public void Save()
        {
            this.pluginInterface.SavePluginConfig(this);
        }
    }
}
