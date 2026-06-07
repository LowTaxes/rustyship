using Godot;
using System;

public partial class BottomDoors : Node2D
{
	
	public override void _Ready()
	{
		SignalConnect.Instance.Connect(SignalConnect.SignalName.ToBattle, new Callable(this, "_ToBattle"));
	}

	private void _ToBattle()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(this.Position.X, 600), .5);
	}
}
