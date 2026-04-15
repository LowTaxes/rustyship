using Godot;
using System;

public partial class Inventory : Control
{
	public int width_x = 5;
	public int width_y = 3;

	PackedScene inventory_square_scene;

	public override void _Ready()
	{
		inventory_square_scene = ResourceLoader.Load<PackedScene>("uid://b8wpkoljgdqgh");
		SpawnInventory();
	}

	public void SpawnInventory()
	{
		for(int i = 0; i < width_x; i ++)
		{
			for (int k = 0; k < width_y; k++)
			{
				InventorySquare new_square = inventory_square_scene.Instantiate<InventorySquare>();
				new_square.tile_x = i;
				new_square.tile_y = k;
				float scale_x = Constants.inv_square_pixel_width/new_square.Texture.GetWidth();
				float scale_y = Constants.inv_square_pixel_width/new_square.Texture.GetHeight();
				new_square.Scale = new Vector2(new_square.Scale.X * scale_x, new_square.Scale.Y*scale_y);
				AddChild(new_square);
				new_square.Translate(new Vector2(Constants.inv_square_pixel_width*i, Constants.inv_square_pixel_width*k));
			}
		}
	}
}
