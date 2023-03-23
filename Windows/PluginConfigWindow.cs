using Dalamud.Interface.Style;
using Dalamud.Interface.Windowing;
using EmiThingsPlugin.Constants;
using ImGuiNET;
using System.Numerics;

namespace EmiThingsPlugin.Windows
{
    public class PluginConfigWindow : Window
    {
        public const string WindowName = "EmiThings Configuration";
        public const string MainTabBarName = "PluginConfigWindowMainTabBar";

        private string ConfigTestString;
        private bool ConfigTestBool;

        private bool ConfigAutoFateSync;
        private bool ConfigAutoFateSyncAutoStance;

        private Configuration Config;

        public PluginConfigWindow(Configuration config) : base(WindowName)
        {
            IsOpen = false;
            Size = new Vector2(810, 520);
            SizeCondition = ImGuiCond.FirstUseEver;

            // Init Config
            Config = config;

            ConfigTestString = Config.TestString ?? string.Empty;
            ConfigTestBool = Config.TestBool;

            ConfigAutoFateSync = Config.AutoFateSync;
            ConfigAutoFateSyncAutoStance = Config.AutoFateSyncAutoStance;
        }

        public override void Draw()
        {
            ImGuiTabBarFlags tab_bar_flags = ImGuiTabBarFlags.None;

            if (ImGui.BeginTabBar(MainTabBarName, tab_bar_flags))
            {
                if (ImGui.BeginTabItem("Config"))
                {
                    DrawConfig();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Test"))
                {
                    DrawTest();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("About"))
                {
                    DrawAbout();
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
        }

        private void DrawConfig()
        {
            ImGui.Text("");
            //ImGui.Text("Don't forget to click on the SAVE button to apply the changes ;3");
            ImGui.TextColored(EmiColors.Yellow, "Don't forget to click on the SAVE button to apply the changes ;3");
            ImGui.Text("");

            ImGui.Separator();

            ImGui.Text("");
            ImGui.Checkbox("Auto FATE sync?", ref ConfigAutoFateSync);
            ImGui.Checkbox("Auto FATE - Automatic tank stance?", ref ConfigAutoFateSyncAutoStance);
            ImGui.Text($"Auto FATE sync is {(Config.AutoFateSync ? "enabled" : "disabled")}");
            ImGui.Text($"Automatic tank stance is {(Config.AutoFateSyncAutoStance ? "enabled" : "disabled")}");
            ImGui.Text("");

            ImGui.Separator();

            ImGui.Text("");

            if (ImGui.Button("Save Config"))
            {
                Config.AutoFateSync = ConfigAutoFateSync;
                Config.AutoFateSyncAutoStance = ConfigAutoFateSyncAutoStance;
            }
        }

        private void DrawTest()
        {
            ImGui.Text($"Test mouse pos: {ImGui.GetMousePos()}");
            ImGui.Text($"TestString: {Config.TestString}");
            ImGui.Text($"TestStringWithDefaultValue: {Config.TestStringWithDefaultValue}");
            ImGui.Text($"TestInt: {Config.TestInt}");
            ImGui.Text($"TestBool: {Config.TestBool}");
            ImGui.Text($"TestListString: {string.Join(',', Config.TestListString)}");

            if (ImGui.TreeNode("Tabs"))
            {
                if (ImGui.TreeNode("Test 1"))
                {
                    ImGui.Text("Test test test");
                    ImGui.Text("");
                    ImGui.Text("Test test test");
                    ImGui.Text("Test test test");
                    ImGui.TreePop();
                }

                if (ImGui.TreeNode("Test 2"))
                {
                    ImGui.Text("Test test test");
                    ImGui.Separator();
                    ImGui.Text("Test test test");
                    ImGui.Text("Test test test");
                    ImGui.TreePop();
                }

                if (ImGui.TreeNode("Test 3"))
                {
                    ImGui.Text("Test test test");
                    ImGui.Text("");
                    ImGui.Text("Test test test");
                    ImGui.Text("Test test test");
                    ImGui.TreePop();
                }

                ImGui.TreePop();
            }
        }

        private void DrawAbout()
        {
            ImGui.Text("Hello u cutie uwu");
        }
    }
}
