using Godot;
using System;

public partial class UpperPanel : Control
{
	Panel panel;
	Sprite2D border_sprite;
	bool has_been_lowered = false;



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

		Tween tween = GetTree().CreateTween();

		tween.TweenProperty(this, "position", inner_position, 1.5);
	}

	private void _OnShopButtonPressed()
	{
		Tween tween = GetTree().CreateTween();
		if (has_been_lowered)
		{
			tween.TweenProperty(this, "position", inner_position, 1);
			has_been_lowered = false;
		}
		else if(!has_been_lowered)
		{
			tween.TweenProperty(this, "position", lowest_position, 1);
			has_been_lowered = true;
		}
	}

	
}
