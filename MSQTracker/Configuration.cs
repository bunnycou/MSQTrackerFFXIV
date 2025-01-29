using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace MSQTracker;

// Nothing here is used, keeping it in case I need config

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool isDevDumb = true;
    public bool Tracking = true;

    // the below exist just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
