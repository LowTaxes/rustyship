using Godot;
using System;

public partial class Constants : Node
{
	public static Vector2 PLAYER_START_LOCATION = new Vector2(0,57);
	public static Vector2 ENEMY_START_LOCATION = new Vector2(0,-57);
	public static int player_storage_size_x = 10;
	public static int player_storage_size_y = 5;
	public static int inventory_square_size = 10;
	public static int pixel_size = 1;
	public static float health_modifier = .1f;
	public static float armor_modifier = .1f;
	public static float camera_zoom_dragging_modifier = 1;

	public static int SEPERATOR_Y = 10;
	public static int UPPER_SEPERATOR_Y = -30;

	public static string WEAPON_DESIGNATION = "weapon";
	public static string DEFENSIVE_DESIGNATION = "defensive";
	public static string SUPPORT_DESIGNATION = "support";
	public static int HARDPOINT_BAR_SEPARATION = 10;
	public static int HARDPOINT_BAR_HEIGHT = 30;


	public static Color COLOR_RED = new Color(255, 44, 33);
	public static Color COLOR_GREEN = new Color(0, 178, 255);
	public static Color COLOR_YELLOW = new Color(255, 254, 56);

	
	public enum WeaponDataEnum
	{
		DAMAGE,
		ARMOR_DAMAGE_MODIFIER,
		CRIT_CHANCE,
		FIRE_RATE,
		MULTI_SHOT_TIMESPAN,
		VOLLEY_COUNT,
		BULLET_UID,
		BULLET_SPEED,
		SPREAD_RADIUS,
		VOLLEY_SPREAD_RADIUS,
		WEAPON_MODEL_UID,
		INVENTORY_ITEM_SIZE,
		INVENTORY_ITEM_SPRITE_UID,
		INFO_SPRITE_UID,
		INV_ITEM_UID,
		BATTLE_SCENE_UID,
		WEIGHT_CLASS,
		PLACEMENT_RADIUS
	}
	public enum DefensiveDataEnum
	{
		HEALING,
		FIRE_RATE,
		MULTI_SHOT_TIMESPAN,
		VOLLEY_COUNT,
		BATTLE_MODEL_UID,
		INVENTORY_ITEM_SIZE,
		INVENTORY_ITEM_SPRITE_UID,
		INV_ITEM_UID,
		BATTLE_SCENE_UID,
		WEIGHT_CLASS,
		HEALING_TYPE,
		PLACEMENT_RADIUS
		

	}

	public enum SupportDataEnum
	{
		SUPPORT_TYPE,
		MODIFIER_TYPE,
		SUPPORT_AMOUNT,
		SUPPORT_RADIUS,
		BATTLE_MODEL_UID,
		INVENTORY_ITEM_SIZE,
		INVENTORY_ITEM_SPRITE_UID,
		INV_ITEM_UID,
		BATTLE_SCENE_UID,
		WEIGHT_CLASS,
		PLACEMENT_RADIUS,
		WEIGHT_CLASS_RESTRICTION
	}

	public enum ShipDataEnum
	{
		MAX_HEALTH,
		MAX_ARMOR,
		HARDPOINT_WEIGHT_CLASSES,
		SHIP_WIDTH,
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
		LEVEL_ID,
		ACTIVE_HARDPOINTS,
	}

	public enum LevelDataEnum
	{
		SHIP_TEMPLATE_ID,
		ENEMY_NAME,
		HEALTH_MODIFIER_COUNT,
		ARMOR_MODIFIER_COUNT,
		CRIT_CHANCE_MODIFIER_COUNT,
		LEVEL,
		ENEMY_WEAPONS,
		
	}


	
}
