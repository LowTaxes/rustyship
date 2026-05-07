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
public partial class RunData : Node
{
	public static RunData Instance;
	string user_path = ProjectSettings.GlobalizePath("user://");


	//RunData Variables are to be used for packaging data between scenes,
	//not direct retrieval of data


	//Player Run Data
	public string p_ship_template_id;
	public int p_health_m_count;
	public int p_armor_m_count;
	public int p_crit_chance_m_count;
	public int p_level;
	public Array p_active_inv;
	public Array p_storage_inv;

	//Enemy Run Data
	public string e_ship_template_id;
	public int e_health_m_count;
	public int e_armor_m_count;
	public int e_crit_chance_m_count;
	public int e_level;
	public Array e_active_inv;

	public override void _Ready()
	{
		Instance = this;
		Debug.Print(user_path);





		p_ship_template_id = GetPlayerShipTemplateID();
		p_health_m_count = GetPlayerHealthModifierCount();
		p_armor_m_count = GetPlayerArmorModifierCount();
		p_level = GetPlayerLevel();
		p_active_inv = (Array)((Array)Instance.LoadUserData()["player"])[(int)Constants.RunDataEnum.ACTIVE_INVENTORY];
		p_storage_inv = (Array)((Array)RunData.Instance.LoadUserData()["player"])[(int)Constants.RunDataEnum.STORAGE_INVENTORY];

		e_ship_template_id = GetEnemyShipTemplateID();
		e_health_m_count = GetEnemyHealthModifierCount();
		e_armor_m_count = GetEnemyArmorModifierCount();
		e_crit_chance_m_count = GetEnemyCritChanceModifierCount();
		e_level = GetEnemyLevel();
		e_active_inv = (Array)((Array)Instance.LoadUserData()["player"])[(int)Constants.RunDataEnum.ACTIVE_INVENTORY]; 

		/*
		User data should store:
		player ship template ID
		HealthModifierCount (from level ups)
		ArmorModifierCount (from level ups)
		ManeuverabilityModifierCount(from level ups)
		CritChanceModifierCount (from level ups)
		
		Enemy ship template ID
		EnemyHealthModifierCount (from level ups)
		EnemyArmorModifierCount (from level ups)
		EnemyManeuverabilityModifierCount(from level ups)
		EnemyCritChanceModifierCount (from level ups)
		EnemyLevel


		*/
		//SaveTextToFile("test", Json.Stringify(ship_dic));

	}

	public void SaveToUserData(string data)
	{
		
		if(!Directory.Exists(user_path))
		{
			Directory.CreateDirectory(user_path);
		}

		string path = Path.Join(user_path,"RunData.json");
		Debug.Print(path);

		try
		{
			File.WriteAllText(path, data);
		}
		catch (Exception e)
		{
			Debug.Print(e.ToString());
		}
	}

	public Dictionary LoadUserData()
	{
		
		string loaded_data = null;

		Json json_loader = new Json();
		
		string path = Path.Join(user_path,"RunData.json");

		if(!File.Exists(path))
		{
			return null;
		}

		try
		{
			loaded_data = File.ReadAllText(path);
		}
		catch(Exception e)
		{
			Debug.Print(e.ToString());
		}

		json_loader.Parse(loaded_data);

		
		return (Dictionary)json_loader.Data;

	}

	public static InventoryItem GetEmptyInvItem()
	{
		InventoryItem inventoryItem = new InventoryItem();
		inventoryItem.weapon_name = "empty";
		inventoryItem.level = 0;
		return inventoryItem;
	}

	public static string GetPlayerShipTemplateID()
	{
		Dictionary run_data = Instance.LoadUserData();
		return ((Array)run_data["player"])[(int)Constants.RunDataEnum.SHIP_TEMPLATE_ID].ToString();
	}
	public static string GetEnemyShipTemplateID()
	{
		Dictionary run_data = Instance.LoadUserData();
		return ((Array)run_data["enemy"])[(int)Constants.RunDataEnum.SHIP_TEMPLATE_ID].ToString();
	}

	public static int GetPlayerHealthModifierCount()
	{
		Dictionary run_data = Instance.LoadUserData();
		return (int)((Array)run_data["player"])[(int)Constants.RunDataEnum.HEALTH_MODIFIER_COUNT];
	}

	public static int GetEnemyHealthModifierCount()
	{
		Dictionary run_data = Instance.LoadUserData();
		return (int)((Array)run_data["enemy"])[(int)Constants.RunDataEnum.HEALTH_MODIFIER_COUNT];
	}

	public static int GetPlayerArmorModifierCount()
	{
		Dictionary run_data = Instance.LoadUserData();
		return (int)((Array)run_data["player"])[(int)Constants.RunDataEnum.ARMOR_MODIFIER_COUNT];
	}
	public static int GetEnemyArmorModifierCount()
	{
		Dictionary run_data = Instance.LoadUserData();
		return (int)((Array)run_data["enemy"])[(int)Constants.RunDataEnum.ARMOR_MODIFIER_COUNT];
	}
	public static int GetPlayerCritChanceModifierCount()
	{
		Dictionary run_data = Instance.LoadUserData();
		return (int)((Array)run_data["player"])[(int)Constants.RunDataEnum.CRIT_CHANCE_MODIFIER_COUNT];
	}
	public static int GetEnemyCritChanceModifierCount()
	{
		Dictionary run_data = Instance.LoadUserData();
		return (int)((Array)run_data["enemy"])[(int)Constants.RunDataEnum.CRIT_CHANCE_MODIFIER_COUNT];
	}

	public static int GetPlayerLevel()
	{
		Dictionary run_data = Instance.LoadUserData();
		return (int)((Array)run_data["player"])[(int)Constants.RunDataEnum.LEVEL];
	}
	public static int GetEnemyLevel()
	{
		Dictionary run_data = Instance.LoadUserData();
		return (int)((Array)run_data["enemy"])[(int)Constants.RunDataEnum.LEVEL];
	}
	public static List<InventoryItem> GetPlayerActiveInventoryItems()
	{
		List<InventoryItem> return_list = new List<InventoryItem>();

		Array active_inventory_items = (Array)((Array)Instance.LoadUserData()["player"])[(int)Constants.RunDataEnum.ACTIVE_INVENTORY];
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
	public static List<InventoryItem> GetEnemyActiveInventoryItems()
	{
		List<InventoryItem> return_list = new List<InventoryItem>();

		Array active_inventory_items = (Array)((Array)Instance.LoadUserData()["enemy"])[(int)Constants.RunDataEnum.ACTIVE_INVENTORY];
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



	public static Vector2[] UnpackArrayOfVector2(string stringified_array)
	{
		Json json_loader = new Json();
		Array unpacked_vectors = new Array();

		json_loader.Parse(stringified_array);
		unpacked_vectors = (Array)json_loader.Data;

		Vector2[] returning_array = new Vector2[unpacked_vectors.Count];
		for (int i = 0; i < unpacked_vectors.Count; i++)
		{
			json_loader.Parse(Json.Stringify(unpacked_vectors[i]));
			Dictionary vector2 = (Dictionary)json_loader.Data;
			returning_array[i] = new Vector2((float)vector2["x"], (float)vector2["y"]);
		}
		
		return returning_array;

	}
}
