using Godot;
using System;
using System.Collections.Generic;

public partial class ShipModel : Sprite2D
{
	public List<Control> hardpoints;
	public override void _Ready()
	{
		hardpoints = new List<Control>();
		for (int i = 0; i < GetChildCount() ; i++)
		{
			if(GetChild(i) is Control hardpoint)
				hardpoints.Add(hardpoint);
		}
	}
}
