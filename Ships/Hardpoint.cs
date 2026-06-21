using Godot;
using System;
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
	public Area2D area2D;
	public Area2D placement_area;
	public override void _Ready()
	{
		area2D = GetChild<Area2D>(0);
		
		
		
		
	}

    public override void _Process(double delta)
    {
        if(this.GlobalPosition.Y > Constants.SEPERATOR_Y)
		{
			Debug.Print("left hardpoint area");
			PackedScene new_inv_item_scene = null;
			if(ConstantData.GetBattleItemDesignation(attatched_weaponID).Equals(Constants.WEAPON_DESIGNATION))
			{
				new_inv_item_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponInvItemUID(attatched_weaponID));
			}
			else if (ConstantData.GetBattleItemDesignation(attatched_weaponID).Equals(Constants.DEFENSIVE_DESIGNATION))
			{
				new_inv_item_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetDefensiveInventoryItemUID(attatched_weaponID));
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
			weapon_model.LookAt(new Vector2(weapon_model.GlobalPosition.X, weapon_model.GlobalPosition.Y-1));


			if(ConstantData.GetWeaponWeightClass(weaponID).Equals("light"))
			{
				PackedScene placement_area_scene = ResourceLoader.Load<PackedScene>("uid://cmcstf16p78g4");//smallarea uid
				Area2D new_area = placement_area_scene.Instantiate<Area2D>();
				placement_area = new_area;
				AddChild(new_area);
				

			}
			else if(ConstantData.GetWeaponWeightClass(weaponID).Equals("medium"))
			{
				PackedScene placement_area_scene = ResourceLoader.Load<PackedScene>("uid://cfkgkj1nsleg0");//mediumarea uid
				Area2D new_area = placement_area_scene.Instantiate<Area2D>();
				placement_area = new_area;
				AddChild(new_area);

			}
			else
			{
				PackedScene placement_area_scene = ResourceLoader.Load<PackedScene>("uid://dlwbsyypwmufy");//largearea uid
				Area2D new_area = placement_area_scene.Instantiate<Area2D>();
				placement_area = new_area;
				AddChild(new_area);
			}
		}

		else if (ConstantData.GetBattleItemDesignation(weaponID).Equals(Constants.DEFENSIVE_DESIGNATION))
		{
			Sprite2D defensive_model = ResourceLoader.Load<PackedScene>(ConstantData.GetDefensiveBattleModelUID(weaponID)).Instantiate<Sprite2D>();
			attatched_weapon_model_sprite = defensive_model;
			AddChild(defensive_model);


			PackedScene placement_area_scene = ResourceLoader.Load<PackedScene>("uid://cmcstf16p78g4");//smallarea uid
			Area2D new_area = placement_area_scene.Instantiate<Area2D>();
			placement_area = new_area;
			AddChild(new_area);





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
				SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.HardpointRemoved.ToString(), this);
			}
			else if(mouse_event.IsActionReleased("left_click") && mouse_dragging)
			{
				mouse_dragging = false;
				SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.HardpointAdded.ToString(), this);
			}
			if(@event is InputEventMouseMotion mouse_motion_event && moveable && mouse_dragging)
			{
				Position = Position + mouse_motion_event.Relative *Constants.camera_zoom_dragging_modifier;
			}
			
		}
	}
	
	
	
		
	
	
}
