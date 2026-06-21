using Godot;
using System;

public partial class PlayerStats : Control
{
	public ProgressBar player_health_bar;
	public ProgressBar player_armor_bar;
	public override void _Ready()
	{
		SignalConnect.Instance.Connect(SignalConnect.SignalName.PlayerDamageTaken, new Callable(this, "_OnPlayerDamageTaken"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.BattleSequenceBegins, new Callable(this, "_OnBattleSequenceBegins"));
	}
	private void _OnBattleSequenceBegins()
	{
		player_health_bar = GetChild<ProgressBar>(0);
		player_armor_bar = GetChild<ProgressBar>(1);

		string player_ship_template_id = RunData.GetPlayerShipTemplateID();

		float player_max_health = ConstantData.GetShipTemplateHealth(player_ship_template_id) + (ConstantData.GetShipTemplateHealth(player_ship_template_id) * RunData.GetPlayerHealthModifierCount() * Constants.health_modifier);
		float player_max_armor = ConstantData.GetShipTemplateArmor(player_ship_template_id) + (ConstantData.GetShipTemplateArmor(player_ship_template_id) * RunData.GetPlayerArmorModifierCount() * Constants.armor_modifier);

		player_health_bar.MaxValue = player_max_health;
		player_health_bar.Value = player_health_bar.MaxValue;
		player_health_bar.GetChild<Label>(0).Text = player_health_bar.Value.ToString() + "/" + player_health_bar.MaxValue.ToString();

		player_armor_bar.MaxValue = player_max_armor;
		player_armor_bar.Value = player_armor_bar.MaxValue;
		player_armor_bar.GetChild<Label>(0).Text = player_armor_bar.Value.ToString() + "/" + player_armor_bar.MaxValue.ToString();
	}

	private void _OnPlayerDamageTaken(double damage, double armor_damage_modifier)
	{
		
		
		if(player_armor_bar.Value > 0)
		{
			player_armor_bar.Value-=damage*armor_damage_modifier;
			player_armor_bar.GetChild<Label>(0).Text = player_armor_bar.Value + "/" + player_armor_bar.MaxValue;
			
		}
		else if(player_health_bar.Value > 0)
		{
			player_health_bar.Value-=damage;
			player_health_bar.GetChild<Label>(0).Text = player_health_bar.Value + "/" + player_health_bar.MaxValue;
			
		}
		if(player_health_bar.Value<=0)
		{
			SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.StartNewLoop);
			
		}
	}
	
}
