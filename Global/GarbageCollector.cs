using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Array = Godot.Collections.Array;
using System.Diagnostics;
using System.Threading.Tasks;
public partial class GarbageCollector : Node
{
	public static List<Hardpoint> all_hardpoints;
	public static  List<InventoryItem> all_inv_items;
	public override void _Ready()
	{
		SignalConnect.Instance.Connect(SignalConnect.SignalName.StartNewLoop, new Callable(this,"_OnStartNewLoop"));
		all_hardpoints = new List<Hardpoint>();
		all_inv_items = new List<InventoryItem>();
	}

	private void _OnStartNewLoop()
	{
		all_hardpoints.Clear();
		all_inv_items.Clear();
	}

	
}
