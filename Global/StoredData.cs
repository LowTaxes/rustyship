using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
public partial class StoredData : Node
{
	public static StoredData Instance;
	string project_path = ProjectSettings.GlobalizePath("user://");
	public override void _Ready()
	{
		Instance = this;
		/*
		Dictionary weapon_data = new Dictionary();
		weapon_data.Add("lightmachinegun", new Array
		{
			1,
			.25,
			.01,
			.1,
			"uid://cbdlciicv5vn6",
			20,
			50
		});
		weapon_data.Add("mediumcannon", new Array
		{
			10,
			1,
			.05,
			2,
			"uid://cbdlciicv5vn6",
			20,
			100
		});
		

		
		string file_name = "WeaponData";
		string formatted_data = Json.Stringify(weapon_data);

		SaveTextToFile(project_path, file_name, formatted_data);
		
		string file_name = "WeaponData";
		Dictionary retrieved_data = LoadData(project_path, file_name);

		Debug.Print(retrieved_data["heavycannon"].ToString());
		*/
	}

	public void SaveTextToFile(string file_name, string data)
	{
		if(!Directory.Exists(project_path))
		{
			Directory.CreateDirectory(project_path);
		}
		string path = Path.Join(project_path,file_name);
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

	public Dictionary LoadData(string file_name)
	{
		
		string loaded_data = null;

		Json json_loader = new Json();
		
		string path = Path.Join(project_path,file_name);

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
}

