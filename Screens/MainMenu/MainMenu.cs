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
	public PackedScene game_loop_scene;
	
	public PackedScene test_ship;

    public override void _Ready()
    {
        
		game_loop_scene = ResourceLoader.Load<PackedScene>("uid://bkxbp4irv6s6o");
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


		
		
    }

	private void _OnNewGame()
	{
		Dictionary active_inv_size = new Dictionary();
		active_inv_size.Add("x", 12);
		active_inv_size.Add("y", 7);
		
		Dictionary storage_inv_size = new Dictionary();
		storage_inv_size.Add("x", 12);
		storage_inv_size.Add("y", 6);

		Dictionary light_1 = new Dictionary();
		light_1.Add("weaponID", "health_basic_light");
		light_1.Add("level", 1);
		light_1.Add("x", 3);
		light_1.Add("y", 3);

		Dictionary medium_1 = new Dictionary();
		medium_1.Add("weaponID", "health_basic_light");
		medium_1.Add("level", 4);
		medium_1.Add("x", 1);
		medium_1.Add("y", 1);

		Dictionary light_2 = new Dictionary();
		light_2.Add("weaponID", "autocannon");
		light_2.Add("level", 1);

		Dictionary medium_2 = new Dictionary();
		medium_2.Add("weaponID", "203mm");
		medium_2.Add("level", 3);
		
		Dictionary empty_1 = new Dictionary();
		empty_1.Add("weaponID", "autocannon");
		empty_1.Add("level", 0);

		
		Dictionary hardpoint_1 = new Dictionary();
		hardpoint_1.Add("level", 1);
		hardpoint_1.Add("weaponID", "autocannon");
		hardpoint_1.Add("x", -200);
		hardpoint_1.Add("y", 0);
		hardpoint_1.Add("weight_class", "light");
		hardpoint_1.Add("inv_x", 0);
		hardpoint_1.Add("inv_y", 0);

		Dictionary hardpoint_2 = new Dictionary();
		hardpoint_2.Add("level", 1);
		hardpoint_2.Add("weaponID", "203mm");
		hardpoint_2.Add("x", 100);
		hardpoint_2.Add("y", 0);
		hardpoint_2.Add("weight_class", "medium");
		hardpoint_2.Add("inv_x", 0);
		hardpoint_2.Add("inv_y", 0);

		Dictionary hardpoint_3 = new Dictionary();
		hardpoint_3.Add("level", 1);
		hardpoint_3.Add("weaponID", "health_basic_light");
		hardpoint_3.Add("x", 0);
		hardpoint_3.Add("y", 0);
		hardpoint_3.Add("weight_class", "light");
		hardpoint_3.Add("inv_x", 0);
		hardpoint_3.Add("inv_y", 0);

		Dictionary hardpoint_4 = new Dictionary();
		hardpoint_4.Add("level", 1);
		hardpoint_4.Add("weaponID", "shield_basic_light");
		hardpoint_4.Add("x", 50);
		hardpoint_4.Add("y", 0);
		hardpoint_4.Add("weight_class", "light");
		hardpoint_4.Add("inv_x", 0);
		hardpoint_4.Add("inv_y", 0);

		Dictionary hardpoint_5 = new Dictionary();
		hardpoint_5.Add("level", 1);
		hardpoint_5.Add("weaponID", "damage_support_flat_light");
		hardpoint_5.Add("x", -50);
		hardpoint_5.Add("y", 0);
		hardpoint_5.Add("weight_class", "light");
		hardpoint_5.Add("inv_x", 0);
		hardpoint_5.Add("inv_y", 0);


		Dictionary run_data = new Dictionary();
		run_data.Add("player", new Array
		{
			"NavalCamo",
			0,
			0,
			0,
			1,
			new Array{light_2, medium_2, empty_1, empty_1, empty_1},
			new Array{medium_1, light_1},
			"0-0",
			new Array{hardpoint_3, hardpoint_4, hardpoint_5},
		});

		
		RunData.Instance.SaveToUserData(Json.Stringify(run_data));
		RunData.Instance.InitializeDataVariables();
		//Debug.Print(RunData.GetPlayerActiveInventoryItems().Count.ToString());
		GetTree().ChangeSceneToPacked(game_loop_scene);
		
	}
	private void _OnContinueGame()
	{

		RunData.Instance.InitializeDataVariables();
		
		GetTree().ChangeSceneToPacked(game_loop_scene);
		
	}

	
}
