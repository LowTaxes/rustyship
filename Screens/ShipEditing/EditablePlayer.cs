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
	
	
	public override void _Ready()
	{

		SignalConnect.Instance.Connect(SignalConnect.SignalName.ActiveItemAdded, new Callable(this, "_ActiveItemAdded"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.ActiveItemRemoved, new Callable(this, "_ActiveItemRemoved"));

		SignalConnect.Instance.Connect(SignalConnect.SignalName.ChangeToNextScene, new Callable(this, "_ChangeToNextScene"));


		//get ship and run data
		Dictionary run_data = RunData.Instance.LoadUserData();
		Array player_data = (Array) run_data["player"];
		Array ship_data = (Array)ConstantData.ShipData[RunData.GetPlayerShipTemplateID()];


		attatched_inventory_items = RunData.GetPlayerActiveInventoryItems();
		//spawn ship and scale it
		ship_model_scene = GD.Load<PackedScene>(ship_data[(int)Constants.ShipDataEnum.SHIP_MODEL_UID].ToString());
		ship_model = ship_model_scene.Instantiate<ShipModel>();
		AddChild(ship_model);
		ship_model.Position = new Vector2(this.Size.X/2, Size.Y/2);

		float scale = this.Size.X/ship_model.Texture.GetWidth();
		//Debug.Print(this.Size.X.ToString());
		//Debug.Print(scale.ToString());
		ship_model.Scale = new Vector2(scale, scale);

		//Debug.Print("scale: " + scale.ToString());
		//Debug.Print("UID:   " + ConstantData.GetWeaponModelUID(attatched_inventory_items[0].weapon_name));
		for(int i = 0; i < ship_model.hardpoints.Count; i++)
		{
			if(!(i >= attatched_inventory_items.Count))
			{
				if(!(attatched_inventory_items[i].weapon_name.Equals("empty")))
				{
					PackedScene new_model_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponModelUID(attatched_inventory_items[i].weapon_name));
					Sprite2D new_weapon_model = new_model_scene.Instantiate<Sprite2D>();
					//new_weapon_model.Scale = new Vector2(scale, scale);
					ship_model.hardpoints[i].AddChild(new_weapon_model);
					new_weapon_model.LookAt(new Vector2(new_weapon_model.GlobalPosition.X, new_weapon_model.GlobalPosition.Y *2));
				}
				
			}
			
		}


	
	}

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
