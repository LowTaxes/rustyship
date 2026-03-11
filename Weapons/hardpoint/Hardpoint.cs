using Godot;
using System;

public partial class Hardpoint
{
	public int level;
	public string weight_class; // light, medium, heavy, superheavy
	public string attatched_weapon_name;

	public Hardpoint(int level, string weight_class, string attatched_weapon_name)
	{
		this.level = level;
		this.weight_class = weight_class;
		this.attatched_weapon_name = attatched_weapon_name;
	}
	
}
