using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Fates;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Plugin;
using DalamudPluginProjectTemplate.Attributes;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Linq;
using XivCommon;

namespace DalamudPluginProjectTemplate
{
    public class Plugin : IDalamudPlugin
    {
        private readonly DalamudPluginInterface pluginInterface;
        private readonly ChatGui chat;
        private readonly ClientState clientState;

        private readonly PluginCommandManager<Plugin> commandManager;
        private readonly Configuration config;
        private readonly WindowSystem windowSystem;
        private readonly FateTable fateTable;
        private readonly Framework framework;

        public string Name => "Your Plugin's Display Name";

        public Plugin(
            DalamudPluginInterface pi,
            CommandManager commands,
            ChatGui chat,
            ClientState clientState,
            FateTable fateTable,
            Framework framework)
        {
            this.pluginInterface = pi;
            this.chat = chat;
            this.clientState = clientState;
            this.fateTable = fateTable;
            this.framework = framework;

            clientState.CfPop += ClientState_CfPop;

            // Get or create a configuration object
            this.config = (Configuration)this.pluginInterface.GetPluginConfig()
                          ?? this.pluginInterface.Create<Configuration>();

            // Initialize the UI
            this.windowSystem = new WindowSystem(typeof(Plugin).AssemblyQualifiedName);

            var window = this.pluginInterface.Create<PluginWindow>(this.config);


            if (window is not null)
            {
                this.windowSystem.AddWindow(window);
            }

            this.pluginInterface.UiBuilder.Draw += this.windowSystem.Draw;

            // Load all of our commands
            this.commandManager = new PluginCommandManager<Plugin>(this, commands);

            framework.Update += Framework_Update;
        }

        private void Framework_Update(Framework framework)
        {

        }

        private void ClientState_CfPop(object sender, ContentFinderCondition e)
        {
            if (e.AllianceRoulette)
            {

            }
        }

        [Command("/example1")]
        [HelpMessage("Example help message.")]
        public void ExampleCommand1(string command, string args)
        {
            // You may want to assign these references to private variables for convenience.
            // Keep in mind that the local player does not exist until after logging in.
            var world = this.clientState.LocalPlayer?.CurrentWorld.GameData;

            //this.chat.Print($"Hello, {world?.Name}!");
            //PluginLog.Log("Message sent successfully.");

            Window pluginWindow = this.windowSystem.Windows.First(w => w.WindowName.Equals(PluginWindow.WindowName));

            if (!pluginWindow.IsOpen)
                pluginWindow.Toggle();
        }

        [Command("/example2")]
        [HelpMessage("Example 2 help message")]
        public void Example2Command(string commands, string args)
        {
            this.chat.Print($"Hello {this.clientState.LocalPlayer.Name} du serveur {this.clientState.LocalPlayer.HomeWorld.GameData.Name}");
        }

        [Command("/testChat")]
        [HelpMessage("test")]
        public void TestChat(string commands, string args)
        {
            this.chat.Print($"commands: {commands}");
            this.chat.Print($"args: {args}");

            using (var xivcommonbase = new XivCommonBase())
            {
                xivcommonbase.Functions.Chat.SendMessage("/afk");

                string sanitizedText = xivcommonbase.Functions.Chat.SanitiseText(args);
                xivcommonbase.Functions.Chat.SendMessage($"/p {sanitizedText}");
            }
        }

        [Command("/testConfig")]
        public void TestConfig(string commands, string args)
        {
            this.chat.Print($"TestString: {this.config.TestString}");
            // this.config.CoolText = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); // rewrite the config without needing to save
        }


        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            this.commandManager.Dispose();

            this.pluginInterface.SavePluginConfig(this.config);

            this.pluginInterface.UiBuilder.Draw -= this.windowSystem.Draw;
            this.windowSystem.RemoveAllWindows();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
