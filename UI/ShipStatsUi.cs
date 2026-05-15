using Godot;
using System;
using System.Diagnostics;
using Array = Godot.Collections.Array;
using RunDataEnum = Constants.RunDataEnum;
using ShipDataEnum = Constants.ShipDataEnum;
public partial class ShipStatsUi : Control
{
	public ProgressBar player_health_bar;
	public ProgressBar player_armor_bar;

	public ProgressBar enemy_health_bar;
	public ProgressBar enemy_armor_bar;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		

		player_health_bar = GetNode<ProgressBar>("PlayerStats/PlayerHealthbar");
		player_armor_bar = GetNode<ProgressBar>("PlayerStats/PlayerArmorbar");

		enemy_health_bar = GetNode<ProgressBar>("EnemyStats/EnemyHealthbar");
		enemy_armor_bar = GetNode<ProgressBar>("EnemyStats/EnemyArmorbar");

		Debug.Print("levelID: " + RunData.GetLevelID());
		string player_ship_template_id = RunData.GetPlayerShipTemplateID();
		string enemy_ship_template_id = ConstantData.GetLevelShipTemplateID(RunData.GetLevelID());

		float player_max_health = ConstantData.GetShipTemplateHealth(player_ship_template_id) + (ConstantData.GetShipTemplateHealth(player_ship_template_id) * RunData.GetPlayerHealthModifierCount() * Constants.health_modifier);
		float player_max_armor = ConstantData.GetShipTemplateArmor(player_ship_template_id) + (ConstantData.GetShipTemplateArmor(player_ship_template_id) * RunData.GetPlayerArmorModifierCount() * Constants.armor_modifier);

		float enemy_max_health = ConstantData.GetShipTemplateHealth(enemy_ship_template_id) + (ConstantData.GetShipTemplateHealth(enemy_ship_template_id) * ConstantData.GetLevelHealthModifierCount(RunData.GetLevelID()) * Constants.health_modifier);
		float enemy_max_armor = ConstantData.GetShipTemplateArmor(enemy_ship_template_id) + (ConstantData.GetShipTemplateArmor(enemy_ship_template_id) * ConstantData.GetLevelArmorModifierCount(RunData.GetLevelID()) * Constants.armor_modifier);

		player_health_bar.MaxValue = player_max_health;
		player_health_bar.Value = player_health_bar.MaxValue;
		player_health_bar.GetNode<Label>("HealthText").Text = player_health_bar.Value.ToString() + "/" + player_health_bar.MaxValue.ToString();

		player_armor_bar.MaxValue = player_max_armor;
		player_armor_bar.Value = player_armor_bar.MaxValue;
		player_armor_bar.GetNode<Label>("ArmorText").Text = player_armor_bar.Value.ToString() + "/" + player_armor_bar.MaxValue.ToString();

		enemy_health_bar.MaxValue = enemy_max_health;
		enemy_health_bar.Value = enemy_health_bar.MaxValue;
		enemy_health_bar.GetNode<Label>("HealthText").Text = enemy_health_bar.Value.ToString() + "/" + enemy_health_bar.MaxValue.ToString();

		enemy_armor_bar.MaxValue = enemy_max_armor;
		enemy_armor_bar.Value = enemy_armor_bar.MaxValue;
		enemy_armor_bar.GetNode<Label>("ArmorText").Text = enemy_armor_bar.Value.ToString() + "/" + enemy_armor_bar.MaxValue.ToString();
		

		SignalConnect.Instance.Connect(SignalConnect.SignalName.PlayerHealthDamageTaken, new Callable(this, "_OnPlayerHealthDamageTaken"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.PlayerArmorDamageTaken, new Callable(this, "_OnPlayerArmorDamageTaken"));

		SignalConnect.Instance.Connect(SignalConnect.SignalName.EnemyHealthDamageTaken, new Callable(this, "_OnEnemyHealthDamageTaken"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.EnemyArmorDamageTaken, new Callable(this, "_OnEnemyArmorDamageTaken"));
		
    }
	

	private void _OnPlayerHealthDamageTaken(double damage)
	{
		player_health_bar.Value -= damage;
		player_health_bar.GetNode<Label>("HealthText").Text = player_health_bar.Value.ToString() + "/" + player_health_bar.MaxValue.ToString();
	}
	private void _OnPlayerArmorDamageTaken(double damage)
	{
		player_armor_bar.Value -= damage;
		player_armor_bar.GetNode<Label>("ArmorText").Text = player_armor_bar.Value.ToString() + "/" + player_armor_bar.MaxValue.ToString();
	}

	private void _OnEnemyHealthDamageTaken(double damage)
	{
		enemy_health_bar.Value -= damage;
		enemy_health_bar.GetNode<Label>("HealthText").Text = enemy_health_bar.Value.ToString() + "/" + enemy_health_bar.MaxValue.ToString();
	}
	private void _OnEnemyArmorDamageTaken(double damage)
	{
		//Debug.Print("hi");
		enemy_armor_bar.Value -= damage;
		enemy_armor_bar.GetNode<Label>("ArmorText").Text = enemy_armor_bar.Value.ToString() + "/" + enemy_armor_bar.MaxValue.ToString();
	}
}
