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
	public string weapon_name = "lightmachinegun";
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
	public float sprite_scale_x;
	public float sprite_scale_y;
	
	
	public override void _Ready()
	{

		sprite2D = GetChild<Sprite2D>(0);
		area2D = GetChild<Area2D>(1);
		reference_point = area2D.GetChild<Node2D>(1);

		
		if(!(weapon_name.Equals("empty")))
		{
			Array weapon_data = (Array)ConstantData.WeaponData[weapon_name];
			Dictionary inv_size = (Dictionary) weapon_data[(int)Constants.WeaponDataEnum.INVENTORY_ITEM_SIZE];
			size_x = (int) inv_size["x"];
			size_y = (int) inv_size["y"];
			sprite2D.Texture = GD.Load<Texture2D>(weapon_data[(int)Constants.WeaponDataEnum.INVENTORY_ITEM_SPRITE_UID].ToString());

		}
        
	}
    public override void _Process(double delta)
    {
        
    }


	private void _On_Mouse_Entered()
	{
		mouse_hovering = true;
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
				Position = Position + mouse_motion.Relative;
			}
		}
       
		
    }

	


	
	
}
	
