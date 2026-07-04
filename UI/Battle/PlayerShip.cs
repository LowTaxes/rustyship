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
		SignalConnect.Instance.Connect(SignalConnect.SignalName.CreateWeapon, new Callable(this, "_OnCreateWeapon"));
	}

	private void _OnBattleSequenceBegins()
	{
		//Debug.Print("hi");
		PackedScene player_ship_model_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetShipModelUID(RunData.GetPlayerShipTemplateID()));
		ShipModel player_ship_model = player_ship_model_scene.Instantiate<ShipModel>();
		
		AddChild(player_ship_model);
		MoveChild(player_ship_model, 0);

		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(Constants.PLAYER_START_LOCATION.X,Constants.PLAYER_START_LOCATION.Y),.5);

		tween.Finished += _LoadInComplete;


		
	}

	private void _OnCreateWeapon(Hardpoint hardpoint_used)
	{
		if(ConstantData.GetBattleItemDesignation(hardpoint_used.attatched_weaponID).Equals(Constants.WEAPON_DESIGNATION))
			{
				PackedScene new_weapon_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponSceneUID(hardpoint_used.attatched_weaponID));
				Weapon new_weapon = new_weapon_scene.Instantiate<Weapon>();

				new_weapon.InititializeWeaponConstants(hardpoint_used.attatched_weaponID);
				new_weapon.other_ship_start_pos = Constants.ENEMY_START_LOCATION;
				AddChild(new_weapon);
				new_weapon.is_player = true;

				string level_ID = RunData.GetLevelID();
				string ship_template = ConstantData.GetLevelShipTemplateID(level_ID);
				string other_ship_UID = ConstantData.GetShipModelUID(ship_template);
				Sprite2D other_ship_sprite = ResourceLoader.Load<PackedScene>(other_ship_UID).Instantiate<Sprite2D>();
				new_weapon.other_ship_width = other_ship_sprite.Texture.GetWidth();

				new_weapon.Position = new Vector2(hardpoint_used.placement_position.X, hardpoint_used.placement_position.Y);
				new_weapon.LookAt(new Vector2(new_weapon.GlobalPosition.X, new_weapon.GlobalPosition.Y - 1));

				//apply weapon modifiers
				for(int i = 0; i < hardpoint_used.active_modifiers.Count; i++)
				{
					if(hardpoint_used.active_modifiers[i].weight_class_restriction.Equals(hardpoint_used.weight_class) || hardpoint_used.active_modifiers[i].weight_class_restriction.Equals("none"))
					{
						if(hardpoint_used.active_modifiers[i].support_type.Equals("damage"))
						{
							
							if(hardpoint_used.active_modifiers[i].modifier_type.Equals("flat"))
							{
								Debug.Print("before: " + new_weapon.damage.ToString());
								//Debug.Print("support amount: " + hardpoint_used.active_modifiers[i].support_amount.ToString());
								new_weapon.damage += hardpoint_used.active_modifiers[i].support_amount;
								Debug.Print("after: " + new_weapon.damage.ToString());
							}
							else if(hardpoint_used.active_modifiers[i].modifier_type.Equals("percentage"))
							{
								new_weapon.damage += hardpoint_used.active_modifiers[i].support_amount * new_weapon.damage;
							}
						}

						else if(hardpoint_used.active_modifiers[i].support_type.Equals("firerate"))
						{
							if(hardpoint_used.active_modifiers[i].modifier_type.Equals("percentage"))
							{
								new_weapon.fire_rate -= hardpoint_used.active_modifiers[i].support_amount * new_weapon.fire_rate;
							}
						}
					}
					
				}
			}
			else if(ConstantData.GetBattleItemDesignation(hardpoint_used.attatched_weaponID).Equals(Constants.DEFENSIVE_DESIGNATION))
			{
				PackedScene new_defensive_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetDefensiveBattleSceneUID(hardpoint_used.attatched_weaponID));
				Defensive new_defensive = new_defensive_scene.Instantiate<Defensive>();

				new_defensive.InititializeDefensiveConstants(hardpoint_used.attatched_weaponID);
				AddChild(new_defensive);
				new_defensive.is_player = true;
				new_defensive.Position = new Vector2(hardpoint_used.placement_position.X, hardpoint_used.placement_position.Y);
				
			}
			else if(ConstantData.GetBattleItemDesignation(hardpoint_used.attatched_weaponID).Equals(Constants.SUPPORT_DESIGNATION))
			{
				PackedScene new_support_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetSupportBattleSceneUID(hardpoint_used.attatched_weaponID));
				Node2D new_support = new_support_scene.Instantiate<Node2D>();
				AddChild(new_support);
				new_support.Position = new Vector2(hardpoint_used.placement_position.X, hardpoint_used.placement_position.Y);
			}
	}

	private void _LoadInComplete()
	{
		SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.StartBattleTimer);
	}


}
