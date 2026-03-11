using Godot;
using System;
using System.Diagnostics;

public partial class Weapon : Sprite2D
{
	
	public int damage;
	public double armor_damage_modifier;
	public double crit_chance;
	public double fire_rate;
	public Timer fire_rate_timer;
	
	public string bulletUID;
	public PackedScene bullet_scene;
	public double bullet_speed;
	public bool is_active = false;
	public bool is_player = false;
	public string weapon_name;

    public override void _Process(double delta)
    {
        if(is_active)
		{
			Debug.Print(damage.ToString());
		}
    }


	public void Initialize(string weaon_name)
	{
		this.weapon_name = weaon_name;
		BattleConnect.Instance.Connect(BattleConnect.SignalName.BattleStart, new Callable(this, "_OnBattleStart"));
		//Debug.Print(is_player.ToString());
		if(is_player)
		{
			LookAt(new Vector2(Position.X, -1000));
			
		}
		else
		{
			LookAt(new Vector2(Position.X, 1000));
		}

		for(int i = 0; i < GetChildCount(); i++)
		{
			if(GetChild(i).GetType() == typeof(Timer))
			{
				fire_rate_timer = GetChild<Timer>(i);
			}
		}
		
		//bullet_scene = ResourceLoader.Load<PackedScene>(bulletUID);
		if (weapon_name.Equals("lightmachinegun"))
		{
			damage = 1;
			armor_damage_modifier = .25;
			crit_chance = .01;
			fire_rate = .5;
			bulletUID = "uid://cbdlciicv5vn6";
			Debug.Print(bulletUID);
			bullet_speed = 1;
		}

		if (weapon_name.Equals("mediumcannon"))
		{
			damage = 10;
			armor_damage_modifier = 1;
			crit_chance = .05;
			fire_rate = 6;
			bulletUID = "uid://cbdlciicv5vn6";
			bullet_speed = .5; 
		}
	}
	

	

	private void _OnBattleStart()
	{
		is_active = true;
		fire_rate_timer.Start();
		
	}

	private void _OnReadyToFire()
	{
		Debug.Print("yipee");
	}
	
}
