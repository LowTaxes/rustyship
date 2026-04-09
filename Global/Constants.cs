using Godot;
using System;

public partial class Constants : Node
{
	public static Vector2 PLAYER_START_LOCATION = new Vector2(0,200);
	public static Vector2 ENEMY_START_LOCATION = new Vector2(0,-200);
	public enum WeaponDataEnum
	{
		DAMAGE,
		ARMOR_DAMAGE_MODIFIER,
		CRIT_CHANCE,
		FIRE_RATE,
		BULLET_UID,
		BULLET_SPEED,
		SPREAD_RADIUS,
		WEAPON_UID
	}

	public enum ShipDataEnum
	{
		MAX_HEALTH,
		MAXARMOR,
		MANEUVERABILITY,
		HARDPOINT_LOCATIONS,
		HARDPOINT_WEIGHT_CLASSES,
		SHIP_WIDTH,
		SHIP_UID
	}


	
}
