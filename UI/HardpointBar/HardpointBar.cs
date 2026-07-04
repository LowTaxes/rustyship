using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class HardpointBar : Control
{
	List<Hardpoint> active_hardpoints;
	List<InventoryItem> active_bar_inv_items;
    public override void _EnterTree()
    {
        SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointAdded, new Callable(this, "_OnHardpointAdded"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointRemoved, new Callable(this, "_OnHardpointRemoved"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.InvItemClicked, new Callable(this, "_OnInvItemClicked"));
	}
    public override void _Ready()
    {

		active_hardpoints = new List<Hardpoint>();
		active_bar_inv_items = new List<InventoryItem>();

		

	}

	private void _OnHardpointAdded(Hardpoint hardpoint)
	{
		if(!active_hardpoints.Contains(hardpoint))
		{
			active_hardpoints.Add(hardpoint);

			PackedScene inv_item_scene = null;
			InventoryItem new_inv_item = null;
			if(ConstantData.GetBattleItemDesignation(hardpoint.attatched_weaponID).Equals(Constants.WEAPON_DESIGNATION))
			{
				inv_item_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponInvItemUID(hardpoint.attatched_weaponID));
				new_inv_item = inv_item_scene.Instantiate<InventoryItem>();
				new_inv_item.size_x = (int)ConstantData.GetWeaponInventoryItemSize(hardpoint.attatched_weaponID).X;
				new_inv_item.size_y = (int)ConstantData.GetWeaponInventoryItemSize(hardpoint.attatched_weaponID).Y;
			}
			else if (ConstantData.GetBattleItemDesignation(hardpoint.attatched_weaponID).Equals(Constants.DEFENSIVE_DESIGNATION))
			{
				inv_item_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetDefensiveInventoryItemUID(hardpoint.attatched_weaponID));
				new_inv_item = inv_item_scene.Instantiate<InventoryItem>();
				new_inv_item.size_x = (int)ConstantData.GetDefensiveInventoryItemSize(hardpoint.attatched_weaponID).X;
				new_inv_item.size_y = (int)ConstantData.GetDefensiveInventoryItemSize(hardpoint.attatched_weaponID).Y;
			}
			else if (ConstantData.GetBattleItemDesignation(hardpoint.attatched_weaponID).Equals(Constants.SUPPORT_DESIGNATION))
			{
				inv_item_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetSupportInvItemUID(hardpoint.attatched_weaponID));
				new_inv_item = inv_item_scene.Instantiate<InventoryItem>();
				new_inv_item.size_x = (int)ConstantData.GetSupportInvItemSize(hardpoint.attatched_weaponID).X;
				new_inv_item.size_y = (int)ConstantData.GetSupportInvItemSize(hardpoint.attatched_weaponID).Y;
			}

			
			new_inv_item.weapon_name = hardpoint.attatched_weaponID;
			new_inv_item.level = hardpoint.level;
			new_inv_item.Position = new Vector2(0,0);
			active_bar_inv_items.Add(new_inv_item);
			AddChild(new_inv_item);



		}
		UpdateHardpointBar();
		//Debug.Print(active_hardpoints.Count.ToString());
	}

	private void _OnHardpointRemoved(Hardpoint hardpoint)
	{
		if(active_hardpoints.Contains(hardpoint))
		{
			InventoryItem removed_item = active_bar_inv_items[active_hardpoints.IndexOf(hardpoint)];
			active_bar_inv_items.RemoveAt(active_hardpoints.IndexOf(hardpoint));
			removed_item.CallDeferred("free");
			active_hardpoints.Remove(hardpoint);
			
		}
		UpdateHardpointBar();
	}

	private void UpdateHardpointBar()
	{

		int current_pos = Constants.HARDPOINT_BAR_SEPARATION;

		for(int i = 0; i < active_bar_inv_items.Count; i ++)
		{
			int pos_y = Constants.HARDPOINT_BAR_HEIGHT/2;
			int pos_x = current_pos + (active_bar_inv_items[i].size_x * Constants.inventory_square_size/2);
			active_bar_inv_items[i].Position = new Vector2(pos_x, pos_y);
			current_pos += Constants.HARDPOINT_BAR_SEPARATION;
			current_pos += active_bar_inv_items[i].size_x * Constants.inventory_square_size;
		}
		CustomMinimumSize = new Vector2(current_pos, Constants.HARDPOINT_BAR_HEIGHT);
		
	}

	private void _OnInvItemClicked(InventoryItem inv_item)
	{
		for(int i = 0; i < active_bar_inv_items.Count; i++)
		{
			if(active_bar_inv_items[i] == inv_item)
			{
				Hardpoint removed_hardpoint = active_hardpoints[active_bar_inv_items.IndexOf(inv_item)];
				active_hardpoints.Remove(removed_hardpoint);
				SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.PartnerInvItemRemoved, removed_hardpoint);
				active_bar_inv_items.Remove(inv_item);
			}
		}
	}
	
}
