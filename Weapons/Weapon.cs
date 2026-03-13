using Godot;
using System;
using System.Diagnostics;


public partial class Weapon : Sprite2D
{
	
	public double damage;
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
	public int other_ship_width;
	public Vector2 other_ship_start_point;
	public Vector2 ship_start_point;
	public Vector2 target_look_at;
	public Vector2 old_target_look_at;
	
	

    

    public override void _PhysicsProcess(double delta)
    {
        if(is_active)
		{
			
			Vector2 globalized_look_at = new Vector2(target_look_at.X - GlobalPosition.X, target_look_at.Y-GlobalPosition.Y);
			double angle = old_target_look_at.AngleTo(globalized_look_at);
			//Rotate((float)(angle*delta/fire_rate));
			
		}
    }



	public void Initialize()
	{
		
		BattleConnect.Instance.Connect(BattleConnect.SignalName.BattleStart, new Callable(this, "_OnBattleStart"));
		//Debug.Print(is_player.ToString());
		
		LookAt(new Vector2(Position.X, other_ship_start_point.Y));
		old_target_look_at = new Vector2(0, other_ship_start_point.Y);
		

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
			fire_rate = 1;
			bulletUID = "uid://cbdlciicv5vn6";
			Debug.Print(bulletUID);
			bullet_speed = 20;
		}

		if (weapon_name.Equals("mediumcannon"))
		{
			damage = 10;
			armor_damage_modifier = 1;
			crit_chance = .05;
			fire_rate = 2;
			bulletUID = "uid://cbdlciicv5vn6";
			bullet_speed = 20; 
		}

		bullet_scene = ResourceLoader.Load<PackedScene>(bulletUID);
	}
	

	

	private void _OnBattleStart()
	{
		
		fire_rate_timer.WaitTime = fire_rate;
		fire_rate_timer.Start();
		
		int look_x = GD.RandRange(-other_ship_width/2, other_ship_width/2);
		target_look_at = new Vector2(look_x-GlobalPosition.X, other_ship_start_point.Y);
		
		
		is_active = true;
	}

	private void _OnReadyToFire()
	{
		
		Rotate(old_target_look_at.AngleTo(target_look_at));
		
		int look_x = GD.RandRange(-other_ship_width/2, other_ship_width/2);
		old_target_look_at = target_look_at;
		target_look_at = new Vector2(look_x-GlobalPosition.X, other_ship_start_point.Y);
		
		
		
		//Debug.Print("after rotate: " + Rotation.ToString());
		//LookAt(new_look_at);
		//Debug.Print("after Lookat: " + Rotation.ToString());

		Vector2 bullet_travel_direction = new Vector2(target_look_at.X - GlobalPosition.X, target_look_at.Y-GlobalPosition.Y);
		Projectile bullet = bullet_scene.Instantiate<Projectile>();
		GetNode("/root/GameManager").AddChild(bullet);
		bullet.is_player = is_player;
		bullet.travel_direction = bullet_travel_direction.Normalized();
		bullet.speed = bullet_speed;
		bullet.damage = damage;
		bullet.armor_damage_modifier = armor_damage_modifier;
		bullet.crit_chance = crit_chance;
		bullet.Translate(GlobalPosition);
		
		
	}
	
}
