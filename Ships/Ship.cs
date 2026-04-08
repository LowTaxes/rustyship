using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;

using WeaponDataEnum = Constants.WeaponDataEnum;
public partial class Ship : CharacterBody2D
{
	
	public bool is_player;
	public double max_health = 0;
	public double health = 0;
	public double max_armor = 0;

	public double armor = 0;
	public double maneuverability = 0;
	public int other_ship_width;
	
	public List<Hardpoint> available_hardpoints;
	public List<Hardpoint> hardpoints_in_use;
	public Vector2[] hardpoint_locations;
	public string[] hardpoint_weight_classes;
	public Vector2 other_ship_start_point;
	public Vector2 ship_start_point;


    public override void _Ready()
    {
		
		PackedScene hardpoint_scene = ResourceLoader.Load<PackedScene>("uid://djegmfpqrdo3e");

		int max_guns = hardpoint_locations.Length;
		int num_light_needed = 0;
		int num_medium_needed = 0;
		int num_heavy_needed = 0;
		int num_superheavy_needed = 0;

		int num_light_held = 0;
		int num_medium_held = 0;
		int num_heavy_held = 0;
		int num_superheavy_held = 0;

		List<Hardpoint> light_hardpoints_available = new List<Hardpoint>();
		List<Hardpoint> medium_hardpoints_available = new List<Hardpoint>();
		List<Hardpoint> heavy_hardpoints_available = new List<Hardpoint>();
		List<Hardpoint> superheavy_hardpoints_available = new List<Hardpoint>();
		
		//finding how many hardpoints of each weight class is needed
		for(int i = 0; i< hardpoint_weight_classes.Length; i++)
		{
			if(hardpoint_weight_classes[i].Equals("light"))
			{
				num_light_needed+=1;
			}
			else if(hardpoint_weight_classes[i].Equals("medium"))
			{
				num_medium_needed+=1;
			}
			else if(hardpoint_weight_classes[i].Equals("heavy"))
			{
				num_heavy_needed+=1;
			}
			else if(hardpoint_weight_classes[i].Equals("superheavy"))
			{
				num_superheavy_needed+=1;
			}
		}
		//Finding how many hardpoints of each weight class we have
		for(int i = 0; i < available_hardpoints.Count; i++)
		{
			if(available_hardpoints[i].weight_class.Equals("light"))
			{
				num_light_held+=1;
				light_hardpoints_available.Add(available_hardpoints[i]);
				
			}
			if(available_hardpoints[i].weight_class.Equals("medium"))
			{
				num_medium_held+=1;
				medium_hardpoints_available.Add(available_hardpoints[i]);
				
			}
			if(available_hardpoints[i].weight_class.Equals("heavy"))
			{
				num_heavy_held+=1;
				heavy_hardpoints_available.Add(available_hardpoints[i]);
			}
			if(available_hardpoints[i].weight_class.Equals("superheavy"))
			{
				num_superheavy_held+=1;
				superheavy_hardpoints_available.Add(available_hardpoints[i]);
				
			}
		}

		//spawning and distributing light gun hardpoints
		if(num_light_needed >= 1)
		{
			if(num_light_held < num_light_needed)
			{
				for(int i = 0; i < num_light_needed-num_light_held; i++)
				{
					Hardpoint new_hardpoint = new Hardpoint(1, "light", "lightmachinegun");
					available_hardpoints.Add(new_hardpoint);
					light_hardpoints_available.Add(new_hardpoint);
				}
			}

			for(int i = 0; i < light_hardpoints_available.Count; i++)
			{
				Hardpoint largest = light_hardpoints_available[i];
				int largest_index = i;
				for(int k = i+1; k < light_hardpoints_available.Count; k++)
				{
					if(light_hardpoints_available[k].level > largest.level)
					{
						largest = light_hardpoints_available[k];
						largest_index = k;
					}
				}
				Hardpoint temp = light_hardpoints_available[i];
				light_hardpoints_available[i] = largest;
				light_hardpoints_available[largest_index] = temp;
					
			}
			int light_hardpoint_available_inc = 0;

			for( int i = 0; i< hardpoint_weight_classes.Length; i++)
			{
				if(hardpoint_weight_classes[i].Equals("light"))
				{
					
					
					PackedScene weapon_scene = ResourceLoader.Load<PackedScene>(((Array)(ConstantData.WeaponData[light_hardpoints_available[light_hardpoint_available_inc].attatched_weapon_name]))[(int)WeaponDataEnum.WEAPON_UID].ToString());
					Weapon new_weapon = weapon_scene.Instantiate<Weapon>();
					
					AddChild(new_weapon);
					new_weapon.Translate(hardpoint_locations[i]);
					
					if(is_player)
					{
						new_weapon.is_player = true;
					}
					new_weapon.weapon_name = light_hardpoints_available[light_hardpoint_available_inc].attatched_weapon_name;
					new_weapon.other_ship_width = other_ship_width;
					new_weapon.ship_start_point = ship_start_point;
					new_weapon.other_ship_start_point = other_ship_start_point;
					new_weapon.Initialize();
					
					
					light_hardpoint_available_inc +=1;
				}
			}
		
		}

		if(num_medium_needed >= 1)
		{
			if(num_medium_held < num_medium_needed)
			{
				for(int i = 0; i < num_medium_needed-num_medium_held; i++)
				{
					Hardpoint new_hardpoint = new Hardpoint(1, "medium", "mediumcannon");
					available_hardpoints.Add(new_hardpoint);
					medium_hardpoints_available.Add(new_hardpoint);
				}
			}

			for(int i = 0; i < medium_hardpoints_available.Count; i++)
			{
				Hardpoint largest = medium_hardpoints_available[i];
				int largest_index = i;
				for(int k = i+1; k < medium_hardpoints_available.Count; k++)
				{
					if(medium_hardpoints_available[k].level > largest.level)
					{
						largest = medium_hardpoints_available[k];
						largest_index = k;
					}
				}
				Hardpoint temp = medium_hardpoints_available[i];
				medium_hardpoints_available[i] = largest;
				medium_hardpoints_available[largest_index] = temp;
					
			}
			int medium_hardpoint_available_inc = 0;

			for( int i = 0; i< hardpoint_weight_classes.Length; i++)
			{
				if(hardpoint_weight_classes[i].Equals("medium"))
				{
						
					
					
					PackedScene weapon_scene = ResourceLoader.Load<PackedScene>(((Array)(ConstantData.WeaponData[medium_hardpoints_available[medium_hardpoint_available_inc].attatched_weapon_name]))[(int)WeaponDataEnum.WEAPON_UID].ToString());
					Weapon new_weapon = weapon_scene.Instantiate<Weapon>();
					
					AddChild(new_weapon);
					new_weapon.Translate(hardpoint_locations[i]);
					if(is_player)
					{
						new_weapon.is_player = true;
					}
					new_weapon.weapon_name = medium_hardpoints_available[medium_hardpoint_available_inc].attatched_weapon_name;
					new_weapon.other_ship_width = other_ship_width;
					new_weapon.ship_start_point = ship_start_point;
					new_weapon.other_ship_start_point = other_ship_start_point;
					new_weapon.Initialize();
					medium_hardpoint_available_inc +=1;
				}
			}
		
		}
		
    }


	public void takeDamage(double damage,  double armor_damage_modifier, double crit_chance)
	{
		if(armor > 0 )
		{
			
			armor -= damage;
			if(is_player)
			{
				BattleConnect.Instance.EmitSignal(BattleConnect.SignalName.PlayerArmorDamageTaken.ToString(), damage);
			}
			else
			{
				BattleConnect.Instance.EmitSignal(BattleConnect.SignalName.EnemyArmorDamageTaken.ToString(), damage);
			}
			
		}
		else
		{
			health -= damage;
			if(is_player)
			{
				
				BattleConnect.Instance.EmitSignal(BattleConnect.SignalName.PlayerHealthDamageTaken.ToString(), damage);
			}
			else
			{
				BattleConnect.Instance.EmitSignal(BattleConnect.SignalName.EnemyHealthDamageTaken.ToString(), damage);
			}
		}	
			
	}	
		
	
 

	
	
	
}
