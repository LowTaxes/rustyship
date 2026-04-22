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
	public int size_x = 1;
	public int size_y = 1;
	public Sprite2D sprite2D;
	public Area2D area2D;
	public bool mouse_hovering = false;
	public bool mouse_dragging = false;
	public List<InventorySquare> touching_squares;
	
	public override void _Ready()
	{

		sprite2D = GetChild<Sprite2D>(0);
		area2D = sprite2D.GetChild<Area2D>(0);

		touching_squares = new List<InventorySquare>();

        Array weapon_data = (Array)ConstantData.WeaponData[weapon_name];
		Dictionary inv_size = (Dictionary) weapon_data[(int)Constants.WeaponDataEnum.INVENTORY_ITEM_SIZE];

		size_x = (int)inv_size["x"];
		size_y = (int)inv_size["y"];
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

	private void _OnArea2DEntered(Area2D other_area2D)
	{
		if(other_area2D.GetParent().GetParent() is InventorySquare entered_inv_square)
		{
			touching_squares.Add(entered_inv_square);
			Debug.Print(touching_squares.Count.ToString());
		}
		
	}

	private void _OnArea2DExited(Area2D other_area2D)
	{
		if(other_area2D.GetParent().GetParent() is InventorySquare entered_inv_square)
		{
			touching_squares.Remove(entered_inv_square);
			Debug.Print(touching_squares.Count.ToString());
		}
		
	}


	
	
}
	
