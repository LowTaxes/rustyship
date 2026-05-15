using Godot;
using System;
using System.Collections.Generic;

public partial class BattlePort : Control
{
	Control PlayerSlot;
	Control EnemySlot;
	
	public override void _Ready()
	{
		PlayerSlot = GetNode<Control>("PlayerSlot");
		EnemySlot = GetNode<Control>("EnemySlot");

		Vector2 global_player_spawn = new Vector2(this.GlobalPosition.X + PlayerSlot.Size.X/2, this.GlobalPosition.Y + PlayerSlot.Size.Y/2);
		Vector2 global_enemy_spawn = new Vector2(this.GlobalPosition.X + EnemySlot.Size.X/2, this.GlobalPosition.Y + EnemySlot.Size.Y/2);

		//spawn player and enemy models

		ShipModel player_ship_model = ResourceLoader.Load<PackedScene>(ConstantData.GetShipModelUID(RunData.GetPlayerShipTemplateID())).Instantiate<ShipModel>();
		PlayerSlot.AddChild(player_ship_model);
		player_ship_model.Position = new Vector2(PlayerSlot.Size.X/2, PlayerSlot.Size.Y/2);

		ShipModel enemy_ship_model = ResourceLoader.Load<PackedScene>(ConstantData.GetShipModelUID(ConstantData.GetLevelShipTemplateID(RunData.GetLevelID()))).Instantiate<ShipModel>();
		EnemySlot.AddChild(enemy_ship_model);
		enemy_ship_model.Position = new Vector2(EnemySlot.Size.X/2, EnemySlot.Size.Y/2);


		//spawn player weapons
		List<InventoryItem> player_active_weapons = RunData.GetPlayerActiveInventoryItems();
		for(int i = 0; i < player_active_weapons.Count; i++)
		{
			if(!player_active_weapons[i].weapon_name.Equals("empty"))
			{
				Weapon new_weapon = ResourceLoader.Load<PackedScene>("uid://dfk85q5d2j7o8").Instantiate<Weapon>();
				player_ship_model.hardpoints[i].AddChild(new_weapon);
				new_weapon.weapon_name = player_active_weapons[i].weapon_name;
				new_weapon.other_ship_width = enemy_ship_model.Texture.GetWidth();
				new_weapon.other_ship_start_point = global_enemy_spawn;
				new_weapon.ship_start_point = global_player_spawn;

				new_weapon.Initialize();
			}
		}
		

		//spawn enemy weapons

		List<InventoryItem> enemy_active_weapons = ConstantData.GetLevelEnemyActiveInventoryItems(RunData.GetLevelID());
		for(int i = 0; i < enemy_active_weapons.Count; i++)
		{
			if(!enemy_active_weapons[i].weapon_name.Equals("empty"))
			{
				Weapon new_weapon = ResourceLoader.Load<PackedScene>("uid://dfk85q5d2j7o8").Instantiate<Weapon>();
				enemy_ship_model.hardpoints[i].AddChild(new_weapon);
				new_weapon.weapon_name = enemy_active_weapons[i].weapon_name;
				new_weapon.other_ship_width = player_ship_model.Texture.GetWidth();
				new_weapon.other_ship_start_point = global_player_spawn;
				new_weapon.ship_start_point = global_enemy_spawn;

				new_weapon.Initialize();
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
