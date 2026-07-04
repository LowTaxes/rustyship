using Godot;
using System;
using System.Collections.Generic;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
public partial class EnemyShip : Node2D
{
	Vector2 outer_position;
	public override void _Ready()
	{
		this.Position = new Vector2(this.Position.X, Constants.ENEMY_START_LOCATION.Y);
		outer_position = this.Position;
		
		SignalConnect.Instance.Connect(SignalConnect.SignalName.BattleSequenceBegins, new Callable(this, "_OnBattleSequenceBegins"));
	}


	private void _OnBattleSequenceBegins()
	{
		//Debug.Print("hi");
		PackedScene enemy_ship_model_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetShipModelUID(ConstantData.GetLevelShipTemplateID(RunData.GetLevelID())));
		ShipModel enemy_ship_model = enemy_ship_model_scene.Instantiate<ShipModel>();

		AddChild(enemy_ship_model);

	
		
		
		List<Hardpoint> active_hardpoints = ConstantData.GetLevelEnemyActiveHarpoints(RunData.GetLevelID());

		for(int i = 0; i < active_hardpoints.Count; i++)
		{
			PackedScene new_weapon_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponSceneUID(active_hardpoints[i].attatched_weaponID));
			Weapon new_weapon = new_weapon_scene.Instantiate<Weapon>();

			new_weapon.InititializeWeaponConstants(active_hardpoints[i].attatched_weaponID);
			new_weapon.other_ship_start_pos = Constants.PLAYER_START_LOCATION;
			AddChild(new_weapon);
			new_weapon.is_player = false;
			
			
			
			
			string other_ship_UID = ConstantData.GetShipModelUID(RunData.p_ship_template_id);
			Sprite2D other_ship_sprite = ResourceLoader.Load<PackedScene>(other_ship_UID).Instantiate<Sprite2D>();
			new_weapon.other_ship_width = other_ship_sprite.Texture.GetWidth();

			new_weapon.Position = new Vector2(active_hardpoints[i].placement_position.X, active_hardpoints[i].placement_position.Y);
			new_weapon.LookAt(new Vector2(new_weapon.GlobalPosition.X, new_weapon.GlobalPosition.Y + 1));
	
		}
		

		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(Constants.ENEMY_START_LOCATION.X,Constants.ENEMY_START_LOCATION.Y), .5);
		
	}

	
}
