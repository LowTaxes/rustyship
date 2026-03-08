using Godot;
using System;

public partial class Hardpoint
{
	public int level;
	public string weight_class; // light, medium, heavy, superheavy
	public Weapon attatched_weapon;

	public Hardpoint(int level, string weight_class, Weapon attatched_weapon)
	{
		this.level = level;
		this.weight_class = weight_class;
		this. attatched_weapon = attatched_weapon;
	}
	
}
