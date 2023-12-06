using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Resolvers;
using Dalamud.Game.Gui;
using Dalamud.Logging;
using Dalamud.Plugin.Services;
using EmiThingsPlugin.Models;
using FFXIVClientStructs.FFXIV.Client.UI;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Runtime.InteropServices;

namespace EmiThingsPlugin
{
    public partial class Plugin
    {
        // Based on FATEAutoSync plugin
        // https://github.com/Tenrys/FATEAutoSync
        private IntPtr InFatePointer = IntPtr.Zero;
        private bool InFate = false;
        private bool InFateOnAMount = false;
        private bool InFateStanceLaunched = false;
        private DateTime DateTimeEnteredInFate = DateTime.MaxValue;

        private void AutoFateSyncUpdate()
        {
            if (!this.Config.AutoFateSync)
                return;

            bool wasInFate = InFate;

            InFate = Marshal.ReadByte(InFatePointer) == 1;

            if (wasInFate != InFate)
            {
                if (!wasInFate)
                {
                    PluginLog.Verbose("wasInFate == false");

                    ExecuteCommand("/levelsync on");

                    DateTimeEnteredInFate = DateTime.Now;
                }

                // We were in a FATE
                if (wasInFate && InFateStanceLaunched)
                {
                    PluginLog.Verbose("wasInFate && InFateStanceLaunched");

                    InFateOnAMount       = false;
                    InFateStanceLaunched = false;
                }
            }

            if (InFate && this.Config.AutoFateSyncAutoStance)
            {
                AutoStance();
            }
        }

        private void InitAutoFateSync()
        {
            // FATE pointer (thanks to Pohky#8008)
            try
            {
                var sig = SigScanner.ScanText("80 3D ?? ?? ?? ?? ?? 0F 84 ?? ?? ?? ?? 48 8B 42 20");

                InFatePointer = sig + Marshal.ReadInt32(sig, 2) + 7;

                //Chat.Print("Retrieved 'InFatePointer' successfully");
                //Chat.Print(InFatePointer.ToString("X8"));
            }
            catch
            {
                PluginLog.Error("Failed loading 'InFatePointer'");
            }
        }

        private void AutoStance()
        {
            // Tank stance
            var classJob = ClientState.LocalPlayer.ClassJob;

            bool isTank =
                classJob.GameData.ClassJobCategory.Value.GLA ||
                classJob.GameData.ClassJobCategory.Value.MRD ||
                classJob.GameData.ClassJobCategory.Value.PLD ||
                classJob.GameData.ClassJobCategory.Value.WAR ||
                classJob.GameData.ClassJobCategory.Value.DRK ||
                classJob.GameData.ClassJobCategory.Value.GNB;

            if (!isTank || InFateStanceLaunched)
                return;

            // Wait for at least some seconds before casting the stance
            if (DateTime.Now < DateTimeEnteredInFate.AddSeconds(2))
                return;

            // Can't cast skill on a mount
            InFateOnAMount = Condition[ConditionFlag.Mounted];

            if (InFateOnAMount)
                return;

            // Execute stance
            string stanceName = GetStanceName(ClientState, classJob);

            PluginLog.Verbose($"StanceName: \"{stanceName}\"");

            ExecuteCommand($"/ac \"{stanceName}\"");

            InFateStanceLaunched = true;
        }

        private string GetStanceName(IClientState clientState, ExcelResolver<ClassJob> classJob)
        {
            string stanceName = null;

            // GLA + PLD
            if (classJob.GameData.NameEnglish.RawString.Equals("Gladiator", StringComparison.InvariantCultureIgnoreCase) ||
                classJob.GameData.NameEnglish.RawString.Equals("Paladin", StringComparison.InvariantCultureIgnoreCase))
                stanceName = XIVSkills.PLDStance.GetSkillName(clientState);

            // MRD + WAR
            if (classJob.GameData.NameEnglish.RawString.Equals("Marauder", StringComparison.InvariantCultureIgnoreCase) ||
                classJob.GameData.NameEnglish.RawString.Equals("Warrior", StringComparison.InvariantCultureIgnoreCase))
                stanceName = XIVSkills.WARStance.GetSkillName(clientState);

            // DRK
            if (classJob.GameData.NameEnglish.RawString.Equals("Dark knight", StringComparison.InvariantCultureIgnoreCase))
                stanceName = XIVSkills.DRKStance.GetSkillName(clientState);

            // GNB
            if (classJob.GameData.NameEnglish.RawString.Equals("Gunbreaker", StringComparison.InvariantCultureIgnoreCase))
                stanceName = XIVSkills.GNBStance.GetSkillName(clientState);

            return stanceName;
        }
    }
}

