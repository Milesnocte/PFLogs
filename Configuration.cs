using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace PFLogs;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool AddTomeStone { get; set; } = false;
    public bool AddFFLogs { get; set; } = true;
    
    public bool AddToPartyList { get; set; } = true;
    public bool AddToPartyMembers { get; set; } = true;
    public bool AddToPartyFinder { get; set; } = true;
    
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
