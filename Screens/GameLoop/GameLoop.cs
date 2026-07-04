using Godot;
using System;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
public partial class GameLoop : Node2D
{
	PackedScene Bottom_Doors_Scene;
	Node2D bottom_doors;

	PackedScene Editable_Player_Scene;
	Node2D editable_player;

	PackedScene Enemy_Select_dropdown_Scene;
	Node2D enemy_select_dropdown;

	PackedScene Loot_Dropdown_Scene;
	Node2D loot_dropdown;

	PackedScene Battle_Scene;
	Node2D battle;

	PackedScene Hardpoint_Bar_Scene;
	Node2D hardpoint_bar;
	public override void _Ready()
	{
		Bottom_Doors_Scene = ResourceLoader.Load<PackedScene>("uid://cc8rvmj88yvh");
		Editable_Player_Scene = ResourceLoader.Load<PackedScene>("uid://y56svr5tngf1");
		Enemy_Select_dropdown_Scene = ResourceLoader.Load<PackedScene>("uid://irm0pguxeib6");
		Loot_Dropdown_Scene = ResourceLoader.Load<PackedScene>("uid://bcacvxnn2iq3a");
		Battle_Scene = ResourceLoader.Load<PackedScene>("uid://bnv1o5a1nwd3a");
		Hardpoint_Bar_Scene = ResourceLoader.Load<PackedScene>("uid://bpibjr0x7urnd");

		SignalConnect.Instance.Connect(SignalConnect.SignalName.StartNewLoop, new Callable(this, "_StartNewLoop"));
		StartNewLoop();
	}

	public void StartNewLoop()
	{
		if(IsInstanceValid(bottom_doors))
		{
			bottom_doors.CallDeferred("free");
		}
		if(IsInstanceValid(editable_player))
		{
			editable_player.CallDeferred("free");
		}
		if(IsInstanceValid(enemy_select_dropdown))
		{
			enemy_select_dropdown.CallDeferred("free");
		}
		if(IsInstanceValid(loot_dropdown))
		{
			loot_dropdown.CallDeferred("free");
		}
		if(IsInstanceValid(battle))
		{
			battle.CallDeferred("free");
		}
		if(IsInstanceValid(hardpoint_bar))
		{
			hardpoint_bar.CallDeferred("free");
			
		}
		enemy_select_dropdown = Enemy_Select_dropdown_Scene.Instantiate<Node2D>();
		loot_dropdown = Loot_Dropdown_Scene.Instantiate<Node2D>();
		bottom_doors = Bottom_Doors_Scene.Instantiate<Node2D>();
		editable_player = Editable_Player_Scene.Instantiate<Node2D>();
		battle = Battle_Scene.Instantiate<Node2D>();
		hardpoint_bar = Hardpoint_Bar_Scene.Instantiate<Node2D>();

		this.AddChild(hardpoint_bar);
		/*
		hardpointbar must be loaded before editable player so that the signals there can be connected before
		editable player sends them
		*/
		this.AddChild(editable_player);
		this.AddChild(enemy_select_dropdown);
		this.AddChild(loot_dropdown);
		this.AddChild(bottom_doors);
		this.AddChild(battle);
		
		
		
	}

	private void _StartNewLoop()
	{
		Dictionary run_data = new Dictionary();
		run_data.Add("player", new Array
		{
			RunData.p_ship_template_id,
			RunData.p_health_m_count,
			RunData.p_armor_m_count,
			RunData.p_crit_chance_m_count,
			RunData.p_level,
			RunData.p_active_inv,
			RunData.p_storage_inv,
			RunData.level_id,
			RunData.p_active_hardpoints,
		});

		
		RunData.Instance.SaveToUserData(Json.Stringify(run_data));
		RunData.Instance.InitializeDataVariables();
		StartNewLoop();
	}
	
}
