using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;

public partial class EnemyPreview : Control
{
	[Export] int index = 0;
	Control ship_preview_control;
	Label health_label;
	Label armor_label;
	Label health_M_label;
	Label armor_M_label;
	Label crit_M_label;
	VBoxContainer weapons_box;
	Label enemy_name_label;
	string new_level_id;
	

	public override void _Ready()
	{
		ship_preview_control = GetNode<Control>("ShipPreview");

		VBoxContainer statsbox = GetNode<Control>("Details").GetNode<VBoxContainer>("Stats");

		health_label = statsbox.GetNode<Label>("Health");
		armor_label = statsbox.GetNode<Label>("Armor");

		health_M_label = statsbox.GetNode<Label>("HealthModifier");
		armor_M_label = statsbox.GetNode<Label>("ArmorModifier");
		crit_M_label = statsbox.GetNode<Label>("CritModifier");

		weapons_box = GetNode<Control>("Details").GetNode<VBoxContainer>("Weapons");
		enemy_name_label = GetNode<Label>("EnemyName");
		
		int old_level_index = Convert.ToInt32(RunData.level_id[0].ToString());
		new_level_id = (old_level_index+1).ToString() + "-" + index.ToString();

		enemy_name_label.Text = ConstantData.GetLevelEnemyName(new_level_id);
		health_label.Text = "Health: " + ((ConstantData.GetShipTemplateHealth(ConstantData.GetLevelShipTemplateID(new_level_id))*(ConstantData.GetLevelHealthModifierCount(new_level_id)*Constants.health_modifier)) + ConstantData.GetShipTemplateHealth(ConstantData.GetLevelShipTemplateID(new_level_id))).ToString();
		armor_label.Text = "Armor: " + ((ConstantData.GetShipTemplateArmor(ConstantData.GetLevelShipTemplateID(new_level_id)) * (ConstantData.GetLevelArmorModifierCount(new_level_id)*Constants.armor_modifier)) + ConstantData.GetShipTemplateArmor(ConstantData.GetLevelShipTemplateID(new_level_id))).ToString();
		
		health_M_label.Text = "Health M: " + ConstantData.GetLevelHealthModifierCount(new_level_id).ToString();
		armor_M_label.Text = "Armor M: " + ConstantData.GetLevelArmorModifierCount(new_level_id).ToString();
		crit_M_label.Text = "Crit M: " + ConstantData.GetLevelCritModifierCount(new_level_id).ToString();

		List<Hardpoint> active_hardpoints = ConstantData.GetLevelEnemyActiveHarpoints(new_level_id);
		for(int i = 0 ; i < active_hardpoints.Count; i++)
		{
			if(!active_hardpoints[i].attatched_weaponID.Equals("empty"))
			{
				Label new_label = new Label();
				new_label.Text = active_hardpoints[i].attatched_weaponID + " / lvl " + active_hardpoints[i].level.ToString();
				weapons_box.AddChild(new_label);
			}
			
		}

		PackedScene ship_model_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetShipModelUID(ConstantData.GetLevelShipTemplateID(new_level_id)));
		ShipModel ship_model = ship_model_scene.Instantiate<ShipModel>();
		ship_model.Position += new Vector2(ship_preview_control.Size.X/2, ship_preview_control.Size.Y/2);

		float scale = ship_preview_control.Size.X / ship_model.Texture.GetWidth();
		ship_model.Scale = new Vector2(scale, scale);

		ship_preview_control.AddChild(ship_model);
	}

	private void _On_Selected()
	{
	
		RunData.level_id = new_level_id;
		
	}
		
	
}
