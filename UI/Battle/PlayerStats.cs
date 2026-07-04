using Godot;
using System;

public partial class PlayerStats : Control
{

	Vector2 outer_position;

	public TextureProgressBar shield_bar_lower;
	public TextureProgressBar health_bar;

	private double max_health;
	private double health;
	private double shield;
	public override void _Ready()
	{
		outer_position = this.Position;

		SignalConnect.Instance.Connect(SignalConnect.SignalName.PlayerDamageTaken, new Callable(this, "_OnPlayerDamageTaken"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.HealthHealed, new Callable(this, "_OnHealthHealed"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.ShieldHealed, new Callable(this, "_OnShieldHealed"));
		SignalConnect.Instance.Connect(SignalConnect.SignalName.BattleSequenceBegins, new Callable(this, "_OnBattleSequenceBegins"));

		shield_bar_lower = GetChild<TextureProgressBar>(0);
		health_bar = GetChild<TextureProgressBar>(1);

		max_health = 150;	//temp
		health = max_health; 
		shield = 0;
	}
	private void _OnBattleSequenceBegins()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(this.Position.X + 2300, this.Position.Y), .5);
	}

	private void _OnPlayerDamageTaken(double damage, double armor_damage_modifier)
	{
		double overflow = 0;
		if(shield > 0)
		{
			overflow = damage - shield;
			shield -= damage;
			if(shield < 0)
			{
				shield = 0;
			}
		}
		else
		{
			health -= damage;
		}
		if(overflow > 0)
		{
			health -= overflow;
		}
		if(health <= 0)
		{
			SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.StartNewLoop);
		}
		UpdateStatus();
	}

	private void _OnHealthHealed(double healing, bool is_player)
	{
		if(is_player)
		{
			if(health < max_health)
			{
				health += healing;
			}
			if(health > max_health)
			{
				health = max_health;
			}
		}
		UpdateStatus();
	}

	private void _OnShieldHealed(double healing, bool is_player)
	{
		if(is_player)
		{
			shield += healing;
		}
		UpdateStatus();
	}

	private void UpdateStatus()
	{
		//mode 1 if total vitality is less than max health
		if((health + shield <= max_health))
		{
			
			health_bar.Value = (int)((100/max_health) * health);
			shield_bar_lower.Value = (int)((100/max_health) * (health + shield));
		}
		else if ((health + shield) > max_health)
		{
			shield_bar_lower.Value = shield_bar_lower.MaxValue;
			health_bar.Value = (100 / (shield + health)) * health;
		}
	}
	
}
