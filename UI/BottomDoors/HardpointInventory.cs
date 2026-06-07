using Godot;
using System;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;
using Godot.Collections;
using System.Collections.Generic;
using System.Diagnostics;
public partial class HardpointInventory : Control
{
	PackedScene inventory_square_scene;
	InventoryItem held_item;
	Hardpoint cur_hardpoint = null;
	List<InventorySquare> grid_squares;
	public GridContainer grid_container;
	public List<List<InventorySquare>> rowed_grid_squares; 
	Area2D area2D;

	public string attatched_weapon_name = "empty";
	public int size_x;
	public int size_y;
	public Texture2D light_container;
	public Texture2D medium_container;
	public Texture2D heavy_container;

	public PackedScene cur_background_container_scene;

	Sprite2D background_container_sprite;
	public override void _Ready()
	{
	
		SignalConnect.Instance.Connect(SignalConnect.SignalName.InvItemClicked, new Callable(this, "_OnInvItemClicked"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.InvItemReleased, new Callable(this, "_OnInvItemReleased"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointInfoChange, new Callable(this, "_OnHardpointInfoChange"));
		
		light_container = ResourceLoader.Load<Texture2D>("uid://g3h4i7w70c6a");
		medium_container = ResourceLoader.Load<Texture2D>("uid://bejm522e8o7is");
		heavy_container = ResourceLoader.Load<Texture2D>("uid://hdc6a8x6vmi");
		


		background_container_sprite = GetChild<Sprite2D>(0);
		//background_container_sprite.Texture = light_container;
		size_x = 5;
		size_y = 3;



		inventory_square_scene = GD.Load<PackedScene>("uid://b1wteem6372ip");
		
		grid_container = GetChild<GridContainer>(1);
		grid_container.Columns = size_x;
		area2D = GetChild<Area2D>(2);

		grid_squares = new List<InventorySquare>();
		rowed_grid_squares = new List<List<InventorySquare>>();

		grid_container.AddThemeConstantOverride("h_separation", (int)Constants.inventory_square_size);
		grid_container.AddThemeConstantOverride("v_separation", (int)Constants.inventory_square_size);

		

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
		//Debug.Print(IsInstanceValid(held_item).ToString());
		if(placeable && held_item==null && cur_hardpoint.weight_class.Equals(ConstantData.GetWeaponWeightClass(new_item.weapon_name)))
		{
			if(new_item.GetParent() != this)
			{
				new_item.Reparent(this);
			}
			
			held_item = new_item;
			
			if(cur_hardpoint != null)
			{
				cur_hardpoint.attatched_weaponID = new_item.weapon_name;
				cur_hardpoint.inv_x = closest_square.tile_x;
				cur_hardpoint.inv_y = closest_square.tile_y;
				cur_hardpoint.level = new_item.level;
				cur_hardpoint.SetWeaponModelSprite(new_item.weapon_name);
				
			}
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

			float pos_x = grid_container.Position.X + (new_item.storage_x)*(Constants.inventory_square_size) + (new_item.sprite2D.Texture.GetWidth()/2) - Constants.inventory_square_size/2;
			float pos_y = grid_container.Position.Y + (new_item.storage_y)*(Constants.inventory_square_size) + (new_item.sprite2D.Texture.GetHeight()/2) - Constants.inventory_square_size/2;
			new_item.Position = new Vector2(pos_x, pos_y);
		}
		

	}

	private void _OnInvItemClicked(InventoryItem inv_item)
	{

		if(held_item == inv_item)
			{
				
				FreeHeldItem();
				held_item = null;
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

	public void FreeHeldItem()
	{
		
		
		if(cur_hardpoint != null)
		{
			cur_hardpoint.attatched_weaponID = "empty";
			cur_hardpoint.SetWeaponModelSprite("empty");
			
		}
		if(held_item != null)
		{
			held_item.attatched = false;
			held_item.Reparent(GetNode("/root"));
			Debug.Print("X: " + held_item.storage_x.ToString());
			Debug.Print("Y: " + held_item.storage_y.ToString());
			
			
			for(int i = 0; i < held_item.size_x; i++)
			{
				for(int k = 0; k < held_item.size_y; k++)
				{
					rowed_grid_squares[held_item.storage_y + k][held_item.storage_x + i].occupied = false;
				}

			}
		}
		
	}

	private void _OnHardpointInfoChange(Hardpoint cur_hardpoint, string weapon_name, int inv_x, int inv_y, int level, string weight_class)
	{
		Debug.Print("weapon name: " + weapon_name);
		//Debug.Print("inv_x " + inv_x);
		//Debug.Print("inv_y " + inv_y);
		
		
		this.cur_hardpoint = cur_hardpoint;
		grid_squares.Clear();
		rowed_grid_squares.Clear();
		for(int i = 0; i < grid_container.GetChildCount(); i ++)
		{
			grid_container.GetChild(i).Free();
			i--;
		}

		

		if(weight_class.Equals("light"))
		{
			size_x = 2;
			size_y = 2;
			
		}
		else if(weight_class.Equals("medium"))
		{
			size_x = 4;
			size_y = 2;
		
		}
		else if(weight_class.Equals("heavy"))
		{
			size_x = 5;
			size_y = 3;
			
		}
		else
		{
			size_x = 2;
			size_y = 2;
			
		}

		grid_container.Position = new Vector2(-Constants.inventory_square_size*size_x/2 + Constants.inventory_square_size/2, -Constants.inventory_square_size*size_y/2 + Constants.inventory_square_size/2);
		grid_container.Columns = size_x;

		float scale_x = Constants.inventory_square_size*size_x / area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.X;
		float scale_y = Constants.inventory_square_size*size_y / area2D.GetChild<CollisionShape2D>(0).Shape.GetRect().Size.Y;
		area2D.Scale = new Vector2(scale_x,scale_y);
		//area2D.Position = new Vector2(-Constants.inventory_square_size*size_x/2 + Constants.inventory_square_size/2, -Constants.inventory_square_size*size_y/2 + Constants.inventory_square_size/2);

		grid_squares = new List<InventorySquare>();
		rowed_grid_squares = new List<List<InventorySquare>>();
		//Spawn all inventory squares
		for(int i = 0; i < size_y; i ++)
		{
			List<InventorySquare> new_row = new List<InventorySquare>();
			for(int k = 0; k < size_x; k ++)
			{
				InventorySquare new_square = inventory_square_scene.Instantiate<InventorySquare>();
				new_square.tile_x = k;
				new_square.tile_y = i;
				new_square.attatched_container = grid_container;
				
				grid_container.AddChild(new_square);
				
				//float scale_x = Constants.inventory_square_size/(float)new_square.sprite2D.Texture.GetWidth();
				//float scale_y = Constants.inventory_square_size/(float)new_square.sprite2D.Texture.GetHeight();
				//new_square.sprite2D.Scale = new Vector2(scale_x, scale_y);
				
				//new_square.area2d.Scale = new Vector2(scale_x, scale_y);
				grid_squares.Add(new_square);
				new_row.Add(new_square);

			}

			rowed_grid_squares.Add(new_row);
		}

		if(held_item is InventoryItem && IsInstanceValid(held_item))
		{
			held_item.Free();
		}

		if(!weapon_name.Equals("empty"))
		{
			PackedScene weapon_inv_item_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponInvItemUID(weapon_name));
			InventoryItem new_item = weapon_inv_item_scene.Instantiate<InventoryItem>();
			new_item.weapon_name = weapon_name;
			new_item.level = level;
			new_item.storage_x = inv_x;
			new_item.storage_y = inv_y;

			
			this.AddChild(new_item);
			
			
			held_item = new_item;
			
			
			float pos_x = grid_container.Position.X + (inv_x)*(Constants.inventory_square_size) + (new_item.sprite2D.Texture.GetWidth()/2) - Constants.inventory_square_size/2;
			float pos_y = grid_container.Position.Y + (inv_y)*(Constants.inventory_square_size) + (new_item.sprite2D.Texture.GetHeight()/2) - Constants.inventory_square_size/2;
			
			Label level_label = new_item.GetChild<Label>(2);
			level_label.Text = new_item.level.ToString();
			
			

			new_item.Position = new Vector2(0,0);
			new_item.Position += new Vector2(pos_x, pos_y);
			new_item.attatched = true;
			
		
		}
	}
}
