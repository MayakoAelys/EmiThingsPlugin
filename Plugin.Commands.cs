using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using EmiThingsPlugin.Attributes;
using EmiThingsPlugin.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XivCommon;

namespace EmiThingsPlugin
{
    public partial class Plugin : IDalamudPlugin
    {
        //[Aliases("/emiConfig")]
        [Command("/emiconfig")]
        [HelpMessage("Open the configuration window")]
        public void TestConfig(string commands, string args)
        {
            Window pluginWindow = 
                windowSystem.Windows.First(w => w.WindowName.Equals(PluginConfigWindow.WindowName));

            if (!pluginWindow.IsOpen)
                pluginWindow.Toggle();
        }

        //[Command("/example1")]
        //[HelpMessage("Example help message.")]
        //public void ExampleCommand1(string command, string args)
        //{
        //    // You may want to assign these references to private variables for convenience.
        //    // Keep in mind that the local player does not exist until after logging in.
        //    var world = this.clientState.LocalPlayer?.CurrentWorld.GameData;

        //    //this.chat.Print($"Hello, {world?.Name}!");
        //    //PluginLog.Log("Message sent successfully.");

        //    Window pluginWindow = this.windowSystem.Windows.First(w => w.WindowName.Equals(PluginWindow.WindowName));

        //    if (!pluginWindow.IsOpen)
        //        pluginWindow.Toggle();
        //}

        //[Command("/example2")]
        //[HelpMessage("Example 2 help message")]
        //public void Example2Command(string commands, string args)
        //{
        //    this.chat.Print($"Hello {this.clientState.LocalPlayer.Name} du serveur {this.clientState.LocalPlayer.HomeWorld.GameData.Name}");
        //}

        //[Command("/testChat")]
        //[HelpMessage("test")]
        //public void TestChat(string commands, string args)
        //{
        //    this.chat.Print($"commands: {commands}");
        //    this.chat.Print($"args: {args}");

        //    using (var xivcommonbase = new XIVCommonBase())
        //    {
        //        xivcommonbase.Functions.Chat.SendMessage("/afk");

        //        string sanitizedText = xivcommonbase.Functions.Chat.SanitiseText(args);
        //        xivcommonbase.Functions.Chat.SendMessage($"/p {sanitizedText}");
        //    }
        //}

        //[Command("/testConfig")]
        //public void TestConfig(string commands, string args)
        //{
        //    this.chat.Print($"TestString: {this.config.TestString}");
        //    // this.config.CoolText = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); // rewrite the config without needing to save
        //}
    }
}
