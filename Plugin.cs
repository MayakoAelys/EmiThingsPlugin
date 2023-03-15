using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Fates;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using EmiThingsPlugin.Windows;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Runtime.Versioning;
using XivCommon;
using XivCommon.Functions;

namespace EmiThingsPlugin
{
    [SupportedOSPlatform("windows")]
    public partial class Plugin : IDalamudPlugin
    {
        private readonly DalamudPluginInterface pluginInterface;
        private readonly ChatGui chat;
        private readonly ClientState clientState;

        private readonly PluginCommandManager<Plugin> commandManager;
        private readonly Configuration config;
        private readonly WindowSystem windowSystem;
        private readonly FateTable fateTable;
        private readonly Framework framework;
        private readonly SigScanner SigScanner;


        protected XivCommonBase XIVCommonBase;
        protected Chat XIVChat;

        public string Name => "Emi things and tests";

        public Plugin(
            DalamudPluginInterface pi,
            CommandManager commands,
            ChatGui chat,
            ClientState clientState,
            FateTable fateTable,
            Framework framework,
            SigScanner sigScanner)
        {
            this.pluginInterface = pi;
            this.chat            = chat;
            this.clientState     = clientState;
            this.fateTable       = fateTable;
            this.framework       = framework;
            this.SigScanner      = sigScanner;

            this.XIVCommonBase = new XivCommonBase();
            this.XIVChat = this.XIVCommonBase.Functions.Chat;

            this.clientState.CfPop += ClientState_CfPop;

            this.config = (Configuration) pluginInterface.GetPluginConfig() ?? this.pluginInterface.Create<Configuration>();
            this.windowSystem = new WindowSystem(typeof(Plugin).AssemblyQualifiedName);
            this.commandManager = new PluginCommandManager<Plugin>(this, commands);

            InitUI();
            InitEvents();

            InitAutoFateSync();
        }

        private void Framework_Update(Framework framework)
        {
            AutoFateSync();
        }

        private void ClientState_CfPop(object sender, ContentFinderCondition e)
        {
            if (e.AllianceRoulette)
            {

            }
        }

        #region Utils

        private void ExecuteCommand(string command)
        {
            string sanitizedCommand = XIVChat.SanitiseText(command);

            XIVChat.SendMessage(sanitizedCommand);
        }

        #endregion

        #region Private functions

        private void InitUI()
        {
            var window = this.pluginInterface.Create<PluginConfigWindow>(this.config);

            if (window is not null)
            {
                this.windowSystem.AddWindow(window);
            }

            this.pluginInterface.UiBuilder.Draw += this.windowSystem.Draw;
        }

        private void InitEvents()
        {
            framework.Update += Framework_Update;
        }

        #endregion

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
            this.XIVCommonBase.Dispose();

            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
