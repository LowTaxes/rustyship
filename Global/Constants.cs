using Godot;
using System;

public partial class Constants : Node
{
	public static Vector2 PLAYER_START_LOCATION = new Vector2(0,200);
	public static Vector2 ENEMY_START_LOCATION = new Vector2(0,-200);
	public static int player_storage_size_x = 5;
	public static int player_storage_size_y = 10;
	public static int inventory_square_size = 50;

	
	public enum WeaponDataEnum
	{
		DAMAGE,
		ARMOR_DAMAGE_MODIFIER,
		CRIT_CHANCE,
		FIRE_RATE,
		BULLET_UID,
		BULLET_SPEED,
		SPREAD_RADIUS,
		WEAPON_MODEL_UID,
		INVENTORY_ITEM_SIZE,
		INVENTORY_ITEM_SPRITE_UID
	}

	public enum ShipDataEnum
	{
		MAX_HEALTH,
		MAX_ARMOR,
		MANEUVERABILITY,
		HARDPOINT_LOCATIONS,
		HARDPOINT_WEIGHT_CLASSES,
		SHIP_WIDTH,
		SHIP_UID,
		SHIP_MODEL_UID
	}

	public enum RunDataEnum
	{
		SHIP_TEMPLATE_ID,
		HEALTH_MODIFIER_COUNT,
		ARMOR_MODIFIER_COUNT,
		CRIT_CHANCE_MODIFIER_COUNT,
		LEVEL,
		ACTIVE_INVENTORY,
		STORAGE_INVENTORY,
	}


	
}
