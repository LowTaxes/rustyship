using Godot;
using System;

public partial class EnemyStats : Control
{
	public ProgressBar enemy_health_bar;
	public ProgressBar enemy_armor_bar;
	public override void _Ready()
	{
		SignalConnect.Instance.Connect(SignalConnect.SignalName.EnemyDamageTaken, new Callable(this, "_OnEnemyDamageTaken"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.BattleSequenceBegins, new Callable(this, "_OnBattleSequenceBegins"));
		

	}

	private void _OnBattleSequenceBegins()
	{
		enemy_health_bar = GetChild<ProgressBar>(0);
		enemy_armor_bar = GetChild<ProgressBar>(1);

		string enemy_ship_template_id = ConstantData.GetLevelShipTemplateID(RunData.GetLevelID());

		float enemy_max_health = ConstantData.GetShipTemplateHealth(enemy_ship_template_id) + (ConstantData.GetShipTemplateHealth(enemy_ship_template_id) * ConstantData.GetLevelHealthModifierCount(RunData.GetLevelID()) * Constants.health_modifier);
		float enemy_max_armor = ConstantData.GetShipTemplateArmor(enemy_ship_template_id) + (ConstantData.GetShipTemplateArmor(enemy_ship_template_id) * ConstantData.GetLevelArmorModifierCount(RunData.GetLevelID()) * Constants.armor_modifier);

		enemy_health_bar.MaxValue = enemy_max_health;
		enemy_health_bar.Value = enemy_health_bar.MaxValue;
		enemy_health_bar.GetChild<Label>(0).Text = enemy_health_bar.Value.ToString() + "/" + enemy_health_bar.MaxValue.ToString();

		enemy_armor_bar.MaxValue = enemy_max_armor;
		enemy_armor_bar.Value = enemy_armor_bar.MaxValue;
		enemy_armor_bar.GetChild<Label>(0).Text = enemy_armor_bar.Value.ToString() + "/" + enemy_armor_bar.MaxValue.ToString();
	}



	private void _OnEnemyDamageTaken(double damage, double armor_damage_modifier)
	{
		
		if(enemy_armor_bar.Value > 0)
		{
			enemy_armor_bar.Value-=damage*armor_damage_modifier;
			enemy_armor_bar.GetChild<Label>(0).Text = enemy_armor_bar.Value + "/" + enemy_armor_bar.MaxValue;
			
		}
		else if(enemy_health_bar.Value > 0)
		{
			enemy_health_bar.Value-=damage;
			enemy_health_bar.GetChild<Label>(0).Text = enemy_health_bar.Value + "/" + enemy_health_bar.MaxValue;
			
		}
		if(enemy_health_bar.Value<=0)
		{
			SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.StartNewLoop);
			
		}
	}
}
