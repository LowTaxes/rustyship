using Godot;
using System;

public partial class ToBattleButton : Button
{
	Vector2 upper_position;
	public override void _Ready()
	{
		SignalConnect.Instance.Connect(SignalConnect.SignalName.LootTaken.ToString(), new Callable(this, "_LootTaken"));
		upper_position = new Vector2(this.Position.X, 588);
	}

	private void _LootTaken()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", upper_position, .5);
	}
	private void _On_Pressed()
	{
		SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.ToBattle);
	}
}
