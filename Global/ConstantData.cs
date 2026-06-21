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
	public static Dictionary DefensiveData;

	
	public override void _Ready()
	{
		Instance = this;
		
		WeaponData = LoadJsonFile("WeaponData.json");
		ShipData = LoadJsonFile("ShipData.json");
		LevelData = LoadJsonFile("LevelData.json");
		DefensiveData = LoadJsonFile("DefensiveData.json");

		
		

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



	public static string GetBattleItemDesignation(string battle_item_ID)
	{
		if(WeaponData.ContainsKey(battle_item_ID))
		{
			return Constants.WEAPON_DESIGNATION;
		}
		else if(DefensiveData.ContainsKey(battle_item_ID))
		{
			return Constants.DEFENSIVE_DESIGNATION;
		}

		return "none";
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






/////////////////////////////////////////////////////WEAPONINFO
/// 
/// 
/// 
/// 

	public static double GetWeaponDamage(string weapon_name)
	{
		return (double)((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.DAMAGE];
	}

	public static double GetWeaponFirerate(string weapon_name)
	{
		return (double)((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.FIRE_RATE];
	}
	public static double GetWeaponMultishotTimespan(string weapon_name)
	{
		return (double)((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.MULTI_SHOT_TIMESPAN];
	}

	public static int GetWeaponVolleyCount(string weapon_name)
	{
		return (int)((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.VOLLEY_COUNT];
	}
	public static int GetVolleyCount(string weapon_name)
	{
		return (int)((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.VOLLEY_COUNT];
	}
	public static Vector2 GetWeaponInventoryItemSize(string weapon_name)
	{
		Dictionary inv_size = (Dictionary)(((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.INVENTORY_ITEM_SIZE]);
		int size_x = (int) inv_size["x"];
		int size_y = (int) inv_size["y"];
		return new Vector2(size_x,size_y);
	}

	public static string GetWeaponBulletUID(string weapon_name)
	{
		return ((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.BULLET_UID].ToString();
	}
	
	public static int GetWeaponBulletSpeed(string weapon_name)
	{
		return (int)((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.BULLET_SPEED];
	}

	public static int GetWeaponSpreadRadius(string weapon_name)
	{
		return (int)((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.SPREAD_RADIUS];
	}

	public static int GetWeaponVolleySpreadRadius(string weapon_name)
	{
		return (int)((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.VOLLEY_SPREAD_RADIUS];
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
	public static string GetWeaponInfoSpriteUID(string weapon_name)
	{
		return (((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.INFO_SPRITE_UID]).ToString();
	}

	public static string GetWeaponInvItemUID(string weapon_name)
	{
		return (((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.INV_ITEM_UID]).ToString();
	}
	public static string GetWeaponSceneUID(string weapon_name)
	{
		return ((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.BATTLE_SCENE_UID].ToString();
	}
	public static string GetWeaponWeightClass(string weapon_name)
	{
		return (((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.WEIGHT_CLASS]).ToString();
	}

	/// 
	/// 
	/// 
	/// 
	/////////////////////////////////////////////////////WEAPON INFO







	/////////////////////////////////////////////////////DEFENSIVE INFO
	/// 
	/// 
	/// 
	/// 
	
	public static double GetDefensiveHealing(string weapon_name)
	{
		return (double)((Array)DefensiveData[weapon_name])[(int)Constants.DefensiveDataEnum.HEALING];
	}

	public static double GetDefensiveFirerate(string weapon_name)
	{
		return (double)((Array)DefensiveData[weapon_name])[(int)Constants.DefensiveDataEnum.FIRE_RATE];
	}
	public static double GetDefensiveMultishotTimespan(string weapon_name)
	{
		return (double)((Array)DefensiveData[weapon_name])[(int)Constants.DefensiveDataEnum.MULTI_SHOT_TIMESPAN];
	}

	public static int GetDefensiveVolleyCount(string weapon_name)
	{
		return (int)((Array)DefensiveData[weapon_name])[(int)Constants.DefensiveDataEnum.VOLLEY_COUNT];
	}
	public static string GetDefensiveBattleModelUID(string weapon_name)
	{
		return ((Array)DefensiveData[weapon_name])[(int)Constants.DefensiveDataEnum.BATTLE_MODEL_UID].ToString();
	}


	public static Vector2 GetDefensiveInventoryItemSize(string weapon_name)
	{
		Dictionary inv_size = (Dictionary)((Array)DefensiveData[weapon_name])[(int)Constants.DefensiveDataEnum.INVENTORY_ITEM_SIZE];
		int size_x = (int) inv_size["x"];
		int size_y = (int) inv_size["y"];
		return new Vector2(size_x,size_y);
	}

	public static string GetDefensiveInventoryItemSpriteUID(string weapon_name)
	{
		return ((Array)DefensiveData[weapon_name])[(int)Constants.DefensiveDataEnum.INVENTORY_ITEM_SPRITE_UID].ToString();
	}

	public static string GetDefensiveInventoryItemUID(string weapon_name)
	{
		return ((Array)DefensiveData[weapon_name])[(int)Constants.DefensiveDataEnum.INV_ITEM_UID].ToString();
	}
	public static string GetDefensiveBattleSceneUID(string weapon_name)
	{
		return ((Array)DefensiveData[weapon_name])[(int)Constants.DefensiveDataEnum.BATTLE_SCENE_UID].ToString();
	}
	public static string GetDefensiveWeightClass(string weapon_name)
	{
		return ((Array)DefensiveData[weapon_name])[(int)Constants.DefensiveDataEnum.WEIGHT_CLASS].ToString();
	}

	









	/// 
	/// 
	/// 
	/// 
	/////////////////////////////////////////////////////DEFENSIVE INFO

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
	
	public static List<Hardpoint> GetLevelEnemyActiveHarpoints(string level_ID)
	{
		List<Hardpoint> return_list = new List<Hardpoint>();

		Array active_hardpoints = (Array)((Array)(LevelData[level_ID]))[(int)Constants.LevelDataEnum.ENEMY_WEAPONS];
		for (int i = 0; i < active_hardpoints.Count; i ++)
		{
			Dictionary new_hardpoint_dict = (Dictionary)active_hardpoints[i];
			Hardpoint new_hardpoint = (GD.Load<PackedScene>("uid://1h4nrs17ravr")).Instantiate<Hardpoint>();

			new_hardpoint.level = (int)new_hardpoint_dict["level"];
			new_hardpoint.attatched_weaponID = new_hardpoint_dict["weaponID"].ToString();
			new_hardpoint.placement_position = new Vector2((int)new_hardpoint_dict["x"],(int)new_hardpoint_dict["y"]);
			return_list.Add(new_hardpoint);
		}
		return return_list;
	}
}