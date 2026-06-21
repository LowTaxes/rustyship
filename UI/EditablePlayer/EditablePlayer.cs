using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Array = Godot.Collections.Array;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class EditablePlayer : Control
{
	PackedScene ship_model_scene;
	ShipModel ship_model;
	List<Hardpoint> active_hardpoints;
	public bool hardpoint_editing = true;
	public Vector2 outer_position;

	public Sprite2D ship_placement_area;
	
	
	public override void _Ready()
	{

		SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointAdded, new Callable(this, "_HardpointAdded"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointRemoved, new Callable(this, "_HardpointRemoved"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.CanEditHardpoints, new Callable(this, "_CanEditHardpoints"));

		SignalConnect.Instance.Connect(SignalConnect.SignalName.ChangeToNextScene, new Callable(this, "_ChangeToNextScene"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.ToBattle, new Callable(this, "_ToBattle"));

		SignalConnect.Instance.Connect(SignalConnect.SignalName.InvItemClicked, new Callable(this, "_OnInvItemClicked"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.InvItemReleased, new Callable(this, "_OnInvItemReleased"));

		SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointRemoved, new Callable(this, "_OnHardpointRemoved"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointAdded, new Callable(this, "_OnHardpointAdded"));
		
		ship_placement_area = GetChild<Sprite2D>(0);
		ship_placement_area.Visible = false;
		//get ship and run data
		Dictionary run_data = RunData.Instance.LoadUserData();
		Array player_data = (Array) run_data["player"];
		Array ship_data = (Array)ConstantData.ShipData[RunData.GetPlayerShipTemplateID()];


		
		//spawn ship and scale it
		ship_model_scene = GD.Load<PackedScene>(ship_data[(int)Constants.ShipDataEnum.SHIP_MODEL_UID].ToString());
		ship_model = ship_model_scene.Instantiate<ShipModel>();
		AddChild(ship_model);
		//ship_model.Position = new Vector2(this.Size.X/2, Size.Y/2);

		active_hardpoints = RunData.GetPlayerActiveHardpoints();

		for(int i = 0; i<active_hardpoints.Count; i++)
		{
			//Debug.Print("hi");
			AddChild(active_hardpoints[i]);
			GarbageCollector.all_hardpoints.Add(active_hardpoints[i]);
			active_hardpoints[i].Position = active_hardpoints[i].placement_position;
			active_hardpoints[i].Initialize(active_hardpoints[i].attatched_weaponID);
		}
		outer_position = this.Position;

		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(0,this.Position.Y), 1);
		
		tween.Finished += _OnLoadInFinished;
		
	}

	private void _HardpointRemoved(Hardpoint hardpoint)
	{
		
		if(hardpoint_editing)
		{
			for(int i = 0; i < active_hardpoints.Count; i++)
			{
				//Debug.Print(hardpoint.ToString());
				//Debug.Print(active_hardpoints[i].ToString());
				if(active_hardpoints[i] == hardpoint)
				{
					//Debug.Print("1");
					
					active_hardpoints[i].moveable = true;
					active_hardpoints.Remove(hardpoint);
					//Debug.Print("test");
					

				}
			}
		}
		
		
	}

	private void _HardpointAdded(Hardpoint hardpoint)
	{
		Area2D model_area2d = ship_model.GetChild<Area2D>(0);
		Array<Area2D> overlapping_areas = model_area2d.GetOverlappingAreas();

		bool intersects_ship = false;
		bool intersects_other_center_area = false;

		//checks if hardpoint intersects with ship
		for(int i = 0; i < overlapping_areas.Count;i++)
		{
			if(overlapping_areas[i] == hardpoint.area2D)
			{
				intersects_ship = true;			
			}
		}
	
		//checks if the hardpoint placement area intersects any hardpoints
		for(int i = 0; i < hardpoint.placement_area.GetOverlappingAreas().Count; i++)
		{
			if(hardpoint.placement_area.GetOverlappingAreas()[i].Name.Equals("CentralArea") && hardpoint.placement_area.GetOverlappingAreas()[i] != hardpoint.area2D)
			{
				intersects_other_center_area = true;
			}
		}
	
		//checks if other hardpoint placement areas intersect this hardpoint
		for(int i = 0; i<GarbageCollector.all_hardpoints.Count;i++)
		{
			for(int k = 0; k<GarbageCollector.all_hardpoints[i].placement_area.GetOverlappingAreas().Count; k++)
			{
				if(GarbageCollector.all_hardpoints[i].placement_area.GetOverlappingAreas()[k] == hardpoint.area2D && (GarbageCollector.all_hardpoints[i] != hardpoint))
				{
					intersects_other_center_area = true;
				}
			}
		}

		if(!intersects_other_center_area && intersects_ship)
		{
			active_hardpoints.Add(hardpoint);
			hardpoint.Reparent(this);
			hardpoint.placement_position = hardpoint.Position;
			hardpoint.attatched = true;
			//Debug.Print("placed");
		}
		else
		{
			//Debug.Print("failed");
		}

	}

	private void _CanEditHardpoints(bool can_edit)
	{
		hardpoint_editing = can_edit;
	}

	private void _OnLoadInFinished()
	{
		SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.EditablePlayerReady.ToString());
	}

	private void _OnLoadOutFinished()
	{
		SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.BattleSequenceBegins.ToString());

		
	}

/*

	private void _ActiveItemAdded(InventoryItem inv_item, int hardpoint_index)
	{
		PackedScene new_model_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponModelUID(inv_item.weapon_name));
		Sprite2D new_weapon_model = new_model_scene.Instantiate<Sprite2D>();

		foreach (Sprite2D model in ship_model.hardpoints[hardpoint_index].GetChildren())
		{
			model.Free();
		}
		ship_model.hardpoints[hardpoint_index].AddChild(new_weapon_model);
		new_weapon_model.LookAt(new Vector2(new_weapon_model.GlobalPosition.X, new_weapon_model.GlobalPosition.Y *2));

		attatched_inventory_items[hardpoint_index] = inv_item;

	}

	private void _ActiveItemRemoved(int hardpoint_index)
	{
		foreach (Sprite2D model in ship_model.hardpoints[hardpoint_index].GetChildren())
		{
			model.Free();
		}
		attatched_inventory_items[hardpoint_index] = RunData.GetEmptyInvItem();

		
	}
*/

	private void _ToBattle()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", outer_position, .5);
		tween.Finished += _OnLoadOutFinished;
		
		Array new_p_active_hardpoints = new Array();
		for(int i = 0; i < active_hardpoints.Count; i++)
		{
			Dictionary hardpoint_dict = new Dictionary();
			hardpoint_dict.Add("level", active_hardpoints[i].level);
			hardpoint_dict.Add("weaponID", active_hardpoints[i].attatched_weaponID);
			hardpoint_dict.Add("x", active_hardpoints[i].placement_position.X);
			hardpoint_dict.Add("y", active_hardpoints[i].placement_position.Y);
			hardpoint_dict.Add("weight_class", active_hardpoints[i].weight_class);
			hardpoint_dict.Add("inv_x", active_hardpoints[i].inv_x);
			hardpoint_dict.Add("inv_y", active_hardpoints[i].inv_y);
			new_p_active_hardpoints.Add(hardpoint_dict);
		}
		//Debug.Print(RunData.p_active_hardpoints.ToString());
		RunData.p_active_hardpoints = new_p_active_hardpoints;
		//Debug.Print(RunData.p_active_hardpoints.ToString());

		
	}

	private void _OnInvItemClicked(InventoryItem inv_item)
	{
		ship_model.Visible = false;
		ship_placement_area.Visible = true;
	}

	private void _OnInvItemReleased(InventoryItem inv_item)
	{
		ship_model.Visible = true;
		ship_placement_area.Visible = false;

	}

	private void _OnHardpointRemoved(Hardpoint hardpoint)
	{
		ship_model.Visible = false;
		ship_placement_area.Visible = true;
	}

	private void _OnHardpointAdded(Hardpoint hardpoint)
	{
		ship_model.Visible = true;
		ship_placement_area.Visible = false;
	}

	
}
