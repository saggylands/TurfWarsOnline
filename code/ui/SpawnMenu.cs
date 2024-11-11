﻿using Sandbox;
using Sandbox.Tools;

using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Linq;

[Library]
public partial class SpawnMenu : Panel
{
	public static SpawnMenu Instance;
	readonly Panel toollist;
	public Panel ToolPanel { get; private set; }
	public ButtonGroup SpawnMenuLeftTabs;
	public Panel SpawnMenuLeftBody;

	public bool IgnoreMenuButton = false;
	private bool IsOpen = false;

	private static ModelList modelList;
	private bool isSearching;

	public SpawnMenu()
	{
		Instance = this;

		var left = Add.Panel( "left" );
		{
			var tabs = left.AddChild<ButtonGroup>();
			tabs.AddClass( "tabs" );
			SpawnMenuLeftTabs = tabs;

			var body = left.Add.Panel( "body" );
			SpawnMenuLeftBody = body;
			{
				var props = body.AddChild<SpawnList>();
				tabs.SelectedButton = tabs.AddButtonActive( "#spawnmenu.props", ( b ) => props.SetClass( "active", b ) );

				modelList = body.AddChild<ModelList>();
				tabs.AddButtonActive( "#spawnmenu.modellist", ( b ) => modelList.SetClass( "active", b ) );

				var ents = body.AddChild<EntityList>();
				tabs.AddButtonActive( "#spawnmenu.entities", ( b ) => ents.SetClass( "active", b ) );

				var npclist = body.AddChild<NpcList>();
				tabs.AddButtonActive( "#spawnmenu.npclist", ( b ) => npclist.SetClass( "active", b ) );
				var joblist = body.AddChild<JobList>();
				tabs.AddButtonActive( "Jobs", ( b ) => joblist.SetClass( "active", b ) );
				var shipmentlist = body.AddChild<ShipmentsList>();
				tabs.AddButtonActive( "Shipments", ( b ) => shipmentlist.SetClass( "active", b ) );
				var drpentslist = body.AddChild<DRPEntsList>();
				tabs.AddButtonActive( "RP Entities", ( b ) => drpentslist.SetClass( "active", b ) );
				var customcrosshairslist = body.AddChild<CrosshairCustom>();
				tabs.AddButtonActive( "Crosshair", ( b ) => customcrosshairslist.SetClass( "active", b ) );
				var debuglist = body.AddChild<DebugList>();
				tabs.AddButtonActive( "Debug", ( b ) => debuglist.SetClass( "active", b ) );
			}
		}

		var right = Add.Panel( "right" );
		{
			var tabs = right.Add.Panel( "tabs" );
			{
				tabs.Add.Button( "#spawnmenu.tools" ).AddClass( "active" );
				tabs.Add.Button( "#spawnmenu.utility" );
			}
			var body = right.Add.Panel( "body" );
			{
				toollist = body.Add.Panel( "toollist" );
				{
					RebuildToolList();
				}
				ToolPanel = body.Add.Panel( "inspector" );
			}
		}

	}

	void RebuildToolList()
	{
		toollist.DeleteChildren( true );

		foreach ( var entry in TypeLibrary.GetTypes<BaseTool>().OrderBy( ( x ) => x.Title ) )
		{
			if ( entry.Name.StartsWith( "Base" ) )
				continue;

			var button = toollist.Add.Button( entry.Title );
			button.SetClass( "active", entry.ClassName == ConsoleSystem.GetValue( "tool_current" ) );

			button.AddEventListener( "onclick", () =>
			{
				SetActiveTool( entry.ClassName );

				foreach ( var child in toollist.Children )
					child.SetClass( "active", child == button );
				ToolPanel.DeleteChildren( true );
			} );
		}
	}
	void SetActiveTool( string className )
	{
		// setting a cvar
		ConsoleSystem.Run( "tool_current", className );

		// set the active weapon to the toolgun
		if ( Game.LocalPawn is not Player player ) return;
		if ( player.Inventory is null ) return;

		// why isn't inventory just an ienumurable wtf
		for ( int i = 0; i < player.Inventory.Count(); i++ )
		{
			var entity = player.Inventory.GetSlot( i );
			if ( !entity.IsValid() ) continue;
			if ( entity.ClassName != "weapon_tool" ) continue;

			player.ActiveChildInput = entity;
		}
	}

	private bool menuWasPressed = false;

	public override void Tick()
	{
		base.Tick();
		if ( !IgnoreMenuButton )
		{
			if ( Input.Pressed( "menu" ) )
			{
				IsOpen = true;
			}
			if ( menuWasPressed && !Input.Down( "menu" ) )
			{
				IsOpen = false;
			}
		}
		menuWasPressed = Input.Down( "menu" );

		if ( modelList.SearchInput.HasFocus )
		{
			isSearching = true;
		}
		else if ( isSearching && Input.Pressed( "menu" ) )
		{
			isSearching = false;
		}

		UpdateActiveTool();

		if ( isSearching )
			return;

		Parent.SetClass( "spawnmenuopen", IsOpen );

	}

	void UpdateActiveTool()
	{
		var toolCurrent = ConsoleSystem.GetValue( "tool_current" );
		var tool = string.IsNullOrWhiteSpace( toolCurrent ) ? null : TypeLibrary.GetType<BaseTool>( toolCurrent );

		foreach ( var child in toollist.Children )
		{
			if ( child is Button button )
			{
				child.SetClass( "active", tool != null && button.Text == tool.Title );
			}
		}
		Parent.SetClass( "spawnmenuopen", Input.Down( "menu" ) );
	}

	public override void OnHotloaded()
	{
		base.OnHotloaded();

		RebuildToolList();
	}
}
