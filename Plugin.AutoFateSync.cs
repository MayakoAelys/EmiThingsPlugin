using Dalamud.Game;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.UI;
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

        private void AutoFateSync()
        {
            if (!this.config.AutoFateSync)
                return;

            bool wasInFate = InFate;

            InFate = Marshal.ReadByte(InFatePointer) == 1;

            if (wasInFate != InFate)
            {
                ExecuteCommand("/levelsync on");
            }
        }

        private void InitAutoFateSync()
        {
            // FATE pointer (thanks to Pohky#8008)
            try
            {
                var sig = SigScanner.ScanText("80 3D ?? ?? ?? ?? ?? 0F 84 ?? ?? ?? ?? 48 8B 42 20");

                InFatePointer = sig + Marshal.ReadInt32(sig, 2) + 7;

                chat.Print("Retrieved 'InFatePointer' successfully");
                chat.Print(InFatePointer.ToString("X8"));
            }
            catch
            {
                PluginLog.Error("Failed loading 'InFatePointer'");
            }
        }
    }
}
