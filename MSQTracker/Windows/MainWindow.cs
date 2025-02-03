using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace MSQTracker.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin plugin;

    public MainWindow(Plugin plugin)
        : base("MSQTracker##MSQTMain", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(135, 95),
            MaximumSize = new Vector2(float.MaxValue, 95)
        };

        this.plugin = plugin;
    }

    public void Dispose()
    {
    }

    public unsafe override void Draw()
    {
        if (plugin.progress.quest.currentQuestNum == -1)
        {
            ImGui.TextUnformatted("No current MSQ");
            ImGui.TextUnformatted("in progress.");
            if (plugin.Configuration.Tracking)
            {
                ImGui.TextUnformatted("Checking in 10 Sec...");
            } else
            {
                ImGui.TextUnformatted("Tracking Disabled.");
            }
        } else
        {
            ImGui.TextUnformatted(plugin.progress.quest.xpac);
            ImGui.TextUnformatted(plugin.progress.quest.name);
            ImGui.TextUnformatted($"{plugin.progress.quest.percentProgress} ({plugin.progress.quest.currentQuestNum}/{plugin.progress.quest.totalQuests})");
        }
    }
}
