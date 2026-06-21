using Godot;
using System;
using System.Diagnostics;

public partial class Projectile : Area2D
{
	public double damage;
	public double armor_damage_modifier;

	public double speed;
	public bool is_player;
	public Vector2 travel_direction;

	public override void _Ready()
	{
		//Debug.Print(Position.ToString());
		SignalConnect.Instance.Connect(SignalConnect.SignalName.StartNewLoop, new Callable(this, "_OnStartNewLoop"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Debug.Print((travel_direction.Y * speed).ToString());
		Translate(new Vector2((float)(travel_direction.X * speed ), (float)(travel_direction.Y * speed )));	
	}


	private void _OnAreaEntered(Area2D other_area2d)
	{
		//Debug.Print("touching");
		if(other_area2d.GetParent().GetParent() is EnemyShip && is_player == true)
		{	
			SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.EnemyDamageTaken, damage, armor_damage_modifier);
			this.CallDeferred("free");

		}

		if(other_area2d.GetParent().GetParent() is PlayerShip && is_player == false)
		{	
			SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.PlayerDamageTaken, damage, armor_damage_modifier);
			this.CallDeferred("free");

		}


		/*
			//Debug.Print("yipee");
			if(this.is_player == true && model.is_player == false)
			{
				SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.EnemyDamageTaken, damage, armor_damage_modifier);
				//QueueFree();
			}
			if(this.is_player == false && model.is_player == true)
			{
				SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.PlayerDamageTaken, damage, armor_damage_modifier);
				//QueueFree();
			}
			*/
	}

	private void _OnStartNewLoop()
	{
		this.CallDeferred("free");
	}
}
