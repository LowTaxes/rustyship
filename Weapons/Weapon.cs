using Godot;
using System;

public partial class Weapon : Sprite2D
{
	
	public int damage;
	public double armor_damage_modifier;
	public double crit_chance;
	public double fire_rate;
	public Timer fire_rate_timer;
	public string weaponUID;
	public string bulletUID;
	public double bullet_speed;

	public Weapon()
	{

	}
	public Weapon(string weaponname)
	{
		if (weaponname.Equals("lightmachinegun"))
		{
			damage = 1;
			armor_damage_modifier = .25;
			crit_chance = .01;
			fire_rate = .5;
			weaponUID = "uid://vwk20d4x7ag7";
			bulletUID = "uid://cbdlciicv5vn6";
			bullet_speed = 1;
		}

		if (weaponname.Equals("mediumcannon"))
		{
			damage = 10;
			armor_damage_modifier = 1;
			crit_chance = .05;
			fire_rate = 6;
			weaponUID = "uid://bhbke6nd0s8oj";
			bulletUID = "uid://cbdlciicv5vn6";
			bullet_speed = .5; 
		}
	}
	
}
