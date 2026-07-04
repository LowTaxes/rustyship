using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;


public partial class Hardpoint : Sprite2D
{	
	
	public Sprite2D attatched_weapon_model_sprite;
	public string attatched_weaponID = "empty";
	public int level = 0;
	public Vector2 placement_position = new Vector2(0,0);
	public string weight_class = "light";
	public int inv_x = 0;
	public int inv_y = 0;
	public bool moveable = true;
	public bool mouse_hovering = false;
	public bool mouse_dragging = false;
	public bool attatched = false;
	public Area2D core_area;
	public Area2D placement_area;
	public Area2D modifier_area;
	public List<Modifier> active_modifiers;

	public Sprite2D placement_arrow;
	public Sprite2D placement_x;
	public override void _Ready()
	{
		core_area = GetChild<Area2D>(0);
		placement_area = GetChild<Area2D>(1);
		modifier_area = GetChild<Area2D>(2);
		placement_arrow = GetChild<Sprite2D>(3);
		placement_x = GetChild<Sprite2D>(4);
		
		active_modifiers = new List<Modifier>();

		core_area.AreaEntered += CoreAreaEntered;
		core_area.AreaExited += CoreAreaExited;

		placement_arrow.Visible = false;
		placement_x.Visible = false;
		
		
	}

    public override void _PhysicsProcess(double delta)
    {
        if(this.GlobalPosition.Y > Constants.SEPERATOR_Y || this.GlobalPosition.Y < Constants.UPPER_SEPERATOR_Y)
		{
			//Debug.Print("left hardpoint area");
			PackedScene new_inv_item_scene = null;
			if(ConstantData.GetBattleItemDesignation(attatched_weaponID).Equals(Constants.WEAPON_DESIGNATION))
			{
				new_inv_item_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponInvItemUID(attatched_weaponID));
			}
			else if (ConstantData.GetBattleItemDesignation(attatched_weaponID).Equals(Constants.DEFENSIVE_DESIGNATION))
			{
				new_inv_item_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetDefensiveInventoryItemUID(attatched_weaponID));
			}
			else if (ConstantData.GetBattleItemDesignation(attatched_weaponID).Equals(Constants.SUPPORT_DESIGNATION))
			{
				new_inv_item_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetSupportInvItemUID(attatched_weaponID));
			}
		


			InventoryItem new_inv_item = new_inv_item_scene.Instantiate<InventoryItem>();
			GetNode("/root").AddChild(new_inv_item);
			new_inv_item.weapon_name = attatched_weaponID;
			new_inv_item.level = level;
			new_inv_item.mouse_hovering = true;
			new_inv_item.mouse_dragging = true;
			new_inv_item.Position = GetGlobalMousePosition();
			GarbageCollector.all_inv_items.Add(new_inv_item);
			GarbageCollector.all_hardpoints.Remove(this);
			Debug.Print(new_inv_item.Name);
			this.QueueFree();
			
		}
		
    }

	

	public void Initialize(string weaponID)
	{
		if(weaponID.Equals("empty"))
		{
			if(IsInstanceValid(attatched_weapon_model_sprite))
			{
				attatched_weapon_model_sprite.Free();
			}
		}


		else if(ConstantData.GetBattleItemDesignation(weaponID).Equals(Constants.WEAPON_DESIGNATION))
		{
			Sprite2D weapon_model = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponModelUID(weaponID)).Instantiate<Sprite2D>();
			attatched_weapon_model_sprite = weapon_model;
			AddChild(weapon_model);
			MoveChild(weapon_model,0);
			weapon_model.LookAt(new Vector2(weapon_model.GlobalPosition.X, weapon_model.GlobalPosition.Y-1));
			placement_area.Scale *= ConstantData.GetWeaponPlacementRadius(weaponID);

			weight_class = ConstantData.GetWeaponWeightClass(weaponID);
			
		}
		else if (ConstantData.GetBattleItemDesignation(weaponID).Equals(Constants.DEFENSIVE_DESIGNATION))
		{
			Sprite2D defensive_model = ResourceLoader.Load<PackedScene>(ConstantData.GetDefensiveBattleModelUID(weaponID)).Instantiate<Sprite2D>();
			attatched_weapon_model_sprite = defensive_model;
			AddChild(defensive_model);
			MoveChild(defensive_model,0);
			placement_area.Scale *= ConstantData.GetDefensivePlacementRadius(weaponID);

			weight_class = ConstantData.GetDefensiveWeightClass(weaponID);

		}
		else if (ConstantData.GetBattleItemDesignation(weaponID).Equals(Constants.SUPPORT_DESIGNATION))
		{
			Sprite2D support_model = ResourceLoader.Load<PackedScene>(ConstantData.GetSupportBattleModelUID(weaponID)).Instantiate<Sprite2D>();
			attatched_weapon_model_sprite = support_model;
			AddChild(support_model);
			MoveChild(support_model,0);
			
			
			placement_area.Scale = new Vector2(ConstantData.GetSupportPlacementRadius(weaponID),ConstantData.GetSupportPlacementRadius(weaponID));
			modifier_area.Scale = new Vector2(ConstantData.GetSupportRadius(weaponID),ConstantData.GetSupportRadius(weaponID));

			weight_class = ConstantData.GetSupportWeightClass(weaponID);
		}

		
	}

	private void _OnMouseEntered()
	{
		mouse_hovering = true;
		//Debug.Print(mouse_hovering.ToString());
	}

	private void _OnMouseExited()
	{
		mouse_hovering = false;
		mouse_dragging = false;
	}


	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouse mouse_event)
		{
			
			if(mouse_event.IsActionPressed("left_click") && mouse_hovering)
			{
				mouse_dragging = true;
				attatched = false;
				//SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.HardpointInfoChange.ToString(), this, attatched_weaponID, inv_x, inv_y, level, weight_class);
				for(int i = 0; i < modifier_area.GetOverlappingAreas().Count; i++)
				{
					if(modifier_area.GetOverlappingAreas()[i].GetParent() is Hardpoint overlapping_hardpoint && overlapping_hardpoint != this)
					{
						if(modifier_area.OverlapsArea(overlapping_hardpoint.core_area) && !overlapping_hardpoint.placement_x.Visible)
						{
							overlapping_hardpoint.placement_arrow.Visible = true;
						}
					}
				}
				SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.HardpointClicked.ToString(), this);
			}
			else if(mouse_event.IsActionReleased("left_click"))
			{
				//placement_x.Visible = false;
				placement_arrow.Visible = false;

				if(mouse_dragging)
				{
					mouse_dragging = false;
					SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.HardpointReleased.ToString(), this);
				}
				
			}
			if(@event is InputEventMouseMotion mouse_motion_event && moveable && mouse_dragging)
			{
				Position = Position + mouse_motion_event.Relative *Constants.camera_zoom_dragging_modifier;
			}
			
		}
	}

	//support specific methods
	public Modifier GetModifier()
	{
		Modifier returned_modifer = null;
		if(ConstantData.GetBattleItemDesignation(attatched_weaponID).Equals(Constants.SUPPORT_DESIGNATION))
		{
			returned_modifer = new Modifier(
				ConstantData.GetSupportType(attatched_weaponID),
				ConstantData.GetSupportModifierType(attatched_weaponID),
				ConstantData.GetSupportWeightClassRestriction(attatched_weaponID),
				ConstantData.GetSupportAmount(attatched_weaponID),
				this
			);
		}
		else
		{
			Debug.Print("This isn't a Support Hardpoint so it cant make a modifier");
		}

		return returned_modifer;
	}
	
	private void CoreAreaEntered(Area2D area2D)
	{
		if(area2D.GetParent() is Hardpoint hardpoint_entered && hardpoint_entered != this)
		{
			if(core_area.OverlapsArea(hardpoint_entered.placement_area))
			{
				placement_x.Visible = true;
				placement_arrow.Visible = false;
			}
			else if (core_area.OverlapsArea(hardpoint_entered.modifier_area) && !mouse_dragging && hardpoint_entered.mouse_dragging)
			{
				placement_x.Visible = false;
				placement_arrow.Visible = true;
			}
		}
	}
	private void CoreAreaExited(Area2D area2D)
	{
		if(area2D.GetParent() is Hardpoint hardpoint_entered)
		{
			if(area2D == hardpoint_entered.modifier_area)
			{
				placement_arrow.Visible = false;
			}

			if(area2D == hardpoint_entered.placement_area)
			{
				//checks if its still overlapping any placement areas
				bool overlapping = false;
				for(int i = 0; i < core_area.GetOverlappingAreas().Count; i++)
				{
					if(core_area.GetOverlappingAreas()[i].GetParent() is Hardpoint other_hardpoint_entered && other_hardpoint_entered != this)
					{
						if(core_area.OverlapsArea(other_hardpoint_entered.placement_area))
						{
							overlapping = true;
						}
					}
				}
				if(!overlapping)
				{
					placement_x.Visible = false;
				}
				


				if(core_area.OverlapsArea(hardpoint_entered.modifier_area) && !mouse_dragging)
				{
					placement_arrow.Visible = true;
				}
			}
			
		}
	}

	
	
	
		
	
	
}
