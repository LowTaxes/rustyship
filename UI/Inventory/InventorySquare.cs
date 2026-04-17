using Godot;
using System;
using System.Diagnostics;

public partial class InventorySquare : Control
{
	public int tile_x;
	public int tile_y;
	public Sprite2D sprite2D;
	public Area2D area2d;
	
	public override void _Ready()
	{
		
		sprite2D = GetChild<Sprite2D>(0);
		area2d = sprite2D.GetChild<Area2D>(0);

		float scale_x = Constants.inv_square_pixel_width/(float)sprite2D.Texture.GetWidth();
		float scale_y = Constants.inv_square_pixel_width/(float)sprite2D.Texture.GetHeight();
		sprite2D.Scale = new Vector2(sprite2D.Scale.X * scale_x, sprite2D.Scale.Y * scale_y);
		
		//Debug.Print(Texture.GetWidth().ToString());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
