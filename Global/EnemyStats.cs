using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyStats : Node
{
	public static EnemyStats Instance;
	public int max_health = 0;
	public int health = 0;
	public int max_armor = 0;

	public int armor = 0;
	public int maneuverability = 0;
	
	public List<Hardpoint> available_hardpoints;
	public Vector2[] hardpoint_locations;
	public string[] hardpoint_weight_classes;
	public int ship_width;
	public Vector2 ship_start_point;

	public string current_ship_UID;
	public override void _Ready()
    {
        Instance = this;
    }
}
