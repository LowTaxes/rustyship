using Godot;
using System;

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
	public override void _Ready()
	{
		Bottom_Doors_Scene = ResourceLoader.Load<PackedScene>("uid://cc8rvmj88yvh");
		Editable_Player_Scene = ResourceLoader.Load<PackedScene>("uid://y56svr5tngf1");
		Enemy_Select_dropdown_Scene = ResourceLoader.Load<PackedScene>("uid://irm0pguxeib6");
		Loot_Dropdown_Scene = ResourceLoader.Load<PackedScene>("uid://bcacvxnn2iq3a");
	 
		StartNewLoop();
	}

	public void StartNewLoop()
	{
		enemy_select_dropdown = Enemy_Select_dropdown_Scene.Instantiate<Node2D>();
		loot_dropdown = Loot_Dropdown_Scene.Instantiate<Node2D>();
		bottom_doors = Bottom_Doors_Scene.Instantiate<Node2D>();
		editable_player = Editable_Player_Scene.Instantiate<Node2D>();
		this.AddChild(editable_player);
		this.AddChild(enemy_select_dropdown);
		this.AddChild(loot_dropdown);
		this.AddChild(bottom_doors);
		
		
	}

	
}
