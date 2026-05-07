using Godot;
using System;
using Godot.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection.Metadata;

public partial class ActiveInventoryItemBox : Control
{
	public InventoryItem attatched_item = null;
	public Control inventory_item_holder;
	public Area2D area2D;
	public string weight_class;
	public int hardpoint_index = 0;
	Label weapon_dmg_label;
	Label fire_rate_label;
	Label armor_modifier_label;
	Label crit_chance_label;
	public override void _Ready()
	{
		SignalConnect.Instance.Connect(SignalConnect.SignalName.InvItemClicked, new Callable(this, "_OnInvItemClicked"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.InvItemReleased, new Callable(this, "_OnInvItemReleased"));

		inventory_item_holder = GetChild<Panel>(1).GetChild<Control>(0);
		area2D = GetChild<Area2D>(2);
		Panel lower_panel = GetNode<Panel>("LowerStats");
		weapon_dmg_label = lower_panel.GetNode("DamagePanel").GetChild<Label>(0);
		fire_rate_label = lower_panel.GetNode("FireratePanel").GetChild<Label>(0);
		armor_modifier_label = lower_panel.GetNode("ArmorModifierPanel").GetChild<Label>(0);
		crit_chance_label = lower_panel.GetNode("CritChancePanel").GetChild<Label>(0);

	}

	
	public void UpdateItem(InventoryItem inv_item)
	{
		
		if(!(inv_item.weapon_name.Equals("empty")))
		{
			if(inv_item.GetParent() != null)
			{
				inv_item.Reparent(inventory_item_holder);
			}
			else
			{
				inventory_item_holder.AddChild(inv_item);
			}
			//Debug.Print(inv_item.weapon_name.ToString());
			float sprite_scale = (float)inventory_item_holder.Size.Y / inv_item.sprite2D.Texture.GetHeight();
			inv_item.sprite2D.Scale = new Vector2(sprite_scale,sprite_scale);

			float area_scale_x = (float) inventory_item_holder.Size.X / inv_item.area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.X;
			float area_scale_y = (float) inventory_item_holder.Size.Y / inv_item.area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.Y;
			inv_item.area2D.Scale = new Vector2(area_scale_x,area_scale_y);

			Label level_label = inv_item.GetChild<Label>(2);
			level_label.Text = inv_item.level.ToString();
			level_label.Position = new Vector2(-sprite_scale*inv_item.sprite2D.Texture.GetWidth()/2, -sprite_scale*inv_item.sprite2D.Texture.GetHeight()/2);
			level_label.AddThemeConstantOverride("font_size", 15);
			
			inv_item.Position = new Vector2(inventory_item_holder.Size.X/2, inventory_item_holder.Size.Y/2);
			attatched_item = inv_item;
			attatched_item.mouse_dragging = false;
			attatched_item.attatched = true;

		
			if(inv_item == null)
			{
				weapon_dmg_label.Text = "Damage - " + "0";
				fire_rate_label.Text = "Firerate - " + "0" + "/s";
				armor_modifier_label.Text = "AD - " + "0" + "%";
				crit_chance_label.Text = "Crit - " + "0" + "%";
			}
			else
			{
				weapon_dmg_label.Text = "Damage - " + ConstantData.GetWeaponDamage(inv_item.weapon_name).ToString();
				fire_rate_label.Text = "Firerate - " + ConstantData.GetWeaponFirerate(inv_item.weapon_name).ToString() + "/s";
				armor_modifier_label.Text = "AD - " + ConstantData.GetWeaponArmorDamageModifier(inv_item.weapon_name).ToString() + "%";
				crit_chance_label.Text = "Crit - " + ConstantData.GetWeaponCritChance(inv_item.weapon_name).ToString() + "%";
			}

			SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.ActiveItemAdded, inv_item, hardpoint_index);
		}
		
		


	}


	public void FreeHeldItem()
	{
		attatched_item.sprite_scale_x = (float)Constants.inventory_square_size / attatched_item.sprite2D.Texture.GetWidth() * attatched_item.size_x;
		attatched_item.sprite_scale_y = (float)Constants.inventory_square_size / attatched_item.sprite2D.Texture.GetHeight() * attatched_item.size_y;
		attatched_item.sprite2D.Scale = new Vector2(attatched_item.sprite_scale_x, attatched_item.sprite_scale_y);

		float area_scale_x = (float)Constants.inventory_square_size / attatched_item.area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.X * attatched_item.size_x;
		float area_scale_y = (float)Constants.inventory_square_size / attatched_item.area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.Y * attatched_item.size_y;
		attatched_item.area2D.Scale = new Vector2(area_scale_x, area_scale_y);
		attatched_item.attatched = false;

		Label level_label = attatched_item.GetChild<Label>(2);
		level_label.Text = attatched_item.level.ToString();
		level_label.Position = new Vector2(-Constants.inventory_square_size*attatched_item.size_x/2,-Constants.inventory_square_size*attatched_item.size_y/2);
		//level_label.Scale = new Vector2(attatched_item.sprite_scale_x, attatched_item.sprite_scale_y);

		attatched_item.Reparent(GetNode<Node2D>("/root/ShipEditing"));
		SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.ActiveItemRemoved.ToString(), hardpoint_index);
		attatched_item = null;

		weapon_dmg_label.Text = "Damage - " + "0";
		fire_rate_label.Text = "Firerate - " + "0" + "/s";
		armor_modifier_label.Text = "AD - " + "0" + "%";
		crit_chance_label.Text = "Crit - " + "0" + "%";
	
		
	}

	private void _OnInvItemClicked(InventoryItem inv_item)
	{

		if(inv_item == attatched_item)
		{
			FreeHeldItem();
		}
	}

	private void _OnInvItemReleased(InventoryItem inv_item)
	{
		bool touching = false;
		Array<Area2D> overlapping_areas = area2D.GetOverlappingAreas();
		for(int i = 0; i < overlapping_areas.Count; i++)
		{
			if(overlapping_areas[i].GetParent() == inv_item)
			{
				touching = true;
			}
		}

		bool closest = false;
		Array<Area2D> inv_item_overlapping_areas = inv_item.area2D.GetOverlappingAreas();
		//finds all touching areas that are activeinventoryitemboxes and puts them in a list
		List<ActiveInventoryItemBox> touching_item_boxes = new List<ActiveInventoryItemBox>();
		for(int i = 0; i < inv_item_overlapping_areas.Count; i++)
		{
			if(inv_item_overlapping_areas[i].GetParent() is  ActiveInventoryItemBox box)
			{
				touching_item_boxes.Add(box);
			}
		}

		//if the closest activeinvitembox is this one closest is set to true
		ActiveInventoryItemBox closest_box;
		if(touching_item_boxes.Count > 1)
		{
			closest_box = touching_item_boxes[0];
			for(int i = 1; i < touching_item_boxes.Count; i++)
			{
				float closest_x = Mathf.Abs(closest_box.GlobalPosition.X - inv_item.GlobalPosition.X);
				float closest_y = Mathf.Abs(closest_box.GlobalPosition.Y - inv_item.GlobalPosition.Y);
				float closest_distance = Mathf.Sqrt(Mathf.Pow(closest_x,2) + Mathf.Pow(closest_y,2));

				float curr_x = Mathf.Abs(touching_item_boxes[i].GlobalPosition.X - inv_item.GlobalPosition.X);
				float curr_y = Mathf.Abs(touching_item_boxes[i].GlobalPosition.Y - inv_item.GlobalPosition.Y);
				float curr_distance = Mathf.Sqrt(Mathf.Pow(curr_x,2) + Mathf.Pow(curr_y,2));
				if(curr_distance < closest_distance)
				{
					closest_box = touching_item_boxes[i];
				}
			}
			if(closest_box == this)
			{
				closest = true;
			}
		}
		else if(touching_item_boxes.Count == 1)
		{
			closest = true;
		}

		if(touching && closest)
		{
			UpdateItem(inv_item);
		}
		

	}




	
}
