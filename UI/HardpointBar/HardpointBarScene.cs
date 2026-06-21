using Godot;
using System;

public partial class HardpointBarScene : Node2D
{
	Vector2 outer_position;
	Vector2 lower_position;
	public override void _Ready()
	{
		outer_position = this.Position;
		lower_position = new Vector2(this.Position.X, this.Position.Y + 300);
		SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointBarDropdown, new Callable(this, "_OnHardpointBardDropdown"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.ToBattle, new Callable(this, "_ToBattle"));
	}

	private void _OnHardpointBardDropdown()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", lower_position, .25);
	}
	private void _ToBattle()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", outer_position, .5);
	}
}
