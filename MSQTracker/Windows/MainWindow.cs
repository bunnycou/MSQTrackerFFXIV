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
        ImGui.TextUnformatted(plugin.progress.xpac);
        ImGui.TextUnformatted(plugin.progress.questName);
        ImGui.TextUnformatted($"{plugin.progress.percentProgress} ({plugin.progress.currentQuestNum}/{plugin.progress.totalQuests})");
    }
}
