using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace MSQTracker.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration configuration;

    public ConfigWindow(Plugin plugin) : base("MSQTracker Config###MSQTConf")
    {
        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(180, 150);
        SizeCondition = ImGuiCond.Always;

        configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void PreDraw()
    {
    }

    public override void Draw()
    {
        ImGui.TextUnformatted("MSQTracker 1.0");
        ImGui.TextUnformatted("Pulls data from");
        ImGui.TextUnformatted("ffxiv-progress.com");

        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Spacing();

        if (configuration.Tracking)
        {
            ImGui.TextUnformatted("Tracking Enabled");
        } else
        {
            ImGui.TextUnformatted("Tracking Disabled");
        }

        if (ImGui.Button("Toggle Tracking"))
        {
            configuration.Tracking = !configuration.Tracking;
            configuration.Save();
        }
    }
}
