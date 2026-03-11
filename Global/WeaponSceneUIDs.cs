using Godot;
using System;

public partial class WeaponSceneUIDs : Node
{
	public static WeaponSceneUIDs Instance;

	public override void _Ready()
	{
		Instance = this;
	}
	public string getWeaponUID(string name)
	{
		if (name.Equals("lightmachinegun"))
		{
			
			return "uid://vwk20d4x7ag7";
			
		}

		else if (name.Equals("mediumcannon"))
		{
			
			return "uid://bhbke6nd0s8oj";
			
		}
		else
		{
			return null;
		}

		
	}
}
