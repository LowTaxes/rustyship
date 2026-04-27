using Godot;
using System;
using System.Diagnostics;
using Godot.Collections;
using Array = Godot.Collections.Array;
public partial class InventorySquare : Control
{
	public int tile_x;
	public int tile_y;
	public Sprite2D sprite2D;
	public Area2D area2d;
	public Node2D reference_point;
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
        Array<Area2D> overlapping_areas = area2d.GetOverlappingAreas();
		Texture2D current_texture = unselected_inv_square;
		
		
		
		for(int i = 0; i < overlapping_areas.Count; i++)
		{
			if(overlapping_areas[i].GetParent() is InventoryItem inv_item && inv_item.attatched == false)
			{
				current_texture = selected_inv_square;
			}
		}
		
		if(occupied)
		{
			current_texture = unselected_inv_square;
		}
		sprite2D.Texture = current_texture;
		
    }


	
}
