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
				PackedScene new_model_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponModelUID(attatched_inventory_items[i].weapon_name));
				Sprite2D new_weapon_model = new_model_scene.Instantiate<Sprite2D>();
				//new_weapon_model.Scale = new Vector2(scale, scale);
				ship_model.hardpoints[i].AddChild(new_weapon_model);
				new_weapon_model.LookAt(new Vector2(new_weapon_model.GlobalPosition.X, new_weapon_model.GlobalPosition.Y *2));
			}
			
		}



	
	}

	
}
