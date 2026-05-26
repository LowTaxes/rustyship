using Godot;
using System;

public partial class LeftDoor : Control
{
	private Sprite2D door_sprite;
	public override void _Ready()
	{
		door_sprite = GetChild<Sprite2D>(0);
		Vector2 outer_position = new Vector2(this.Position.X - (door_sprite.Texture.GetWidth() * door_sprite.Scale.X),this.Position.Y);
		Vector2 inner_position = this.Position; // this assumes the door in the scene starts on the inside

		this.Position = outer_position;

		Tween tween = GetTree().CreateTween();

		tween.TweenProperty(this, "position", inner_position, .5);
	}

	
}
