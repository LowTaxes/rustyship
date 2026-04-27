using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;
public partial class PlayerStorage : Control
{

	PackedScene inventory_square_scene;
	public int adjusted_inv_square_width;
	List<InventoryItem> held_items;
	public PackedScene inventory_item_scene;
	List<InventorySquare> grid_squares;
	public GridContainer grid_container;
	public List<List<InventorySquare>> rowed_grid_squares; 

	
	public override void _Ready()
	{
		inventory_square_scene = GD.Load<PackedScene>("uid://b1wteem6372ip");
		inventory_item_scene = GD.Load<PackedScene>("uid://cedh3uq18etis");
		grid_container = GetChild<GridContainer>(0);
		grid_container.Columns = Constants.player_storage_size_x;
		adjusted_inv_square_width = (int) (grid_container.Size.X/Constants.player_storage_size_x);

		grid_container.AddThemeConstantOverride("h_separation", (int)adjusted_inv_square_width);
		grid_container.AddThemeConstantOverride("v_separation", (int)adjusted_inv_square_width);
		
		grid_squares = new List<InventorySquare>();
		rowed_grid_squares = new List<List<InventorySquare>>();
		//Spawn all inventory squares
		for(int i = 0; i < Constants.player_storage_size_y; i ++)
		{
			List<InventorySquare> new_row = new List<InventorySquare>();
			for(int k = 0; k < Constants.player_storage_size_x; k ++)
			{
				InventorySquare new_square = inventory_square_scene.Instantiate<InventorySquare>();
				new_square.tile_x = k;
				new_square.tile_y = i;
				new_square.attatched_container = grid_container;
				
				grid_container.AddChild(new_square);
				float scale_x = adjusted_inv_square_width/(float)new_square.sprite2D.Texture.GetWidth();
				float scale_y = adjusted_inv_square_width/(float)new_square.sprite2D.Texture.GetHeight();
				new_square.sprite2D.Scale = new Vector2(scale_x, scale_y);
				new_square.area2d.Scale = new Vector2(scale_x, scale_y);
				grid_squares.Add(new_square);
				new_row.Add(new_square);

			}

			rowed_grid_squares.Add(new_row);
		}

		held_items = new List<InventoryItem>();
		
		Array run_data_weapon_dicts = (Array) ((Array)RunData.Instance.LoadUserData()["player"])[(int)Constants.RunDataEnum.STORAGE_INVENTORY]; 

		for(int i = 0; i < run_data_weapon_dicts.Count; i++)
		{
			InventoryItem new_item = inventory_item_scene.Instantiate<InventoryItem>();
			new_item.weapon_name = ((Dictionary)(run_data_weapon_dicts[i]))["weaponID"].ToString();
			
			AddChild(new_item);
			
			new_item.sprite_scale_x = (float)adjusted_inv_square_width / new_item.sprite2D.Texture.GetWidth() * new_item.size_x;
			new_item.sprite_scale_y = (float)adjusted_inv_square_width / new_item.sprite2D.Texture.GetHeight() * new_item.size_y;
			new_item.sprite2D.Scale = new Vector2(new_item.sprite_scale_x, new_item.sprite_scale_y);
		
			float area_scale_x = (float)adjusted_inv_square_width / new_item.area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.X * new_item.size_x;
			float area_scale_y = (float)adjusted_inv_square_width / new_item.area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.Y * new_item.size_y;
			new_item.area2D.GetChild<CollisionShape2D>(0).Scale = new Vector2(area_scale_x, area_scale_y);

			
			float pos_x = ((int)((Dictionary)(run_data_weapon_dicts[i]))["x"])*adjusted_inv_square_width + (new_item.sprite2D.Texture.GetWidth()*new_item.sprite_scale_x/2) - (adjusted_inv_square_width/2);
			float pos_y = ((int)((Dictionary)(run_data_weapon_dicts[i]))["y"])*adjusted_inv_square_width + (new_item.sprite2D.Texture.GetHeight()*new_item.sprite_scale_y/2) - (adjusted_inv_square_width/2);

			new_item.Position = new Vector2(0,0);
			new_item.Position += new Vector2(pos_x, pos_y);
			
		}


	}

	public void AddItem(InventoryItem new_item)
	{
		//sort through and make the first new_item.size_x * new_item.size_y closest squares first in the list
		for(int i = 0; i < grid_squares.Count; i++)
		{
			InventorySquare closest = grid_squares[i];
			for (int k = i + 1; k < grid_squares.Count; k++)
			{
				float closest_x = Mathf.Abs(new_item.Position.X - closest.Position.X);
				float closest_y = Mathf.Abs(new_item.Position.Y - closest.Position.Y);
				float closest_distance = Mathf.Sqrt(Mathf.Pow(closest_x,2) + Mathf.Pow(closest_y,2));

				float current_x = Mathf.Abs(new_item.Position.X - grid_squares[k].Position.X);
				float current_y = Mathf.Abs(new_item.Position.Y - grid_squares[k].Position.Y);
				float current_distance = Mathf.Sqrt(Mathf.Pow(current_x,2) + Mathf.Pow(current_y,2));

				if(current_distance < closest_distance)
				{
					

					grid_squares[i] = grid_squares[k];
					grid_squares[k] = closest;
					closest = grid_squares[i];

					
				}
			}
		}
		//sort through the first new_item.size_x * new_item.size_y organized by lowest x pos
		for(int i = 0; i < new_item.size_x * new_item.size_y; i ++)
		{
			InventorySquare leftmost = grid_squares[i];

			for (int k = i+1; k < new_item.size_x * new_item.size_y; k++)
			{
				if (grid_squares[k].Position.X < leftmost.Position.X)
				{
					grid_squares[i] = grid_squares[k];
					grid_squares[k] = leftmost;
					leftmost = grid_squares[i];
				}
			}
		}





		bool placeable = true;

	
		for (int i = 0; i < new_item.size_y; i++)
		{
			for (int j = 0; j < new_item.size_x; j++)
			{
				if( j + grid_squares[i].tile_x >= rowed_grid_squares[i].Count)
				{
					placeable = false;
				}
				else if(rowed_grid_squares[grid_squares[i].tile_y][grid_squares[i].tile_x+j].occupied == true)
				{
					placeable = false;
				}
			}
			
		}
		
		Debug.Print(placeable.ToString());
		if(placeable)
		{
			if(new_item.GetParent<PlayerStorage>() != this)
			{
				AddChild(new_item);
			}
		
		
		
			
			
			//make the first new_item.size_x * new_item.size_y closest squares ordered by closest to 0,0
			for(int i = 0; i < new_item.size_x * new_item.size_y; i++)
			{
				InventorySquare closest = grid_squares[i];
				for (int k = i + 1; k < new_item.size_x * new_item.size_y; k++)
				{
					float closest_x = Mathf.Abs(closest.Position.X);
					float closest_y = Mathf.Abs(closest.Position.Y);
					float closest_distance = Mathf.Sqrt(Mathf.Pow(closest_x,2) + Mathf.Pow(closest_y,2));

					float current_x = Mathf.Abs(grid_squares[k].Position.X);
					float current_y = Mathf.Abs(grid_squares[k].Position.Y);
					float current_distance = Mathf.Sqrt(Mathf.Pow(current_x,2) + Mathf.Pow(current_y,2));

					if(current_distance < closest_distance)
					{
						grid_squares[i] = grid_squares[k];
						grid_squares[k] = closest;
						closest = grid_squares[i];
					}
				}
			}
			for(int i = 0; i < new_item.size_x * new_item.size_y; i++)
			{
				//rid_squares[i].occupied = true;
				
			}
			
			float pos_x = grid_squares[0].Position.X + (new_item.sprite2D.Texture.GetWidth()/2 * new_item.sprite_scale_x) - (adjusted_inv_square_width/2);
			float pos_y = grid_squares[0].Position.Y + (new_item.sprite2D.Texture.GetHeight()/2 * new_item.sprite_scale_y) - (adjusted_inv_square_width/2);
			new_item.Position = new Vector2(pos_x, pos_y);
		}
		
	}


}
