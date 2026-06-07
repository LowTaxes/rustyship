using Godot;
using System;

public partial class LootDropdown : Control
{
	
	Panel panel;
	Sprite2D border_sprite;

	Vector2 outer_position;
	Vector2 inner_position;
	Vector2 lowest_position;
	public int loot_taken_count = 0;
	
	public override void _Ready()
	{
		panel = GetChild<Panel>(0);
		border_sprite = GetChild<Sprite2D>(1);
	

		outer_position = new Vector2(this.Position.X,this.Position.Y - (panel.Size.Y + border_sprite.Texture.GetHeight()));
		inner_position = this.Position; // this assumes the door in the scene starts on the inside
		lowest_position = new Vector2(this.Position.X, this.Position.Y + 498);

		this.Position = outer_position;

		SignalConnect.Instance.Connect(SignalConnect.SignalName.EnemySelectPanelRetracted, new Callable(this, "_EnemySelectPanelRetracted"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.LootTaken, new Callable(this, "_LootTaken"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.ToBattle, new Callable(this, "_ToBattle"));
	}
   


	private void _EnemySelectPanelRetracted()
	{
		
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", lowest_position, .5);
		
		
	}

	private void _LootTaken()
	{
		loot_taken_count += 1;

		if(loot_taken_count >= 2)
		{
			border_sprite.Texture = ResourceLoader.Load<Texture2D>("uid://vcr7at747b4a");
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(this, "position", inner_position, .5);
		}
	}

	private void _ToBattle()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", outer_position, .5);
	}
	
	

}
