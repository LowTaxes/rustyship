using Godot;
using System;
using System.Collections.Generic;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
using RunDataEnum = Constants.RunDataEnum;
using System.Diagnostics;

public partial class Inventory : Control
{

	PackedScene inventory_square_scene;
	Control control_child;
	VBoxContainer v_box_container;
	HBoxContainer[] h_box_containers;
	[Export] public bool is_player;
	[Export] public bool is_active_storage;

	public bool[][] occupied_squares;

/*
	public override void _Ready()
	{
		
		
		inventory_square_scene = GD.Load<PackedScene>("uid://b1wteem6372ip");
		control_child = GetChild<Control>(0);
		v_box_container = control_child.GetChild<VBoxContainer>(0);
		v_box_container.AddThemeConstantOverride("separation", Constants.inv_square_pixel_width);
		v_box_container.Position = new Vector2(Constants.inv_square_pixel_width/2,Constants.inv_square_pixel_width/2);

		Array run_data;
		int size_x;
		int size_y;

		if(is_player)
		{
			run_data = (Array)RunData.Instance.LoadUserData()["player"];
		}
		else
		{
			run_data = (Array)RunData.Instance.LoadUserData()["enemy"];
		}
		
		if(is_active_storage)
		{
			size_x = (int)((Dictionary)run_data[(int)RunDataEnum.ACTIVE_INVENTORY_SIZE])["x"];
			size_y = (int)((Dictionary)run_data[(int)RunDataEnum.ACTIVE_INVENTORY_SIZE])["y"];
		}
		else
		{
			size_x = (int)((Dictionary)run_data[(int)RunDataEnum.STORAGE_INVENTORY_SIZE])["x"];
			size_y = (int)((Dictionary)run_data[(int)RunDataEnum.STORAGE_INVENTORY_SIZE])["y"];
		}

		occupied_squares = new bool[size_y][];

		for(int i = 0; i < size_y; i++)
		{
			bool[] new_row = new bool[size_x];
			for(int k = 0; k < size_x; k++)
			{
				new_row[k] = false;
			}
			occupied_squares[i] = new_row;
		}


		HBoxContainer[] h_box_containers = new HBoxContainer[size_y];

		GetChild<Control>(0).CustomMinimumSize = new Vector2(Constants.inv_square_pixel_width * size_x,Constants.inv_square_pixel_width * size_y);

		for(int i = 0; i < size_y; i++)
		{
			HBoxContainer new_Hbox = new HBoxContainer();
			new_Hbox.AddThemeConstantOverride("separation", Constants.inv_square_pixel_width);

			h_box_containers[i] = new_Hbox;
			v_box_container.AddChild(new_Hbox);
			
			
		}


		for (int i = 0; i < size_x; i++)
		{
			for (int k = 0; k < size_y; k++)
			{
				InventorySquare new_square = inventory_square_scene.Instantiate<InventorySquare>();
				new_square.tile_x = i;
				new_square.tile_y = k;
				new_square.attatched_inventory = this;
				h_box_containers[k].AddChild(new_square);
				Debug.Print(new_square.relative_position.ToString());
				
				
				Label new_label = new Label();
				new_label.Text = "hi";
				h_box_containers[k].AddChild(new_label);
				
			}
		}

	}

	public void AddItem(InventoryItem new_inventory_item)
	{
		//Debug.Print("1");
		bool placeable = true;
		
		
		This loop organizes the inventory squares by 
		distance from the inventory item least to greatest
		
		
		
		Debug.Print("-----------");
		for(int i = 0; i < new_inventory_item.touching_squares.Count; i++)
		{
			
			InventorySquare closest = new_inventory_item.touching_squares[i];
			double distance_x_closest = Mathf.Abs(new_inventory_item.GlobalPosition.X - closest.GlobalPosition.X);
			double distance_y_closest = Mathf.Abs(new_inventory_item.GlobalPosition.Y - closest.GlobalPosition.Y);
			double distance_closest = Mathf.Sqrt(Mathf.Pow(distance_x_closest, 2) + Mathf.Pow(distance_y_closest, 2));
			Debug.Print(closest.relative_position.ToString());
			
		}
		Debug.Print("-----------");
		
		/*
		for(int i = 0; i < new_inventory_item.touching_squares.Count; i++)
		{
			
			
			for(int k = i+1; k<new_inventory_item.touching_squares.Count; k++)
			{
				if(new_inventory_item.touching_squares[i] == new_inventory_item.touching_squares[k])
				{
					Debug.Print("man wtf");
				}
			}
			
		}
		

		for(int i = 0; i < new_inventory_item.touching_squares.Count; i++)
		{
			InventorySquare closest = new_inventory_item.touching_squares[i];
			double distance_x_closest = Mathf.Abs(new_inventory_item.GlobalPosition.X - closest.relative_position.X);
			double distance_y_closest = Mathf.Abs(new_inventory_item.GlobalPosition.Y - closest.relative_position.Y);
			double distance_closest = Mathf.Sqrt(Mathf.Pow(distance_x_closest, 2) + Mathf.Pow(distance_y_closest, 2));
			
			for (int k = i + 1; k < new_inventory_item.touching_squares.Count; k++)
			{
				double distance_x_current = Mathf.Abs(new_inventory_item.GlobalPosition.X - new_inventory_item.touching_squares[k].relative_position.X);
				double distance_y_current = Mathf.Abs(new_inventory_item.GlobalPosition.Y - new_inventory_item.touching_squares[k].relative_position.Y);
				double distance_current = Mathf.Sqrt(Mathf.Pow(distance_x_current, 2) + Mathf.Pow(distance_y_current, 2));

				if (distance_current < distance_closest)
				{
					InventorySquare temp = closest;
					new_inventory_item.touching_squares[i] = new_inventory_item.touching_squares[k];
					new_inventory_item.touching_squares[k] = temp;

				}
			}

			
			
		}

		

		
		
		
		//this loop puts the size_x * size_y closest squares into an organized list

		List<InventorySquare> organized_list = new List<InventorySquare>();
		for (int i = 0; i < new_inventory_item.size_x * new_inventory_item.size_y; i++)
		{
			organized_list.Add(new_inventory_item.touching_squares[i]);
			Debug.Print(new_inventory_item.touching_squares[i].tile_y.ToString());
			
		}

		
		//This loop checks if the squares are already occupied
		//If one of them is then placeable is false, the placing fails and nothing happens
		
		for (int i = 0; i < organized_list.Count; i++)
		{
			if (occupied_squares[organized_list[i].tile_y][organized_list[i].tile_x] == true)
			{
				placeable = false;
			}
		}
		

		if(placeable)
		{
		
		//this loop finds the closest square to the control_node in order to be used as
		//a reference point for placement INCOMPLETE
		 

		InventorySquare closest_to_control = organized_list[0];

		for(int i = 1; i < organized_list.Count; i++)
		{
			float distance_x_closest = Mathf.Abs(control_child.GlobalPosition.X - closest_to_control.relative_position.X);
			float distance_y_closest = Mathf.Abs(control_child.GlobalPosition.Y - closest_to_control.relative_position.Y);
			float distance_closest = Mathf.Sqrt(Mathf.Pow(distance_x_closest,2) + Mathf.Pow(distance_y_closest,2));

			float distance_x_current = Mathf.Abs(control_child.GlobalPosition.X - organized_list[i].relative_position.X);
			float distance_y_current = Mathf.Abs(control_child.GlobalPosition.Y - organized_list[i].relative_position.Y);
			float distance_current = Mathf.Sqrt(Mathf.Pow(distance_x_current,2) + Mathf.Pow(distance_y_current,2));

			if(distance_current < distance_closest)
			{
				closest_to_control = organized_list[i];
			}
		}	
		

		
		//adds the new_inventory_item as a child of the control node of this inventory
		//and moves it accordingly relative to that control node INCOMPLETE
		
		new_inventory_item.Reparent(control_child);
		new_inventory_item.Position = new Vector2((closest_to_control.tile_x * Constants.inv_square_pixel_width) + (new_inventory_item.size_x * Constants.inv_square_pixel_width /2),closest_to_control.tile_y * Constants.inv_square_pixel_width + (new_inventory_item.size_y * Constants.inv_square_pixel_width /2));
		Debug.Print("attatched");
		}
		
		
	}
	*/
}
