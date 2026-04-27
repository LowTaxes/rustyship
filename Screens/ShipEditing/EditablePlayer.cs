using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Array = Godot.Collections.Array;
using System.Diagnostics;

public partial class EditablePlayer : Control
{
	Sprite2D sprite2D;
	
	Vector2[] hardpoint_position_scales;
	public override void _Ready()
	{
		sprite2D = GetChild<Sprite2D>(0);

		Dictionary run_data = RunData.Instance.LoadUserData();
		Array player_data = (Array) run_data["player"];
		Array ship_data = (Array)ConstantData.ShipData[player_data[(int)Constants.RunDataEnum.SHIP_TEMPLATE_ID].ToString()];

		sprite2D.Texture = GD.Load<Texture2D>(ship_data[(int)Constants.ShipDataEnum.SHIP_MODEL_UID].ToString());
		sprite2D.Position = new Vector2(this.Size.X/2, Size.Y/2);

		float scale = this.Size.X/sprite2D.Texture.GetWidth();
		Debug.Print(this.Size.X.ToString());
		Debug.Print(scale.ToString());
		sprite2D.Scale = new Vector2(sprite2D.Texture.GetWidth() * scale, sprite2D.Texture.GetHeight() * scale);



	}

	
}
