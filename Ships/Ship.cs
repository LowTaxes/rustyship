using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

public partial class Ship : CharacterBody2D
{
	
	public bool is_player;
	public int max_health = 0;
	public int health = 0;
	public int max_armor = 0;

	public int armor = 0;
	public int maneuverability = 0;
	
	public List<Hardpoint> available_hardpoints;
	public List<Hardpoint> hardpoints_in_use;
	public Vector2[] hardpoint_locations;
	public string[] hardpoint_weight_classes;


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

		List<Hardpoint> light = new List<Hardpoint>();
		List<Hardpoint> medium = new List<Hardpoint>();
		List<Hardpoint> heavy = new List<Hardpoint>();
		List<Hardpoint> superheavy = new List<Hardpoint>();
		
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
		//Finding how many hardpoints of each weigth class we have
		for(int i = 0; i < available_hardpoints.Count; i++)
		{
			if(available_hardpoints[i].weight_class.Equals("light"))
			{
				num_light_held+=1;
				light.Add(available_hardpoints[i]);
				
			}
			if(available_hardpoints[i].weight_class.Equals("medium"))
			{
				num_medium_held+=1;
				medium.Add(available_hardpoints[i]);
				
			}
			if(available_hardpoints[i].weight_class.Equals("heavy"))
			{
				num_heavy_held+=1;
				heavy.Add(available_hardpoints[i]);
			}
			if(available_hardpoints[i].weight_class.Equals("superheavy"))
			{
				num_superheavy_held+=1;
				superheavy.Add(available_hardpoints[i]);
				
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
					light.Add(new_hardpoint);
				}
			}

			for(int i = 0; i < light.Count; i++)
			{
				Hardpoint largest = light[i];
				int largest_index = i;
				for(int k = i+1; k < light.Count; k++)
				{
					if(light[k].level > largest.level)
					{
						largest = light[k];
						largest_index = k;
					}
				}
				Hardpoint temp = light[i];
				light[i] = largest;
				light[largest_index] = temp;
					
			}
			int light_inc = 0;

			for( int i = 0; i< hardpoint_weight_classes.Length; i++)
			{
				if(hardpoint_weight_classes[i].Equals("light"))
				{
					
					PackedScene weapon_scene = ResourceLoader.Load<PackedScene>(WeaponSceneUIDs.Instance.getWeaponUID(light[light_inc].attatched_weapon_name));
					Weapon new_weapon = weapon_scene.Instantiate<Weapon>();
					
					AddChild(new_weapon);
					new_weapon.Translate(hardpoint_locations[i]);
					
					if(is_player)
					{
						new_weapon.is_player = true;
					}
					new_weapon.Initialize(light[light_inc].attatched_weapon_name);
					
					
					light_inc +=1;
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
					medium.Add(new_hardpoint);
				}
			}

			for(int i = 0; i < medium.Count; i++)
			{
				Hardpoint largest = medium[i];
				int largest_index = i;
				for(int k = i+1; k < medium.Count; k++)
				{
					if(medium[k].level > largest.level)
					{
						largest = medium[k];
						largest_index = k;
					}
				}
				Hardpoint temp = medium[i];
				medium[i] = largest;
				medium[largest_index] = temp;
					
			}
			int medium_inc = 0;

			for( int i = 0; i< hardpoint_weight_classes.Length; i++)
			{
				if(hardpoint_weight_classes[i].Equals("medium"))
				{
						
					PackedScene weapon_scene = ResourceLoader.Load<PackedScene>(WeaponSceneUIDs.Instance.getWeaponUID(medium[medium_inc].attatched_weapon_name));
					Weapon new_weapon = weapon_scene.Instantiate<Weapon>();
					AddChild(new_weapon);
					new_weapon.Translate(hardpoint_locations[i]);
					if(is_player)
					{
						new_weapon.is_player = true;
					}
					new_weapon.Initialize(medium[medium_inc].attatched_weapon_name);
					medium_inc +=1;
				}
			}
		
		}
		
    }


	public void takeDamage(int damage)
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
				//Debug.Print("playertakedamage");
				BattleConnect.Instance.EmitSignal(BattleConnect.SignalName.PlayerHealthDamageTaken.ToString(), damage);
			}
			else
			{
				BattleConnect.Instance.EmitSignal(BattleConnect.SignalName.EnemyHealthDamageTaken.ToString(), damage);
			}
		}	
			
	}	
		
	public override void _Input(InputEvent @event)
    {
        if(@event is InputEventKey eventkey)
		{
			if(eventkey.IsReleased() && eventkey.Keycode == Key.Enter)
			{
				takeDamage(10);
				
			}
		}
    }


	
	
	
}
