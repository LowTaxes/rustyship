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

		SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointReleased, new Callable(this, "_HardpointReleased"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointClicked, new Callable(this, "_HardpointClicked"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.CanEditHardpoints, new Callable(this, "_CanEditHardpoints"));

		SignalConnect.Instance.Connect(SignalConnect.SignalName.ChangeToNextScene, new Callable(this, "_ChangeToNextScene"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.ToBattle, new Callable(this, "_ToBattle"));

		SignalConnect.Instance.Connect(SignalConnect.SignalName.InvItemClicked, new Callable(this, "_OnInvItemClicked"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.InvItemReleased, new Callable(this, "_OnInvItemReleased"));

		SignalConnect.Instance.Connect(SignalConnect.SignalName.PartnerInvItemRemoved, new Callable(this, "_OnPartnerInvItemRemoved"));


		
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
			SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.HardpointAdded, active_hardpoints[i]);
			GarbageCollector.all_hardpoints.Add(active_hardpoints[i]);
			active_hardpoints[i].Position = active_hardpoints[i].placement_position;
			active_hardpoints[i].Initialize(active_hardpoints[i].attatched_weaponID);
			active_hardpoints[i].attatched = true;
		}
		
		outer_position = this.Position;

		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(0,this.Position.Y), 1);
		
		tween.Finished += _OnLoadInFinished;
		
	}

    


	private void _HardpointClicked(Hardpoint hardpoint)
	{
		ship_model.Visible = false;
		ship_placement_area.Visible = true;
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
					hardpoint.active_modifiers.Clear();
					RefreshActiveModifiers();
					SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.HardpointRemoved, hardpoint);
					//Debug.Print("test");
					

				}
			}
		}
		
		
	}

	private void _HardpointReleased(Hardpoint hardpoint)
	{
		Area2D model_area2d = ship_model.GetChild<Area2D>(0);
		Array<Area2D> overlapping_areas = model_area2d.GetOverlappingAreas();
		
		

		ship_model.Visible = true;
		ship_placement_area.Visible = false;

		bool intersects_ship = false;
		bool intersects_other_center_area = false;

		//checks if hardpoint intersects with ship
		for(int i = 0; i < overlapping_areas.Count;i++)
		{
			if(overlapping_areas[i] == hardpoint.core_area)
			{
				intersects_ship = true;			
			}
		}
	
		//checks if the hardpoint placement area intersects any hardpoints
		for(int i = 0; i < hardpoint.placement_area.GetOverlappingAreas().Count; i++)
		{
			if(hardpoint.placement_area.GetOverlappingAreas()[i].Name.Equals("CentralArea") && hardpoint.placement_area.GetOverlappingAreas()[i] != hardpoint.core_area)
			{
				intersects_other_center_area = true;
			}
		}
	
		//checks if other hardpoint placement areas intersect this hardpoint
		for(int i = 0; i<GarbageCollector.all_hardpoints.Count;i++)
		{
			for(int k = 0; k<GarbageCollector.all_hardpoints[i].placement_area.GetOverlappingAreas().Count; k++)
			{
				if(GarbageCollector.all_hardpoints[i].placement_area.GetOverlappingAreas()[k] == hardpoint.core_area && (GarbageCollector.all_hardpoints[i] != hardpoint))
				{
					intersects_other_center_area = true;
				}
			}
		}

		if(!intersects_other_center_area && intersects_ship)
		{
			SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.HardpointAdded, hardpoint);
			active_hardpoints.Add(hardpoint);
			hardpoint.Reparent(this);
			hardpoint.placement_position = hardpoint.Position;
			hardpoint.attatched = true;
			RefreshActiveModifiers();
			
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
		RefreshActiveModifiers();
	}

	private void _OnLoadOutFinished()
	{
		SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.BattleSequenceBegins.ToString());

		
	}

	public void RefreshActiveModifiers()
	{
		for(int i = 0; i < active_hardpoints.Count; i++)
		{
			active_hardpoints[i].active_modifiers.Clear();//clears all current modifiers to ensure no duplicates are added
			
			for(int k = 0; k < active_hardpoints.Count; k++)
			{
				if( i != k) //checks each active hardpoint against every other active hardpoint
				{
					Hardpoint current_hardpoint = active_hardpoints[i];
					Hardpoint comparison_hardpoint = active_hardpoints[k];
				
					if (current_hardpoint.core_area.OverlapsArea(comparison_hardpoint.modifier_area) && ConstantData.GetBattleItemDesignation(comparison_hardpoint.attatched_weaponID).Equals(Constants.SUPPORT_DESIGNATION))
					{
						//adds a new modifier to the current_hardpoint 
						current_hardpoint.active_modifiers.Add(comparison_hardpoint.GetModifier());
						



					}


				}
			}
			//Debug.Print("current modifiers: " + active_hardpoints[i].active_modifiers.Count.ToString());
		}
	}


	private void _ToBattle()
	{
		
		
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
			SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.CreateWeapon, active_hardpoints[i]);
		}
		//Debug.Print(RunData.p_active_hardpoints.ToString());
		RunData.p_active_hardpoints = new_p_active_hardpoints;
		//Debug.Print(RunData.p_active_hardpoints.ToString());

		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", outer_position, .5);
		tween.Finished += _OnLoadOutFinished;
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

	

	

	private void _OnPartnerInvItemRemoved(Hardpoint partner_hardpoint)
	{
		active_hardpoints.Remove(partner_hardpoint);
		GarbageCollector.all_hardpoints.Remove(partner_hardpoint);
		partner_hardpoint.CallDeferred("free");
	}
	
}
