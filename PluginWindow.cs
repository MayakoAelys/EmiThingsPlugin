using Dalamud.Interface.Windowing;
using ImGuiNET;
using System.Numerics;

namespace EmiThingsPlugin
{
    public class PluginWindow : Window
    {
        public const string WindowName = "WindowConfig";
        public const string MainTabBarName = "WindowConfigMainTabBar";

        private string ConfigTestString;
        private bool ConfigTestBool;

        private Configuration Config;

        public PluginWindow(Configuration config) : base(WindowName)
        {
            IsOpen = false;
            Size = new Vector2(810, 520);
            SizeCondition = ImGuiCond.FirstUseEver;
            
            // Init config
            Config = config;

            this.ConfigTestString = Config.TestString ?? string.Empty;
            this.ConfigTestBool = Config.TestBool;
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
            ImGui.Text("Hello, world!");
            ImGui.Text($"Test mouse pos: {ImGui.GetMousePos()}");
            ImGui.Text($"TestString: {Config.TestString}");
            ImGui.Text($"TestStringWithDefaultValue: {Config.TestStringWithDefaultValue}");
            ImGui.Text($"TestInt: {Config.TestInt}");
            ImGui.Text($"TestBool: {Config.TestBool}");
            ImGui.Text($"TestListString: {string.Join(',', Config.TestListString)}");
            ImGui.Text("");

            ImGui.Separator();

            ImGui.Text("");
            ImGui.Text("Update TestString:");
            ImGui.InputText(string.Empty, ref ConfigTestString, 25);
            ImGui.Checkbox("TestBool", ref ConfigTestBool);

            ImGui.Text("");

            ImGui.Separator();

            ImGui.Text("");

            if (ImGui.Button("Save config"))
            {
                Config.TestString = ConfigTestString;
                Config.TestBool = ConfigTestBool;
            }
        }

        private void DrawTest()
        {
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
            ImGui.Text("This is just a test plugin");
        }
    }
}
