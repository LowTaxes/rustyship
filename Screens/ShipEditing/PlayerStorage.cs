using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerStorage : GridContainer
{

	PackedScene inventory_square_scene;
	public int adjusted_inv_square_width;

	List<InventoryItem> held_items;
	
	public override void _Ready()
	{
		inventory_square_scene = GD.Load<PackedScene>("uid://b1wteem6372ip");
		
		this.Columns = Constants.player_storage_size_x;
		adjusted_inv_square_width = (int) (this.Size.X/Constants.player_storage_size_x);

		this.AddThemeConstantOverride("h_separation", (int)adjusted_inv_square_width);
		this.AddThemeConstantOverride("v_separation", (int)adjusted_inv_square_width);

		//Spawn all inventory squares
		for(int i = 0; i < Constants.player_storage_size_y; i ++)
		{
			for(int k = 0; k < Constants.player_storage_size_x; k ++)
			{
				InventorySquare new_square = inventory_square_scene.Instantiate<InventorySquare>();
				new_square.tile_x = k;
				new_square.tile_y = i;
				
				AddChild(new_square);
				float scale_x = adjusted_inv_square_width/(float)new_square.sprite2D.Texture.GetWidth();
				float scale_y = adjusted_inv_square_width/(float)new_square.sprite2D.Texture.GetHeight();
				new_square.sprite2D.Scale = new Vector2(scale_x, scale_y);
			}
		}

		held_items = new List<InventoryItem>();
		
	}

	public void AddItem(InventoryItem new_item)
	{
		
	}


}
