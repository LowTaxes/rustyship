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
	VBoxContainer v_box_container;
	HBoxContainer[] h_box_containers;
	[Export] public bool is_player;
	[Export] public bool is_active_storage;


	public override void _Ready()
	{
		
		
		inventory_square_scene = GD.Load<PackedScene>("uid://b1wteem6372ip");

		v_box_container = GetChild<Control>(0).GetChild<VBoxContainer>(0);
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
				h_box_containers[k].AddChild(new_square);
				
				/*
				Label new_label = new Label();
				new_label.Text = "hi";
				h_box_containers[k].AddChild(new_label);
				*/
			}
		}


		
	}

	
}
