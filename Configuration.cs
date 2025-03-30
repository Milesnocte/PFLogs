using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace PFLogs;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 6;

    public bool AddTomeStone { get; set; } = false;
    public bool AddFFLogs { get; set; } = true;
    
    public bool AddToPartyList { get; set; } = true;
    public bool AddToPartyMembers { get; set; } = true;
    public bool AddToPartyFinder { get; set; } = true;
    public bool AddToChatLog { get; set; } = false;
    public bool AddToContactList { get; set; } = false;
    public bool AddToFriendsList { get; set; } = false;
    
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
