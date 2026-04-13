using Godot;
using System;

public partial class InventorySquare : Sprite2D
{
	public int tile_x;
	public int tile_y;
	Area2D area2d;
	
	public override void _Ready()
	{
		area2d = GetChild<Area2D>(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
