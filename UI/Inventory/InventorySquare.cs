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
	public Vector2 relative_position;
	public GridContainer attatched_container;
	public bool occupied = false;

	public double distance_to_inv_item = 0;
	
	public override void _Ready()
	{
		unselected_inv_square = ResourceLoader.Load<Texture2D>("uid://drs654f1iosgt");
		selected_inv_square = ResourceLoader.Load<Texture2D>("uid://dscku0yh80jds");

		sprite2D = GetChild<Sprite2D>(0);
		area2d = GetChild<Area2D>(1);

		sprite2D.Texture = unselected_inv_square;
		
	} 

    public override void _Process(double delta)
    {
        
    }


	private void _OnArea2DEntered(Area2D other_area2D)
	{
		//Debug.Print(other_area2D.GetParent().GetParent().Name);
		if(other_area2D.GetParent() is InventoryItem inv_item)
		{
			sprite2D.Texture = selected_inv_square;
		}
	}
	private void _OnArea2DExited(Area2D other_area2D)
	{
		//Debug.Print(other_area2D.GetParent().GetParent().Name);
		if(other_area2D.GetParent() is InventoryItem inv_item)
		{
			sprite2D.Texture = unselected_inv_square;
		}
	}
}
