using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace MSQTracker.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin plugin;
    private MSQProgress progress;

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin, MSQProgress progress)
        : base("MSQTracker##MSQTMain", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(135, 95),
            MaximumSize = new Vector2(float.MaxValue, 95)
        };

        this.plugin = plugin;
        this.progress = progress;
    }

    public void Dispose()
    {
    }

    public unsafe override void Draw()
    {
        ImGui.TextUnformatted(progress.xpac);
        ImGui.TextUnformatted(progress.questName);
        ImGui.TextUnformatted($"{progress.percentProgress} ({progress.currentQuestNum}/{progress.totalQuests})");
    }
}
