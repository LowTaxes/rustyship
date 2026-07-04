using Godot;
using System;
using System.Diagnostics;

public partial class BattleItemInfo : Control
{
	Label damage_label;
	Label fire_rate_label;
	Label weight_class_label;

	public override void _Ready()
	{
		SignalConnect.Instance.Connect(SignalConnect.SignalName.HardpointClicked, new Callable(this, "_OnHardpointClicked"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.InvItemClicked, new Callable(this, "_OnInvItemClicked"));

		damage_label = GetChild<Label>(0);
		fire_rate_label = GetChild<Label>(1);
		weight_class_label = GetChild<Label>(2);


	}

	private void _OnHardpointClicked(Hardpoint hardpoint)
	{
		if(ConstantData.GetBattleItemDesignation(hardpoint.attatched_weaponID).Equals(Constants.WEAPON_DESIGNATION))
		{
			damage_label.Text = "Damage: " + ConstantData.GetWeaponDamage(hardpoint.attatched_weaponID) + "X" + ConstantData.GetWeaponVolleyCount(hardpoint.attatched_weaponID);
			damage_label.AddThemeColorOverride("font_color",Colors.Red);

			fire_rate_label.Text = "Fire Rate: " + ConstantData.GetWeaponFirerate(hardpoint.attatched_weaponID);
			weight_class_label.Text = "Weight_Class: " + ConstantData.GetWeaponWeightClass(hardpoint.attatched_weaponID);


		}
		else if(ConstantData.GetBattleItemDesignation(hardpoint.attatched_weaponID).Equals(Constants.DEFENSIVE_DESIGNATION))
		{
			damage_label.Text = "Damage: " + ConstantData.GetDefensiveHealing(hardpoint.attatched_weaponID) + "X" + ConstantData.GetDefensiveVolleyCount(hardpoint.attatched_weaponID);
			damage_label.AddThemeColorOverride("font_color", Colors.Green);

			fire_rate_label.Text = "Fire Rate: " + ConstantData.GetDefensiveFirerate(hardpoint.attatched_weaponID);
			weight_class_label.Text = "Weight_Class: " + ConstantData.GetDefensiveWeightClass(hardpoint.attatched_weaponID);
		}
		
	}

	private void _OnInvItemClicked(InventoryItem inv_item)
	{
		if(ConstantData.GetBattleItemDesignation(inv_item.weapon_name).Equals(Constants.WEAPON_DESIGNATION))
		{
			damage_label.Text = "Damage: " + ConstantData.GetWeaponDamage(inv_item.weapon_name) + "X" + ConstantData.GetWeaponVolleyCount(inv_item.weapon_name);
			damage_label.AddThemeColorOverride("font_color", Colors.Red);
			//Debug.Print("red: " + Colors.Red);

			fire_rate_label.Text = "Fire Rate: " + ConstantData.GetWeaponFirerate(inv_item.weapon_name);
			weight_class_label.Text = "Weight Class: " + ConstantData.GetWeaponWeightClass(inv_item.weapon_name);


		}
		else if(ConstantData.GetBattleItemDesignation(inv_item.weapon_name).Equals(Constants.DEFENSIVE_DESIGNATION))
		{
			damage_label.Text = "Healing: " + ConstantData.GetDefensiveHealing(inv_item.weapon_name) + "X" + ConstantData.GetDefensiveVolleyCount(inv_item.weapon_name);
			damage_label.AddThemeColorOverride("font_color", Colors.Green);
			//Debug.Print("GREEN: " + Colors.GRE);

			fire_rate_label.Text = "Fire Rate: " + ConstantData.GetDefensiveFirerate(inv_item.weapon_name);
			weight_class_label.Text = "Weight Class: " + ConstantData.GetDefensiveWeightClass(inv_item.weapon_name);
		}
	}

}
