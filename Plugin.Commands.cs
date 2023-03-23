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
                WindowSystem.Windows.First(w => w.WindowName.Equals(PluginConfigWindow.WindowName));

            if (!pluginWindow.IsOpen)
                pluginWindow.Toggle();
        }

        //[Command("/example1")]
        //[HelpMessage("Example help message.")]
        //public void ExampleCommand1(string command, string args)
        //{
        //    // You may want to assign these references to private variables for convenience.
        //    // Keep in mind that the local player does not exist until after logging in.
        //    var world = this.ClientState.LocalPlayer?.CurrentWorld.GameData;

        //    //this.Chat.Print($"Hello, {world?.Name}!");
        //    //PluginLog.Log("Message sent successfully.");

        //    Window pluginWindow = this.WindowSystem.Windows.First(w => w.WindowName.Equals(PluginWindow.WindowName));

        //    if (!pluginWindow.IsOpen)
        //        pluginWindow.Toggle();
        //}

        //[Command("/example2")]
        //[HelpMessage("Example 2 help message")]
        //public void Example2Command(string commands, string args)
        //{
        //    this.Chat.Print($"Hello {this.ClientState.LocalPlayer.Name} du serveur {this.ClientState.LocalPlayer.HomeWorld.GameData.Name}");
        //}

        //[Command("/testChat")]
        //[HelpMessage("test")]
        //public void TestChat(string commands, string args)
        //{
        //    this.Chat.Print($"commands: {commands}");
        //    this.Chat.Print($"args: {args}");

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
        //    this.Chat.Print($"TestString: {this.Config.TestString}");
        //    // this.Config.CoolText = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); // rewrite the Config without needing to save
        //}
    }
}
