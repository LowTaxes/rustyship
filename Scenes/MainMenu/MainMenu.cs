using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

public partial class MainMenu : Node2D
{
	
	public PackedScene main_scene;
	
	public PackedScene test_ship;

    public override void _Ready()
    {
        main_scene = ResourceLoader.Load<PackedScene>("uid://b7a2h0hw00vtn");// combat screen
		
		
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
		PlayerStats.Instance.current_ship_UID = "uid://55gey740o2lm";
		PlayerStats.Instance.ship_width = 400;
		PlayerStats.Instance.ship_start_point = new Vector2(0,200);




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
		EnemyStats.Instance.current_ship_UID = "uid://55gey740o2lm";
		EnemyStats.Instance.ship_width = 400;
		EnemyStats.Instance.ship_start_point = new Vector2(0,-200);
		
    }

	private void _OnStartGame()
	{
		GetTree().ChangeSceneToPacked(main_scene);
		
	}

	
}
