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
	
	public override void _Ready()
	{
		Instance = this;
		
		WeaponData = LoadJsonFile("WeaponData.json");
		
		

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
}

/*
	public void SaveTextToFile(string file_name, string data)
	{
		if(!Directory.Exists(user_path))
		{
			Directory.CreateDirectory(user_path);
		}
		string path = Path.Join(user_path,file_name);
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
		
		string path = Path.Join(user_path,file_name);

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
	*/