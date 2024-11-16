using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace PFLogs.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;

    // We give this window a constant ID using ###
    // This allows for labels being dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin) : base("PFLogs Config")
    {
        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(350, 200);
        SizeCondition = ImGuiCond.Always;

        Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void PreDraw()
    {
        // Flags must be added or removed before Draw() is being called, or they won't apply
        if (Configuration.AddTomeStone)
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
        // can't ref a property, so use a local copy
        var tomestone = Configuration.AddTomeStone;
        var fflogs = Configuration.AddFFLogs;
        
        var partyList = Configuration.AddToPartyList;
        var partyFinder = Configuration.AddToPartyFinder;
        var partyMembers = Configuration.AddToPartyMembers;
        var chatLog = Configuration.AddToChatLog;
        
        if (ImGui.Checkbox("Add FFLogs Option", ref fflogs))
        {
            Configuration.AddFFLogs = fflogs;
            Configuration.Save();
        }
        
        if (ImGui.Checkbox("Add Tomestone Option", ref tomestone))
        {
            Configuration.AddTomeStone = tomestone;
            Configuration.Save();
        }
        
        ImGui.Separator();
        
        if (ImGui.Checkbox("Add to Party Finder", ref partyFinder))
        {
            Configuration.AddToPartyFinder = partyFinder;
            Configuration.Save();
        }
        
        if (ImGui.Checkbox("Add to Party List (HUD)", ref partyList))
        {
            Configuration.AddToPartyList = partyList;
            Configuration.Save();
        }
        
        if (ImGui.Checkbox("Add to Party Member List (Social Window)", ref partyMembers))
        {
            Configuration.AddToPartyMembers = partyMembers;
            Configuration.Save();
        }
        
        if (ImGui.Checkbox("Add to Chat Window", ref chatLog))
        {
            Configuration.AddToChatLog = chatLog;
            Configuration.Save();
        }
    }
}
