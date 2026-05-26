using Godot;
using System;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;
public partial class Lootspawn : Control
{
	//TEMPORARY
	public override void _Ready()
	{
		PackedScene temp_spawn = ResourceLoader.Load<PackedScene>("uid://cm70no81qpt1p");
		InventoryItem new_item = temp_spawn.Instantiate<InventoryItem>();
		new_item.weapon_name = "autocannon";
		new_item.level = 1;
		new_item.is_lootspawn = true;
		
		AddChild(new_item);

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
