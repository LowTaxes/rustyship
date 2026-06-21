using Godot;
using System;

public partial class RightPanel : Sprite2D
{
	Vector2 outer_position;
	public override void _Ready()
	{
		outer_position = this.Position;
		SignalConnect.Instance.Connect(SignalConnect.SignalName.BattleSequenceBegins, new Callable(this, "_OnBattleSequenceBegins"));
	}

	private void _OnBattleSequenceBegins()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(this.Position.X - 433,0), 1);
	}
}
