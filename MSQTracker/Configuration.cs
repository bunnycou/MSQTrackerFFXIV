using Dalamud.Configuration;
using System;

namespace MSQTracker;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool isDevDumb = true; // :3
    public bool Tracking = true;

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
