using Godot;
using System;

public partial class Inventory : Control
{
	int width_x = 5;
	int width_y = 3;
	float tile_size = 40;

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
				float scale_x = tile_size/new_square.Texture.GetWidth();
				float scale_y = tile_size/new_square.Texture.GetHeight();
				new_square.Scale = new Vector2(new_square.Scale.X * scale_x, new_square.Scale.Y*scale_y);
				AddChild(new_square);
				new_square.Translate(new Vector2(tile_size*i, tile_size*k));
			}
		}
	}
}
