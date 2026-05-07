using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class EquippedItemsScrollBox : ScrollContainer
{
	HBoxContainer h_box;
	List<ActiveInventoryItemBox> active_inv_item_boxes;
	PackedScene active_inv_item_box_scene;
	public override void _Ready()
	{
		h_box = GetChild<HBoxContainer>(0);
		h_box.AddThemeConstantOverride("separation", 250);
		active_inv_item_box_scene = ResourceLoader.Load<PackedScene>("uid://cf2pa4xcr1xj8");
		
		List<string> hardpoint_weight_classes = ConstantData.GetShipHardpointWeightClasses(RunData.GetPlayerShipTemplateID());
		List<InventoryItem> player_active_inv_items = RunData.GetPlayerActiveInventoryItems();
		//Debug.Print(player_active_inv_items.Count.ToString());
		//GD.Print(player_active_inv_items[0].weapon_name);
		for(int i = 0; i <hardpoint_weight_classes.Count; i++)
		{
			ActiveInventoryItemBox new_item_box = active_inv_item_box_scene.Instantiate<ActiveInventoryItemBox>();
			new_item_box.hardpoint_index = i;
			h_box.AddChild(new_item_box);
			new_item_box.UpdateItem(player_active_inv_items[i]);

		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
