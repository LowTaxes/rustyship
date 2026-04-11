using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
using RunDataEnum = Constants.RunDataEnum;
using ShipDataEnum = Constants.ShipDataEnum;
public partial class MainMenu : Node2D
{
	
	public PackedScene main_scene;
	
	public PackedScene test_ship;

    public override void _Ready()
    {
        main_scene = ResourceLoader.Load<PackedScene>("uid://b7a2h0hw00vtn");// combat screen
		
		/*
		User data should store:
		player ship template ID
		HealthModifierCount (from level ups)
		ArmorModifierCount (from level ups)
		ManeuverabilityModifierCount(from level ups)
		CritChanceModifierCount (from level ups)
		Level
		
		Enemy ship template ID
		EnemyHealthModifierCount (from level ups)
		EnemyArmorModifierCount (from level ups)
		EnemyManeuverabilityModifierCount(from level ups)
		EnemyCritChanceModifierCount (from level ups)
		EnemyLevel


		*/
		Dictionary run_data = new Dictionary();
		run_data.Add("player", new Array
		{
			"s_player_start",
			0,
			0,
			0,
			0,
			1,
		});
		run_data.Add("enemy", new Array
		{
			"s_player_start",
			0,
			0,
			0,
			0,
			1
		});
		
		Dictionary ship_data = new Dictionary();
		Dictionary first = new Dictionary();
		first.Add("x", -100);
		first.Add("y", 0);

		Dictionary second = new Dictionary();
		second.Add("x", 0);
		second.Add("y", 0);
		
		Dictionary third = new Dictionary();
		third.Add("x", 100);
		third.Add("y", 0);
		
		ship_data.Add("s_player_start", new Array
		{
			100,
			100,
			10,
			new Array{first, second, third},
			new Array{"light", "medium", "light"},
			400,
			"uid://55gey740o2lm"



		});
		









		RunData.Instance.SaveToUserData(Json.Stringify(run_data));

		Array p_run_data_arr =(Array)RunData.Instance.LoadUserData()["player"];
		

		Json json_loader = new Json();
		json_loader.Parse(Json.Stringify(ConstantData.ShipData[p_run_data_arr[(int)RunDataEnum.SHIP_TEMPLATE_ID].ToString()]));
		Array template_data = (Array)(json_loader.Data);
		json_loader.Parse(Json.Stringify(template_data[(int)ShipDataEnum.HARDPOINT_LOCATIONS]));
		Array hardpoint_locs = (Array)json_loader.Data;
		//Debug.Print(hardpoint_locs.ToString());
		
		
		
		
		
		/*
		//TEST PLAYER STATS
		PlayerStats.Instance.max_health = 100;
		PlayerStats.Instance.health = PlayerStats.Instance.max_health;

		PlayerStats.Instance.max_armor = 100;
		PlayerStats.Instance.armor = PlayerStats.Instance.max_armor;
		
		PlayerStats.Instance.maneuverability = 10;


		PlayerStats.Instance.available_hardpoints = new List<Hardpoint>
		{
			new Hardpoint(1, "light",  "lightmachinegun"),
			new Hardpoint(1, "medium",  "mediumcannon"),
			new Hardpoint(1, "light",  "lightmachinegun")
		};

		PlayerStats.Instance.hardpoint_locations = new Vector2[3] {
			new Vector2(-100,0),
			new Vector2(0,0),
			new Vector2(100,0),

		};

		PlayerStats.Instance.hardpoint_weight_classes = new string[3]
		{
			"light",
			"medium",
			"light"
		};
		PlayerStats.Instance.ship_UID = "uid://55gey740o2lm";
		PlayerStats.Instance.ship_width = 400;
		




		//TEST ENEMY STATS
		
		EnemyStats.Instance.max_health = 100;
		EnemyStats.Instance.health = EnemyStats.Instance.max_health;

		EnemyStats.Instance.max_armor = 100;
		EnemyStats.Instance.armor = EnemyStats.Instance.max_armor;
		
		EnemyStats.Instance.maneuverability = 10;


		EnemyStats.Instance.available_hardpoints = new List<Hardpoint>
		{
			new Hardpoint(1, "light",  "lightmachinegun"),
			new Hardpoint(1, "medium",  "mediumcannon"),
			new Hardpoint(1, "light",  "lightmachinegun")
		};

		EnemyStats.Instance.hardpoint_locations = new Vector2[3] {
			new Vector2(-100,0),
			new Vector2(0,0),
			new Vector2(100,0),

		};

		EnemyStats.Instance.hardpoint_weight_classes = new string[3]
		{
			"light",
			"medium",
			"light"
		};
		EnemyStats.Instance.ship_UID = "uid://55gey740o2lm";
		EnemyStats.Instance.ship_width = 400;
	
		*/
    }

	private void _OnStartGame()
	{
		GetTree().ChangeSceneToPacked(main_scene);
		
	}

	
}
