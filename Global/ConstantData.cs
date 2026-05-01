using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
using FileAccess = Godot.FileAccess;
public partial class ConstantData : Node
{
	public static ConstantData Instance;
	string data_path = "res://Data/ConstantData/";

	public static Dictionary WeaponData;
	public static Dictionary ShipData;

	
	public override void _Ready()
	{
		Instance = this;
		
		WeaponData = LoadJsonFile("WeaponData.json");
		ShipData = LoadJsonFile("ShipData.json");

		
		

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


	public static string GetWeaponModelUID(string weapon_name)
	{
		return (((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.WEAPON_MODEL_UID]).ToString();
	}

	public static string GetWeaponInventoryItemSpriteUID(string weapon_name)
	{
		return (((Array)WeaponData[weapon_name])[(int)Constants.WeaponDataEnum.INVENTORY_ITEM_SPRITE_UID]).ToString();
	}
	
	
}