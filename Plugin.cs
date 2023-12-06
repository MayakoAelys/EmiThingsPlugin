using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Fates;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using EmiThingsPlugin.Windows;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Runtime.Versioning;
using XivCommon;
using XivCommon.Functions;
using Conditions = Dalamud.Game.ClientState.Conditions;

namespace EmiThingsPlugin
{
    [SupportedOSPlatform("windows")]
    public partial class Plugin : IDalamudPlugin
    {
        private readonly DalamudPluginInterface PluginInterface;
        private readonly IChatGui Chat;
        private readonly IClientState ClientState;

        private readonly PluginCommandManager<Plugin> CommandManager;
        private readonly Configuration Config;
        private readonly WindowSystem WindowSystem;
        private readonly IFateTable FateTable;
        private readonly IFramework Framework;
        private readonly ISigScanner SigScanner;
        private readonly IPartyList PartyList;
        private readonly ICondition Condition;

        protected XivCommonBase XIVCommonBase;
        protected Chat XIVChat;

        public string Name => "Emi things and tests";

        public Plugin(
            DalamudPluginInterface pi,
            ICommandManager commands,
            IChatGui chat,
            IClientState clientState,
            IFateTable fateTable,
            IFramework framework,
            ISigScanner sigScanner,
            IPartyList partyList,
            ICondition condition)
        {
            this.PluginInterface = pi;
            this.Chat            = chat;
            this.ClientState     = clientState;
            this.FateTable       = fateTable;
            this.Framework       = framework;
            this.SigScanner      = sigScanner;
            this.PartyList       = partyList;
            this.Condition       = condition;

            this.XIVCommonBase = new XivCommonBase(pi);
            this.XIVChat = this.XIVCommonBase.Functions.Chat;

            //this.ClientState.CfPop += ClientState_CfPop;

            this.Config = (Configuration) PluginInterface.GetPluginConfig() ?? this.PluginInterface.Create<Configuration>();
            this.WindowSystem = new WindowSystem(typeof(Plugin).AssemblyQualifiedName);
            this.CommandManager = new PluginCommandManager<Plugin>(this, commands);

            InitUI();
            InitEvents();

            InitAutoFateSync();
        }

        private void Framework_Update(IFramework framework)
        {
            AutoFateSyncUpdate();
        }

        //private void ClientState_CfPop(object sender, ContentFinderCondition e)
        //{
        //    if (e.AllianceRoulette)
        //    {

        //    }
        //}

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
            var window = this.PluginInterface.Create<PluginConfigWindow>(this.Config);

            if (window is not null)
            {
                this.WindowSystem.AddWindow(window);
            }

            this.PluginInterface.UiBuilder.Draw += this.WindowSystem.Draw;
        }

        private void InitEvents()
        {
            Framework.Update += Framework_Update;
        }

        #endregion

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            this.CommandManager.Dispose();

            this.PluginInterface.SavePluginConfig(this.Config);

            this.PluginInterface.UiBuilder.Draw -= this.WindowSystem.Draw;
            this.Framework.Update -= Framework_Update;

            this.WindowSystem.RemoveAllWindows();
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
