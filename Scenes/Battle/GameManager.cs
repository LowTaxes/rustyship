using Godot;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
using RunDataEnum = Constants.RunDataEnum;
using ShipDataEnum = Constants.ShipDataEnum;
public partial class GameManager : Node2D
{
	public Ship player;
	public Ship enemy;

	public Vector2 player_start;
	public Vector2 enemy_start;
	public override void _Ready()
	{
		Array p_run_data_arr =(Array)RunData.Instance.LoadUserData()["player"];
		Array e_run_data_arr =(Array)RunData.Instance.LoadUserData()["enemy"];

		PackedScene player_ship_scene = ResourceLoader.Load<PackedScene>(((Array)ConstantData.ShipData[p_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()])[(int)ShipDataEnum.SHIP_UID].ToString());
		PackedScene enemy_ship_scene = ResourceLoader.Load<PackedScene>(((Array)ConstantData.ShipData[e_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()])[(int)ShipDataEnum.SHIP_UID].ToString());
		
		Json json_loader = new Json();
		//ship TEMPLATE ID 
		
		
		
		//Spawn Player Features
		
		
		player = player_ship_scene.Instantiate<Ship>();
		player.is_player = true;
		player.max_health = (double)((Array)ConstantData.ShipData[p_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()])[(int)ShipDataEnum.MAX_HEALTH];
		player.health = player.max_health;
		player.max_armor = (double)((Array)ConstantData.ShipData[p_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()])[(int)ShipDataEnum.MAX_ARMOR];
		player.armor = player.max_armor;
		player.maneuverability = (double)((Array)ConstantData.ShipData[p_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()])[(int)ShipDataEnum.MANEUVERABILITY];

		player.available_hardpoints = new List<Hardpoint>
		{
			new Hardpoint(1, "light",  "lightmachinegun"),
			new Hardpoint(1, "medium",  "mediumcannon"),
			new Hardpoint(1, "light",  "lightmachinegun")
		};

		json_loader.Parse(Json.Stringify(ConstantData.ShipData[p_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()]));
		Array template_data_p = (Array)(json_loader.Data);
		json_loader.Parse(Json.Stringify(template_data_p[(int)ShipDataEnum.HARDPOINT_LOCATIONS]));
		Array hardpoint_locs_p = (Array)json_loader.Data;
		player.hardpoint_locations = RunData.UnpackListOfVector2(Json.Stringify(hardpoint_locs_p));

		player.hardpoint_weight_classes = (Array)((Array)ConstantData.ShipData[p_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()])[(int)ShipDataEnum.HARDPOINT_WEIGHT_CLASSES];
		player.other_ship_width = (int)((Array)ConstantData.ShipData[e_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()])[(int)ShipDataEnum.SHIP_WIDTH];
		AddChild(player);
		

		//Spawn Enemy Features
		enemy = enemy_ship_scene.Instantiate<Ship>();
		enemy.is_player = false;
		enemy.max_health = (double)((Array)ConstantData.ShipData[e_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()])[(int)ShipDataEnum.MAX_HEALTH];
		enemy.health = enemy.max_health;
		enemy.max_armor = (double)((Array)ConstantData.ShipData[e_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()])[(int)ShipDataEnum.MAX_ARMOR];
		enemy.armor = enemy.max_armor;
		enemy.maneuverability = (double)((Array)ConstantData.ShipData[e_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()])[(int)ShipDataEnum.MANEUVERABILITY];

		enemy.available_hardpoints = new List<Hardpoint>
		{
			new Hardpoint(1, "light",  "lightmachinegun"),
			new Hardpoint(1, "medium",  "mediumcannon"),
			new Hardpoint(1, "light",  "lightmachinegun")
		};

		json_loader.Parse(Json.Stringify(ConstantData.ShipData[e_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()]));
		Array template_data_e = (Array)(json_loader.Data);
		json_loader.Parse(Json.Stringify(template_data_e[(int)ShipDataEnum.HARDPOINT_LOCATIONS]));
		Array hardpoint_locs_e = (Array)json_loader.Data;
		enemy.hardpoint_locations = RunData.UnpackListOfVector2(Json.Stringify(hardpoint_locs_e));

		enemy.hardpoint_weight_classes = (Array)((Array)ConstantData.ShipData[e_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()])[(int)ShipDataEnum.HARDPOINT_WEIGHT_CLASSES];
		enemy.other_ship_width = (int)((Array)ConstantData.ShipData[p_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()])[(int)ShipDataEnum.SHIP_WIDTH];
		
		AddChild(enemy);
		
	
		player.Translate(Constants.PLAYER_START_LOCATION);
		enemy.Translate(Constants.ENEMY_START_LOCATION);
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
