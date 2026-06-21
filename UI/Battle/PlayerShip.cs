using Godot;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
public partial class PlayerShip : Node2D
{
	Vector2 outer_position;
	public override void _Ready()
	{
		this.Position = new Vector2(this.Position.X, Constants.PLAYER_START_LOCATION.Y);
		outer_position = this.Position;
		
		SignalConnect.Instance.Connect(SignalConnect.SignalName.BattleSequenceBegins, new Callable(this, "_OnBattleSequenceBegins"));
	}

	private void _OnBattleSequenceBegins()
	{
		//Debug.Print("hi");
		PackedScene player_ship_model_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetShipModelUID(RunData.GetPlayerShipTemplateID()));
		ShipModel player_ship_model = player_ship_model_scene.Instantiate<ShipModel>();

		AddChild(player_ship_model);

	
		
		
		List<Hardpoint> active_hardpoints = RunData.GetPlayerActiveHardpoints();

		for(int i = 0; i < active_hardpoints.Count; i++)
		{
			if(ConstantData.GetBattleItemDesignation(active_hardpoints[i].attatched_weaponID).Equals(Constants.WEAPON_DESIGNATION))
			{
				PackedScene new_weapon_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponSceneUID(active_hardpoints[i].attatched_weaponID));
				Weapon new_weapon = new_weapon_scene.Instantiate<Weapon>();

				new_weapon.InititializeWeaponConstants(active_hardpoints[i].attatched_weaponID);
				new_weapon.other_ship_start_pos = Constants.ENEMY_START_LOCATION;
				AddChild(new_weapon);
				new_weapon.is_player = true;

				string level_ID = RunData.GetLevelID();
				string ship_template = ConstantData.GetLevelShipTemplateID(level_ID);
				string other_ship_UID = ConstantData.GetShipModelUID(ship_template);
				Sprite2D other_ship_sprite = ResourceLoader.Load<PackedScene>(other_ship_UID).Instantiate<Sprite2D>();
				new_weapon.other_ship_width = other_ship_sprite.Texture.GetWidth();

				new_weapon.Position = new Vector2(active_hardpoints[i].placement_position.X, active_hardpoints[i].placement_position.Y);
				new_weapon.LookAt(new Vector2(new_weapon.GlobalPosition.X, new_weapon.GlobalPosition.Y - 1));
			}
			else if(ConstantData.GetBattleItemDesignation(active_hardpoints[i].attatched_weaponID).Equals(Constants.DEFENSIVE_DESIGNATION))
			{
				PackedScene new_defensive_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetDefensiveBattleSceneUID(active_hardpoints[i].attatched_weaponID));
				Defensive new_defensive = new_defensive_scene.Instantiate<Defensive>();

				new_defensive.InititializeDefensiveConstants(active_hardpoints[i].attatched_weaponID);
				AddChild(new_defensive);
				new_defensive.is_player = true;
				new_defensive.Position = new Vector2(active_hardpoints[i].placement_position.X, active_hardpoints[i].placement_position.Y);
				
			}

		}
		

		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(Constants.PLAYER_START_LOCATION.X,Constants.PLAYER_START_LOCATION.Y), 1);

		tween.Finished += _LoadInComplete;


		
	}

	private void _LoadInComplete()
	{
		SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.StartBattleTimer);
	}


}
