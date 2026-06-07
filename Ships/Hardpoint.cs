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
	public Area2D area2D;
	public override void _Ready()
	{
		area2D = GetChild<Area2D>(0);
		
		
	}
	public void SetWeaponModelSprite(string weaponID)
	{
		if(weaponID.Equals("empty"))
		{
			if(IsInstanceValid(attatched_weapon_model_sprite))
			{
				attatched_weapon_model_sprite.Free();
			}
		}
		else
		{
			Sprite2D weapon_model = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponModelUID(weaponID)).Instantiate<Sprite2D>();
			attatched_weapon_model_sprite = weapon_model;
			AddChild(weapon_model);
			weapon_model.LookAt(new Vector2(weapon_model.GlobalPosition.X, weapon_model.GlobalPosition.Y-1));
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
				SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.HardpointInfoChange.ToString(), this, attatched_weaponID, inv_x, inv_y, level, weight_class);
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
