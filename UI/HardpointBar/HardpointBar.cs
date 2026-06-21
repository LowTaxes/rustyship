using Godot;
using System;

public partial class HardpointBar : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Label l = new Label();
		l.Text = "wassssssup";
		AddChild(l);
		l.Position = new Vector2(2000, 0);

		CustomMinimumSize = new Vector2(200,240);
	}

	
}
