using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
using FileAccess = Godot.FileAccess;
using System.Collections.Generic;
public partial class ConstantData : Node
{
	public static ConstantData Instance;
	string data_path = "res://Data/ConstantData/";

	public static Dictionary WeaponData;
	public static Dictionary ShipData;
	public static Dictionary LevelData;

	
	public override void _Ready()
	{
		Instance = this;
		
		WeaponData = LoadJsonFile("WeaponData.json");
		ShipData = LoadJsonFile("ShipData.json");
		LevelData = LoadJsonFile("LevelData.json");

		
		

	}

	public Dictionary LoadJsonFile(string file_name)
	{

		
		Dictionary loaded_data = null;
		string path = Path.Join(data_path,file_name);
		
		if(!FileAccess.FileExists(path))
		{
			Debug.Print("File: " + path + " does not exist");
			return null;
		}

		FileAccess file = FileAccess.Open(path,FileAccess.ModeFlags.Read);

		loaded_data = (Dictionary) Json.ParseString(file.GetAsText());
		if(loaded_data.GetType() == typeof(Dictionary))
		{
			return loaded_data;
		}
		else
		{
			Debug.Print("file is not a dictionary");
			return null;
		}
		
	
	}


	public static float GetShipTemplateHealth(string ship_ID)
	{
		return (float)((Array)ShipData[ship_ID])[(int)Constants.ShipDataEnum.MAX_HEALTH];
	}
	public static float GetShipTemplateArmor(string ship_ID)
	{
		return (float)((Array)ShipData[ship_ID])[(int)Constants.ShipDataEnum.MAX_ARMOR];
	}

	public static string GetShipModelUID(string ship_ID)
	{
		return ((Array)ShipData[ship_ID])[(int)Constants.ShipDataEnum.SHIP_MODEL_UID].ToString();
	}







	public static double GetWeaponDamage(string weapon_name)
	{
		return (double)((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.DAMAGE];
	}

	public static double GetWeaponFirerate(string weapon_name)
	{
		return (double)((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.FIRE_RATE];
	}
	public static double GetWeaponArmorDamageModifier(string weapon_name)
	{
		return (double)((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.ARMOR_DAMAGE_MODIFIER];
	}
	public static double GetWeaponCritChance(string weapon_name)
	{
		return (double)((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.CRIT_CHANCE];
	}

	public static string GetWeaponModelUID(string weapon_name)
	{
		return (((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.WEAPON_MODEL_UID]).ToString();
	}

	public static string GetWeaponInventoryItemSpriteUID(string weapon_name)
	{
		return (((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.INVENTORY_ITEM_SPRITE_UID]).ToString();
	}
	

	public static List<string> GetShipHardpointWeightClasses(string ship_ID)
	{
		List<string> return_list = new List<string>();

		Array hardpoints = (Array)((Array)ShipData[ship_ID])[(int)Constants.ShipDataEnum.HARDPOINT_WEIGHT_CLASSES];
		for (int i = 0; i < hardpoints.Count; i++)
		{
			return_list.Add(hardpoints[i].ToString());
		}

		return return_list;
	}

	public static string GetLevelShipTemplateID(string level_ID)
	{
		return ((Array)LevelData[level_ID])[(int)Constants.LevelDataEnum.SHIP_TEMPLATE_ID].ToString();
		
	}

	public static string GetLevelEnemyName(string level_ID)
	{
		return ((Array)LevelData[level_ID])[(int)Constants.LevelDataEnum.ENEMY_NAME].ToString();
		
	}

	public static int GetLevelHealthModifierCount(string level_ID)
	{
		return (int)((Array)LevelData[level_ID])[(int)Constants.LevelDataEnum.HEALTH_MODIFIER_COUNT];
		
	}
	public static int GetLevelArmorModifierCount(string level_ID)
	{
		return (int)((Array)LevelData[level_ID])[(int)Constants.LevelDataEnum.ARMOR_MODIFIER_COUNT];
		
	}

	public static int GetLevelCritModifierCount(string level_ID)
	{
		return (int)((Array)LevelData[level_ID])[(int)Constants.LevelDataEnum.CRIT_CHANCE_MODIFIER_COUNT];
	}

	public static int GetLevelEnemyLevel(string level_ID)
	{
		return (int)((Array)LevelData[level_ID])[(int)Constants.LevelDataEnum.LEVEL];
	}
	
	public static List<InventoryItem> GetLevelEnemyActiveInventoryItems(string level_ID)
	{
		List<InventoryItem> return_list = new List<InventoryItem>();

		Array active_inventory_items = (Array)((Array)(LevelData[level_ID]))[(int)Constants.LevelDataEnum.ACTIVE_INVENTORY];
		for (int i = 0; i < active_inventory_items.Count; i ++)
		{
			Dictionary new_item_dict = (Dictionary)active_inventory_items[i];
			InventoryItem new_inv_item = (GD.Load<PackedScene>("uid://cedh3uq18etis")).Instantiate<InventoryItem>();
			new_inv_item.weapon_name = new_item_dict["weaponID"].ToString();
			new_inv_item.level = (int)new_item_dict["level"];
			return_list.Add(new_inv_item);
		}
		return return_list;
	}
}