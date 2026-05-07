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
	public PackedScene ship_editing_scene;
	
	public PackedScene test_ship;

    public override void _Ready()
    {
        main_scene = ResourceLoader.Load<PackedScene>("uid://b7a2h0hw00vtn");// combat screen
		ship_editing_scene = ResourceLoader.Load<PackedScene>("uid://drqy04x8yiama");
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


		Dictionary active_inv_size = new Dictionary();
		active_inv_size.Add("x", 12);
		active_inv_size.Add("y", 7);
		
		Dictionary storage_inv_size = new Dictionary();
		storage_inv_size.Add("x", 12);
		storage_inv_size.Add("y", 6);

		Dictionary light_1 = new Dictionary();
		light_1.Add("weaponID", "lightmachinegun");
		light_1.Add("level", 1);
		light_1.Add("x", 3);
		light_1.Add("y", 3);

		Dictionary medium_1 = new Dictionary();
		medium_1.Add("weaponID", "mediumcannon");
		medium_1.Add("level", 4);
		medium_1.Add("x", 1);
		medium_1.Add("y", 1);

		Dictionary light_2 = new Dictionary();
		light_2.Add("weaponID", "lightmachinegun");
		light_2.Add("level", 1);

		Dictionary medium_2 = new Dictionary();
		medium_2.Add("weaponID", "mediumcannon");
		medium_2.Add("level", 3);
		
		Dictionary empty_1 = new Dictionary();
		empty_1.Add("weaponID", "empty");
		empty_1.Add("level", 0);

		
		


		Dictionary run_data = new Dictionary();
		run_data.Add("player", new Array
		{
			"s_player_start",
			0,
			0,
			0,
			1,
			new Array{light_2, medium_2, empty_1},
			new Array{medium_1, light_1},
		});
		run_data.Add("enemy", new Array
		{
			"s_enemy_start",
			0,
			0,
			0,
			1,
			new Array{light_1},
		});
		/*
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
		*/
		RunData.Instance.SaveToUserData(Json.Stringify(run_data));

		
    }

	private void _OnStartGame()
	{
		GetTree().ChangeSceneToPacked(ship_editing_scene);
		
	}

	
}
