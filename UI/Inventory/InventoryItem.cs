using Godot;
using System;
using System.Collections.Generic;
using WeaponDataEnum = Constants.WeaponDataEnum;
using Array = Godot.Collections.Array;
using Godot.Collections;
using System.Diagnostics;
public partial class InventoryItem : Control
{
	public string weapon_name = "lightmachinegun";
	public Sprite2D sprite2D;
	public Area2D area2D;
	public bool mouse_hovering = false;
	public bool mouse_dragging = false;
	
	public override void _Ready()
	{
		sprite2D = GetChild<Sprite2D>(0);
		area2D = sprite2D.GetChild<Area2D>(0);

        Array weapon_data = (Array)ConstantData.WeaponData[weapon_name];
		Dictionary inv_size = (Dictionary) weapon_data[(int)Constants.WeaponDataEnum.INVENTORY_ITEM_SIZE];

		float scale_x = Constants.inv_square_pixel_width/(float)sprite2D.Texture.GetWidth() * (int)inv_size["x"];
		float scale_y = Constants.inv_square_pixel_width/(float)sprite2D.Texture.GetHeight() * (int)inv_size["y"];
		sprite2D.Scale = new Vector2(sprite2D.Scale.X * scale_x, sprite2D.Scale.Y * scale_y);
		Debug.Print(mouse_hovering.ToString());
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
		 if(@event is InputEventMouse)
		{
			if(mouse_hovering && @event.IsActionPressed("left_click"))
			{
				mouse_dragging = true;
				Debug.Print(@event.ToString());
			}
			else if(mouse_hovering && @event.IsActionReleased("left_click"))
			{
				mouse_dragging = false;
				Debug.Print(@event.ToString());
			}
			if(@event is InputEventMouseMotion mouse_motion && mouse_dragging)
			{
				Position = Position + mouse_motion.Relative;
			}
		}
       
		
    }


	
	
}
	
