using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
using FileAccess = Godot.FileAccess;
public partial class RunData : Node
{
	public static RunData Instance;
	string user_path = ProjectSettings.GlobalizePath("user://");
	public override void _Ready()
	{
		Instance = this;
		Debug.Print(user_path);

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

	public static Array UnpackListOfVector2(string stringified_array)
	{
		Json json_loader = new Json();
		Array unpacked_vectors = new Array();

		json_loader.Parse(stringified_array);
		unpacked_vectors = (Array)json_loader.Data;

		for (int i = 0; i < unpacked_vectors.Count; i++)
		{
			json_loader.Parse(Json.Stringify(unpacked_vectors[i]));
			Dictionary vector2 = (Dictionary)json_loader.Data;
			unpacked_vectors[i] = new Vector2((float)vector2["x"], (float)vector2["y"]);
		}
		
		return unpacked_vectors;

	}
}
