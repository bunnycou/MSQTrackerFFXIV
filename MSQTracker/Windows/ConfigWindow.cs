using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using MSQTracker;

namespace MSQTracker.Windows;

// not using config window right now, keeping it in case it becomes needed

public class ConfigWindow : Window, IDisposable
{
    private Configuration configuration;

    // We give this window a constant ID using ###
    // This allows for labels being dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
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
        // Flags must be added or removed before Draw() is being called, or they won't apply
        if (configuration.isDevDumb)
        {
            Flags &= ~ImGuiWindowFlags.NoMove;
        }
        else
        {
            Flags |= ImGuiWindowFlags.NoMove;
        }
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

        // can't ref a property, so use a local copy
        //var configValue = configuration.SomePropertyToBeSavedAndWithADefault;
        //if (ImGui.Checkbox("Random Config Bool", ref configValue))
        //{
        //    configuration.SomePropertyToBeSavedAndWithADefault = configValue;
        //    // can save immediately on change, if you don't want to provide a "Save and Close" button
        //    configuration.Save();
        //}

        //var movable = configuration.IsConfigWindowMovable;
        //if (ImGui.Checkbox("Movable Config Window", ref movable))
        //{
        //    configuration.IsConfigWindowMovable = movable;
        //    configuration.Save();
        //}
    }
}
