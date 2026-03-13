using Godot;
using System;
using System.Diagnostics;

public partial class Projectile : Area2D
{
	public double damage;
	public double armor_damage_modifier;
	public double crit_chance;
	public double speed;
	public bool is_player;
	public Vector2 travel_direction;

	public override void _Ready()
	{
		//Debug.Print(Position.ToString());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Debug.Print((travel_direction.Y * speed).ToString());
		Translate(new Vector2((float)(travel_direction.X * speed ), (float)(travel_direction.Y * speed )));	
	}


	private void _OnBodyEntered(Node other)
	{
		
		if(other is Ship)
		{
			Ship ship = (Ship)other;
			if (ship.is_player != is_player)
			{
				//Debug.Print("hi");
				ship.takeDamage(damage, armor_damage_modifier, crit_chance);
				QueueFree();
			}
		}
	}
}
