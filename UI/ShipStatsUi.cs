using Godot;
using System;
using System.Diagnostics;

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

		player_health_bar.MaxValue = PlayerStats.Instance.max_health;
		player_health_bar.Value = PlayerStats.Instance.health;
		player_health_bar.GetNode<Label>("HealthText").Text = player_health_bar.Value.ToString() + "/" + player_health_bar.MaxValue.ToString();

		player_armor_bar.MaxValue = PlayerStats.Instance.max_armor;
		player_armor_bar.Value = PlayerStats.Instance.armor;
		player_armor_bar.GetNode<Label>("ArmorText").Text = player_armor_bar.Value.ToString() + "/" + player_armor_bar.MaxValue.ToString();

		enemy_health_bar.MaxValue = EnemyStats.Instance.max_health;
		enemy_health_bar.Value = EnemyStats.Instance.health;
		enemy_health_bar.GetNode<Label>("HealthText").Text = enemy_health_bar.Value.ToString() + "/" + enemy_health_bar.MaxValue.ToString();

		enemy_armor_bar.MaxValue = EnemyStats.Instance.max_armor;
		enemy_armor_bar.Value = EnemyStats.Instance.armor;
		enemy_armor_bar.GetNode<Label>("ArmorText").Text = enemy_armor_bar.Value.ToString() + "/" + enemy_armor_bar.MaxValue.ToString();
		

		BattleConnect.Instance.Connect(BattleConnect.SignalName.PlayerHealthDamageTaken, new Callable(this, "_OnPlayerHealthDamageTaken"));
		BattleConnect.Instance.Connect(BattleConnect.SignalName.PlayerArmorDamageTaken, new Callable(this, "_OnPlayerArmorDamageTaken"));

		BattleConnect.Instance.Connect(BattleConnect.SignalName.EnemyHealthDamageTaken, new Callable(this, "_OnEnemyHealthDamageTaken"));
		BattleConnect.Instance.Connect(BattleConnect.SignalName.EnemyArmorDamageTaken, new Callable(this, "_OnEnemyArmorDamageTaken"));
		
    }
	

	private void _OnPlayerHealthDamageTaken(int damage)
	{
		player_health_bar.Value -= damage;
		player_health_bar.GetNode<Label>("HealthText").Text = player_health_bar.Value.ToString() + "/" + player_health_bar.MaxValue.ToString();
	}
	private void _OnPlayerArmorDamageTaken(int damage)
	{
		player_armor_bar.Value -= damage;
		player_armor_bar.GetNode<Label>("ArmorText").Text = player_armor_bar.Value.ToString() + "/" + player_armor_bar.MaxValue.ToString();
	}

	private void _OnEnemyHealthDamageTaken(int damage)
	{
		enemy_health_bar.Value -= damage;
		enemy_health_bar.GetNode<Label>("HealthText").Text = enemy_health_bar.Value.ToString() + "/" + enemy_health_bar.MaxValue.ToString();
	}
	private void _OnEnemyArmorDamageTaken(int damage)
	{
		//Debug.Print("hi");
		enemy_armor_bar.Value -= damage;
		enemy_armor_bar.GetNode<Label>("ArmorText").Text = enemy_armor_bar.Value.ToString() + "/" + enemy_armor_bar.MaxValue.ToString();
	}
}
