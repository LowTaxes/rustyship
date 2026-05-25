using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Array = Godot.Collections.Array;
using System.Diagnostics;

public partial class EditablePlayer : Control
{
	PackedScene ship_model_scene;
	List<InventoryItem> attatched_inventory_items;
	ShipModel ship_model;
	List<Hardpoint> active_hardpoints;
	public bool hardpoint_editing = false;
	
	
	public override void _Ready()
	{

		SignalConnect.Instance.Connect(SignalConnect.SignalName.ActiveItemAdded, new Callable(this, "_ActiveItemAdded"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.ActiveItemRemoved, new Callable(this, "_ActiveItemRemoved"));

		SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointAdded, new Callable(this, "_HardpointAdded"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointRemoved, new Callable(this, "_HardpointRemoved"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.CanEditHardpoints, new Callable(this, "_CanEditHardpoints"));

		SignalConnect.Instance.Connect(SignalConnect.SignalName.ChangeToNextScene, new Callable(this, "_ChangeToNextScene"));


		//get ship and run data
		Dictionary run_data = RunData.Instance.LoadUserData();
		Array player_data = (Array) run_data["player"];
		Array ship_data = (Array)ConstantData.ShipData[RunData.GetPlayerShipTemplateID()];


		
		//spawn ship and scale it
		ship_model_scene = GD.Load<PackedScene>(ship_data[(int)Constants.ShipDataEnum.SHIP_MODEL_UID].ToString());
		ship_model = ship_model_scene.Instantiate<ShipModel>();
		AddChild(ship_model);
		//ship_model.Position = new Vector2(this.Size.X/2, Size.Y/2);

		active_hardpoints = RunData.GetPlayerActiveHardpoints();

		for(int i = 0; i<active_hardpoints.Count; i++)
		{
			//Debug.Print("hi");
			AddChild(active_hardpoints[i]);
			active_hardpoints[i].Position += active_hardpoints[i].placement_position;
			Sprite2D weapon_model = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponModelUID(active_hardpoints[i].attatched_weaponID)).Instantiate<Sprite2D>();
			active_hardpoints[i].AddChild(weapon_model);
			weapon_model.LookAt(new Vector2(weapon_model.GlobalPosition.X, weapon_model.GlobalPosition.Y-1));
		}
		
	}

	private void _HardpointRemoved(Hardpoint hardpoint)
	{
		
		if(hardpoint_editing)
		{
			for(int i = 0; i < active_hardpoints.Count; i++)
			{
				if(active_hardpoints[i] == hardpoint)
				{
					Debug.Print(active_hardpoints.Count.ToString());
					active_hardpoints[i].moveable = true;
					active_hardpoints.Remove(hardpoint);
					Debug.Print(active_hardpoints.Count.ToString());
					

				}
			}
		}
		
		
	}

	private void _HardpointAdded(Hardpoint hardpoint)
	{
		Area2D model_area2d = ship_model.GetChild<Area2D>(0);
		Array<Area2D> overlapping_areas = model_area2d.GetOverlappingAreas();
		for(int i = 0; i < overlapping_areas.Count;i++)
		{
			if(overlapping_areas[i] == hardpoint.area2D)
			{
				active_hardpoints.Add(hardpoint);
				hardpoint.moveable = false;
				hardpoint.Reparent(this);
				hardpoint.placement_position = hardpoint.Position;
			}
		}
	}

	private void _CanEditHardpoints(bool can_edit)
	{
		hardpoint_editing = can_edit;
	}
/*
	private void _ActiveItemAdded(InventoryItem inv_item, int hardpoint_index)
	{
		PackedScene new_model_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponModelUID(inv_item.weapon_name));
		Sprite2D new_weapon_model = new_model_scene.Instantiate<Sprite2D>();

		foreach (Sprite2D model in ship_model.hardpoints[hardpoint_index].GetChildren())
		{
			model.Free();
		}
		ship_model.hardpoints[hardpoint_index].AddChild(new_weapon_model);
		new_weapon_model.LookAt(new Vector2(new_weapon_model.GlobalPosition.X, new_weapon_model.GlobalPosition.Y *2));

		attatched_inventory_items[hardpoint_index] = inv_item;

	}

	private void _ActiveItemRemoved(int hardpoint_index)
	{
		foreach (Sprite2D model in ship_model.hardpoints[hardpoint_index].GetChildren())
		{
			model.Free();
		}
		attatched_inventory_items[hardpoint_index] = RunData.GetEmptyInvItem();

		
	}
*/

	private void _ChangeToNextScene()
	{
		Array array_attatched_items = new Array();
		for(int i = 0; i < attatched_inventory_items.Count; i++)
		{
			Dictionary curr_item = new Dictionary();
			curr_item.Add("weaponID", attatched_inventory_items[i].weapon_name);
			curr_item.Add("level", attatched_inventory_items[i].level);
			array_attatched_items.Add(curr_item);
		}
		RunData.Instance.p_active_inv = array_attatched_items;
	}

	
}
