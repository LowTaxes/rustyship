using Godot;
using System;

public partial class Projectile : RigidBody2D
{
	public double damage;
	public double speed;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
