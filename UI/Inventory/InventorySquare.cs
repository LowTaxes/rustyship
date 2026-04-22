using Godot;
using System;
using System.Diagnostics;

public partial class InventorySquare : Control
{
	public int tile_x;
	public int tile_y;
	public Sprite2D sprite2D;
	public Area2D area2d;
	public Texture2D unselected_inv_square;
	public Texture2D selected_inv_square;
	public Inventory attatched_inventory;
	public bool selectable = false;
	
	public override void _Ready()
	{
		unselected_inv_square = ResourceLoader.Load<Texture2D>("uid://drs654f1iosgt");
		selected_inv_square = ResourceLoader.Load<Texture2D>("uid://dscku0yh80jds");

		sprite2D = GetChild<Sprite2D>(0);
		area2d = sprite2D.GetChild<Area2D>(0);

		sprite2D.Texture = unselected_inv_square;
		float scale_x = Constants.inv_square_pixel_width/(float)sprite2D.Texture.GetWidth();
		float scale_y = Constants.inv_square_pixel_width/(float)sprite2D.Texture.GetHeight();
		sprite2D.Scale = new Vector2(sprite2D.Scale.X * scale_x, sprite2D.Scale.Y * scale_y);
		
		//Debug.Print(Texture.GetWidth().ToString());
	}

    public override void _Process(double delta)
    {
        if(attatched_inventory.GetGlobalRect().HasPoint(this.GlobalPosition))
		{
			selectable = true;
		}
		else
		{
			selectable = false;
		}
    }


	private void _OnArea2DEntered(Area2D other_area2D)
	{
		//Debug.Print(other_area2D.GetParent().GetParent().Name);
		if(selectable && other_area2D.GetParent().GetParent() is InventoryItem inv_item)
		{
			sprite2D.Texture = selected_inv_square;
		}
	}
	private void _OnArea2DExited(Area2D other_area2D)
	{
		//Debug.Print(other_area2D.GetParent().GetParent().Name);
		if(other_area2D.GetParent().GetParent() is InventoryItem inv_item)
		{
			sprite2D.Texture = unselected_inv_square;
		}
	}
}
