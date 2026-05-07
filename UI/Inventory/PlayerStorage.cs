using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;
using Godot.Collections;
public partial class PlayerStorage : Control
{

	PackedScene inventory_square_scene;
	List<InventoryItem> held_items;
	public PackedScene inventory_item_scene;
	List<InventorySquare> grid_squares;
	public GridContainer grid_container;
	public List<List<InventorySquare>> rowed_grid_squares; 

	Area2D area2D;

	
	public override void _Ready()
	{
		SignalConnect.Instance.Connect(SignalConnect.SignalName.InvItemClicked, new Callable(this, "_OnInvItemClicked"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.InvItemReleased, new Callable(this, "_OnInvItemReleased"));

		SignalConnect.Instance.Connect(SignalConnect.SignalName.ChangeToNextScene, new Callable(this, "_ChangeToNextScene"));

		inventory_square_scene = GD.Load<PackedScene>("uid://b1wteem6372ip");
		inventory_item_scene = GD.Load<PackedScene>("uid://cedh3uq18etis");
		grid_container = GetChild<GridContainer>(0);
		grid_container.Columns = Constants.player_storage_size_x;
		area2D = GetChild<Area2D>(1);

		grid_container.AddThemeConstantOverride("h_separation", (int)Constants.inventory_square_size);
		grid_container.AddThemeConstantOverride("v_separation", (int)Constants.inventory_square_size);
		
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
				float scale_x = Constants.inventory_square_size/(float)new_square.sprite2D.Texture.GetWidth();
				float scale_y = Constants.inventory_square_size/(float)new_square.sprite2D.Texture.GetHeight();
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
			new_item.level = (int)((Dictionary)(run_data_weapon_dicts[i]))["level"];
			new_item.storage_x = (int)((Dictionary)run_data_weapon_dicts[i])["x"];
			new_item.storage_y = (int)((Dictionary)(run_data_weapon_dicts[i]))["y"];

			AddChild(new_item);
			held_items.Add(new_item);
	
			new_item.sprite_scale_x = (float)Constants.inventory_square_size / new_item.sprite2D.Texture.GetWidth() * new_item.size_x;
			new_item.sprite_scale_y = (float)Constants.inventory_square_size / new_item.sprite2D.Texture.GetHeight() * new_item.size_y;
			new_item.sprite2D.Scale = new Vector2(new_item.sprite_scale_x, new_item.sprite_scale_y);
			new_item.reference_point.Position = new Vector2(-new_item.sprite2D.Texture.GetWidth() * new_item.sprite_scale_x/2, -new_item.sprite2D.Texture.GetHeight()*new_item.sprite_scale_y/2);
			//new_item.reference_point.Position = new Vector2(0,0);
			
			float area_scale_x = (float)Constants.inventory_square_size / new_item.area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.X * new_item.size_x;
			float area_scale_y = (float)Constants.inventory_square_size / new_item.area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.Y * new_item.size_y;
			new_item.area2D.Scale = new Vector2(area_scale_x, area_scale_y);
			//Debug.Print(new_item.area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.X.ToString());

			float pos_x = (new_item.storage_x)*Constants.inventory_square_size + (new_item.sprite2D.Texture.GetWidth()*new_item.sprite_scale_x/2) - (Constants.inventory_square_size/2);
			float pos_y = (new_item.storage_y)*Constants.inventory_square_size + (new_item.sprite2D.Texture.GetHeight()*new_item.sprite_scale_y/2) - (Constants.inventory_square_size/2);

			Label level_label = new_item.GetChild<Label>(2);
			level_label.Text = new_item.level.ToString();
			level_label.Position = new Vector2(-Constants.inventory_square_size*new_item.size_x/2,-Constants.inventory_square_size*new_item.size_y/2);
			level_label.AddThemeConstantOverride("font_size", 30);
			//level_label.Scale = new Vector2(new_item.sprite_scale_x, new_item.sprite_scale_y);

			

			new_item.Position = new Vector2(0,0);
			new_item.Position += new Vector2(pos_x, pos_y);
			new_item.attatched = true;
			
		}


	}

	public void AddItem(InventoryItem new_item)
	{

		List<InventorySquare> touching_squares = new List<InventorySquare>();
		bool placeable = true;
		//Debug.Print(new_item.area2D.GetOverlappingAreas().Count.ToString());

		//Debug.Print(rowed_grid_squares[0][0].area2d.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.X.ToString());
		for(int i = 0; i < new_item.area2D.GetOverlappingAreas().Count; i++)
		{
			if(new_item.area2D.GetOverlappingAreas()[i].GetParent() is InventorySquare inv_square)
			{
				touching_squares.Add(inv_square);
			}
		}
		//Find closest_square
		InventorySquare closest_square = touching_squares[0];
		for(int i = 0; i < touching_squares.Count; i++)
		{
			float closest_square_x = Mathf.Abs(closest_square.Position.X - new_item.reference_point.Position.X);
			float closest_square_y = Mathf.Abs(closest_square.Position.Y - new_item.reference_point.Position.Y);
			float closest_distance = Mathf.Sqrt(Mathf.Pow(closest_square_x,2) + Mathf.Pow(closest_square_y,2));

			float current_square_x = Mathf.Abs(touching_squares[i].Position.X - new_item.reference_point.Position.X);
			float current_square_y = Mathf.Abs(touching_squares[i].Position.Y - new_item.reference_point.Position.Y);
			float current_distance = Mathf.Sqrt(Mathf.Pow(current_square_x,2) + Mathf.Pow(current_square_y,2));

			if(current_distance < closest_distance)
			{
				closest_square = touching_squares[i];
			}
		}
		
		//
		//check if any of the squares to be placed in are either occupied or do not exist
		for(int i = 0; i < new_item.size_x; i++)
		{
			
			for(int k = 0; k < new_item.size_y; k++)
			{
				
				if(closest_square.tile_x + i >= Constants.player_storage_size_x ||
				closest_square.tile_y + k >= Constants.player_storage_size_y ||
				rowed_grid_squares[closest_square.tile_y + k][closest_square.tile_x + i].occupied)
				{
					placeable = false;

				}
			}
		}

		if(placeable)
		{
			if(new_item.GetParent() != this)
			{
				new_item.Reparent(this);
			}
			held_items.Add(new_item);
			new_item.attatched = true;
			new_item.storage_x = closest_square.tile_x;
			new_item.storage_y = closest_square.tile_y;

			for(int i = 0; i < new_item.size_x; i++)
			{
				for(int k = 0; k < new_item.size_y; k++)
				{
					rowed_grid_squares[closest_square.tile_y + k][closest_square.tile_x + i].occupied = true;
				}

			}

			float pos_x = closest_square.Position.X + (new_item.sprite2D.Texture.GetWidth()/2 * new_item.sprite_scale_x) - (Constants.inventory_square_size/2);
			float pos_y = closest_square.Position.Y + (new_item.sprite2D.Texture.GetHeight()/2 * new_item.sprite_scale_y) - (Constants.inventory_square_size/2);
			new_item.Position = new Vector2(pos_x, pos_y);
		}
		

	}

	private void _OnInvItemClicked(InventoryItem inv_item)
	{
		for(int i = 0; i < held_items.Count; i++)
		{
			if(held_items[i] == inv_item)
			{
				FreeHeldItem(inv_item);
			}
		}
	}

	private void _OnInvItemReleased(InventoryItem inv_item)
	{
		Array<Area2D> overlapping_areas = area2D.GetOverlappingAreas();

		for(int i = 0; i<overlapping_areas.Count; i++)
		{
			if(overlapping_areas[i].GetParent() is InventoryItem overlapping_inv_item)
			{
				if(overlapping_inv_item == inv_item)
				{
					AddItem(inv_item);
				}
			}
		}
	}

	public void FreeHeldItem(InventoryItem inv_item)
	{
		held_items.Remove(inv_item);
		inv_item.attatched = false;
		inv_item.Reparent(GetNode("/root"));

		for(int i = 0; i < inv_item.size_x; i++)
		{
			for(int k = 0; k < inv_item.size_y; k++)
			{
				rowed_grid_squares[inv_item.storage_y + k][inv_item.storage_x + i].occupied = false;
			}

		}
	}

	private void _ChangeToNextScene()
	{
		Array array_attatched_items = new Array();
		for(int i = 0; i < held_items.Count; i++)
		{
			Dictionary curr_item = new Dictionary();
			curr_item.Add("weaponID", held_items[i].weapon_name);
			curr_item.Add("level", held_items[i].level);
			curr_item.Add("x", held_items[i].storage_x);
			curr_item.Add("y", held_items[i].storage_y);
			array_attatched_items.Add(curr_item);
		}
		RunData.Instance.p_storage_inv = array_attatched_items;
	}


}
