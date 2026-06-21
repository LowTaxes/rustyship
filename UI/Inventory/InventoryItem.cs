using Godot;
using System;
using System.Collections.Generic;
using WeaponDataEnum = Constants.WeaponDataEnum;
using Array = Godot.Collections.Array;
using Godot.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;
public partial class InventoryItem : Control
{
	public string weapon_name = "autocannon";
	public int level = 0;
	public int size_x = 1;
	public int size_y = 1;
	public int storage_x = 0;
	public int storage_y = 0;
	public Sprite2D sprite2D;
	public Area2D area2D;
	public Node2D reference_point;
	public bool mouse_hovering = false;
	public bool mouse_dragging = false;
	public bool attatched = false;
	public bool is_lootspawn = false;
	public float sprite_scale_x;
	public float sprite_scale_y;

	
	
	
	public override void _Ready()
	{

		sprite2D = GetChild<Sprite2D>(0);
		area2D = GetChild<Area2D>(1);
		reference_point = area2D.GetChild<Node2D>(1);

		area2D.MouseEntered += _On_Mouse_Entered;
		area2D.MouseExited += _On_Mouse_Exited;
		
		

		
		if(!(weapon_name.Equals("empty")))
		{
			if(ConstantData.GetBattleItemDesignation(weapon_name).Equals(Constants.WEAPON_DESIGNATION))
			{
				Vector2 size_v = ConstantData.GetWeaponInventoryItemSize(weapon_name);
				size_x = (int)size_v.X;
				size_y = (int)size_v.Y;
			}
			else if(ConstantData.GetBattleItemDesignation(weapon_name).Equals(Constants.DEFENSIVE_DESIGNATION))
			{
				Vector2 size_v = ConstantData.GetDefensiveInventoryItemSize(weapon_name);
				size_x = (int)size_v.X;
				size_y = (int)size_v.Y;
			}
			
			

		}
        
	}
    public override void _Process(double delta)
    {
        if(this.GlobalPosition.Y < Constants.SEPERATOR_Y && !is_lootspawn)
		{
			Debug.Print("left storage area");
			PackedScene new_hardpoint_scene = ResourceLoader.Load<PackedScene>("uid://1h4nrs17ravr");
			Hardpoint new_hardpoint = new_hardpoint_scene.Instantiate<Hardpoint>();
			GetNode("/root").AddChild(new_hardpoint);
			new_hardpoint.attatched_weaponID = weapon_name;
			new_hardpoint.level = level;
			new_hardpoint.moveable = true;
			new_hardpoint.mouse_hovering = true;
			new_hardpoint.mouse_dragging = true;
			new_hardpoint.Initialize(weapon_name);
			new_hardpoint.Position = GetGlobalMousePosition();
			GarbageCollector.all_hardpoints.Add(new_hardpoint);
			GarbageCollector.all_inv_items.Remove(this);
			this.QueueFree();
			
		}
    }


	private void _On_Mouse_Entered()
	{
		mouse_hovering = true;
		//Debug.Print("hi");
		//Debug.Print(mouse_hovering.ToString());
	}

	private void _On_Mouse_Exited()
	{
		mouse_hovering = false;
		mouse_dragging = false;
		//Debug.Print(mouse_hovering.ToString());
	}

    public override void _Input(InputEvent @event)
    {
		 if(@event is InputEventMouse mouse_event && !(weapon_name.Equals("empty")))
		{
			if(mouse_hovering && mouse_event.IsActionPressed("left_click"))
			{
				if(is_lootspawn)
				{
					is_lootspawn = false;
					SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.LootTaken);
				}
		
				this.Reparent(GetNode("/root"), true);
				mouse_dragging = true;
				SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.InvItemClicked.ToString(), this);
			}
			else if(mouse_hovering && mouse_event.IsActionReleased("left_click"))
			{
				mouse_dragging = false;
				SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.InvItemReleased.ToString(), this);
			}
			if(mouse_event is InputEventMouseMotion mouse_motion && mouse_dragging)
			{
				Position = Position + mouse_motion.Relative *Constants.camera_zoom_dragging_modifier;
			}
		}
       
		
    }

	

	


	
	
}
	
