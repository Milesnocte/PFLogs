﻿using Dalamud.IoC;
using Dalamud.Plugin;
using System.Linq;
using Dalamud.Game.Gui.ContextMenu;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Lumina.Excel.GeneratedSheets;
using NetStone;
using NetStone.Search.Character;
using PFLogs.Windows;

namespace PFLogs;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static IContextMenu ContextMenu { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IChatGui Chat { get; private set; } = null!;

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("PFLogs");
    private ConfigWindow ConfigWindow { get; init; }

    public Plugin()
    {
        ContextMenu.OnMenuOpened += ContextMenuOnOnMenuOpened;
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        ConfigWindow = new ConfigWindow(this);

        WindowSystem.AddWindow(ConfigWindow);

        PluginInterface.UiBuilder.Draw += DrawUI;

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;
    }

    private unsafe async void ContextMenuOnOnMenuOpened(IMenuOpenedArgs args)
    {
        //Chat.Print(args.AddonName!);
        if(!args.AddonName!.Equals("LookingForGroup") && !args.AddonName!.Equals("PartyMemberList")&& !args.AddonName!.Equals("_PartyList")) return;
        var ctx = AgentContext.Instance();
        
        var worldSheet = DataManager.GetExcelSheet<World>();
        
        var name = ctx->ContextMenuTarget.NameString;
        var world = worldSheet.GetRow(ctx->ContextMenuTarget.HomeWorld).Name;

        args.AddMenuItem(new MenuItem()
        {
            Name = Configuration.UseTomeStone ? "Open Tomestone" : "Open FFLogs",
            PrefixChar = 'P',
            PrefixColor = 707,
            OnClicked = clickedArgs => GoToSite(name, world)
        });
    }

    private async void GoToSite(string name, string world)
    {
        LodestoneClient lodestoneClient = await LodestoneClient.GetClientAsync();
        var characters = await lodestoneClient.SearchCharacter(new CharacterSearchQuery()
        {
            CharacterName = name,
            World = world
        });
        var lodeId = characters!.Results.First(x => x.Name == name).Id;
        if (Configuration.UseTomeStone)
        {
            Util.OpenLink($"https://tomestone.gg/character/{lodeId}/{name.Replace(" ", "-")}");
        }
        else
        {
            Util.OpenLink($"https://www.fflogs.com/character/lodestone-id/{lodeId}");
        }
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
}
