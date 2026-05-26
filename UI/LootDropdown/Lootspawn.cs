using Godot;
using System;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;
public partial class Lootspawn : Control
{
	//TEMPORARY
	public override void _Ready()
	{
		PackedScene temp_spawn = ResourceLoader.Load<PackedScene>("uid://cedh3uq18etis");
		InventoryItem new_item = temp_spawn.Instantiate<InventoryItem>();
		new_item.weapon_name = "autocannon";
		new_item.level = 1;
		new_item.is_lootspawn = true;
		
		this.AddChild(new_item);
		

		new_item.sprite_scale_x = ((float)Constants.inventory_square_size-Constants.pixel_size) / new_item.sprite2D.Texture.GetWidth() * new_item.size_x;
		new_item.sprite_scale_y = ((float)Constants.inventory_square_size-Constants.pixel_size) / new_item.sprite2D.Texture.GetHeight() * new_item.size_y;
		new_item.sprite2D.Scale = new Vector2(new_item.sprite_scale_x, new_item.sprite_scale_y);
		new_item.reference_point.Position = new Vector2(-new_item.sprite2D.Texture.GetWidth() * new_item.sprite_scale_x/2, -new_item.sprite2D.Texture.GetHeight()*new_item.sprite_scale_y/2);
		//new_item.reference_point.Position = new Vector2(0,0);
		
		float area_scale_x = ((float)Constants.inventory_square_size-Constants.pixel_size) / new_item.area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.X * new_item.size_x;
		float area_scale_y = ((float)Constants.inventory_square_size-Constants.pixel_size) / new_item.area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.Y * new_item.size_y;
		new_item.area2D.Scale = new Vector2(area_scale_x, area_scale_y);

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
