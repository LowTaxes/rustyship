using Godot;
using System;
using System.Diagnostics;

public partial class GameManager : Node2D
{
	public Ship player;
	public Ship enemy;

	public Vector2 player_start;
	public Vector2 enemy_start;
	public override void _Ready()
	{
		
		PackedScene player_ship_scene = ResourceLoader.Load<PackedScene>(PlayerStats.Instance.current_ship_UID);
		PackedScene enemy_ship_scene = ResourceLoader.Load<PackedScene>(EnemyStats.Instance.current_ship_UID);

		player_start = new Vector2(0,200);
		enemy_start = new Vector2(0,-200);
		
		//Spawn Player Features
		
		player = player_ship_scene.Instantiate<Ship>();
		player.is_player = true;
		player.max_health = PlayerStats.Instance.max_health;
		player.health = PlayerStats.Instance.health;
		player.max_armor = PlayerStats.Instance.max_armor;
		player.armor = PlayerStats.Instance.armor;
		player.maneuverability = PlayerStats.Instance.maneuverability;
		player.available_hardpoints = PlayerStats.Instance.available_hardpoints;
		player.hardpoint_locations = PlayerStats.Instance.hardpoint_locations;
		player.hardpoint_weight_classes = PlayerStats.Instance.hardpoint_weight_classes;
		AddChild(player);
		

		//Spawn Enemy Features
		enemy = enemy_ship_scene.Instantiate<Ship>();
		enemy.is_player = false;
		enemy.max_health = EnemyStats.Instance.max_health;
		enemy.health = EnemyStats.Instance.health;
		enemy.max_armor = EnemyStats.Instance.max_armor;
		enemy.armor = EnemyStats.Instance.armor;
		enemy.maneuverability = EnemyStats.Instance.maneuverability;
		enemy.available_hardpoints = EnemyStats.Instance.available_hardpoints;
		enemy.hardpoint_locations = EnemyStats.Instance.hardpoint_locations;
		enemy.hardpoint_weight_classes = EnemyStats.Instance.hardpoint_weight_classes;
		AddChild(enemy);
		
	
		player.Translate(player_start);
		enemy.Translate(enemy_start);
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
