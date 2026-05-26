using Godot;
using System;
using System.Diagnostics;

public partial class EnemySelectDropdown : Control
{
	Panel panel;
	Sprite2D border_sprite;

	Vector2 outer_position;
	Vector2 inner_position;
	Vector2 lowest_position;
	public override void _Ready()
	{
		panel = GetChild<Panel>(0);
		border_sprite = GetChild<Sprite2D>(1);

		outer_position = new Vector2(this.Position.X,this.Position.Y - (panel.Size.Y + border_sprite.Texture.GetHeight()));
		inner_position = this.Position; // this assumes the door in the scene starts on the inside
		lowest_position = new Vector2(this.Position.X, this.Position.Y + 452);

		this.Position = outer_position;

		SignalConnect.Instance.Connect(SignalConnect.SignalName.EditablePlayerReady, new Callable(this, "_OnEditablePlayerReady"));
	}

	private void _OnEditablePlayerReady()
	{
		Debug.Print("hi");
		Tween tween = GetTree().CreateTween();
		
		tween.TweenProperty(this, "position", lowest_position, .5);
		
		
	}
	private void _OptionSelected()
	{
		Tween tween = GetTree().CreateTween();
		
		tween.TweenProperty(this, "position", outer_position, .5);
		tween.Finished+=_OnRetracted;
	}
	private void _OnRetracted()
	{
		SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.EnemySelectPanelRetracted.ToString());
	}
}
