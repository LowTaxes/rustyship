using Godot;
using System;
using System.Diagnostics;

using WeaponStats = Constants.WeaponStats;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;
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
	public Vector2 global_target_look_at;
	public Vector2 old_global_target_look_at;
	private float rotation_angle;
	private float speed_angle;
	private int spread_radius;
	
    

    public override void _PhysicsProcess(double delta)
    {
		
        if(is_active)
		{
		Rotate(rotation_angle/ (int)(fire_rate*Engine.PhysicsTicksPerSecond));
	
		}
    }


	

	public void Initialize()
	{
		
		BattleConnect.Instance.Connect(BattleConnect.SignalName.BattleStart, new Callable(this, "_OnBattleStart"));
		//Debug.Print(is_player.ToString());
		
		global_target_look_at = new Vector2(Position.X, other_ship_start_point.Y);
		LookAt(global_target_look_at);
		
		

		for(int i = 0; i < GetChildCount(); i++)
		{
			if(GetChild(i).GetType() == typeof(Timer))
			{
				fire_rate_timer = GetChild<Timer>(i);
			}
		}
		
		Dictionary WeaponData = StoredData.Instance.LoadData("WeaponData");
		Array weapon_data = (Array)WeaponData[weapon_name];

		//All values stored in WeaponData file
		damage = (double)weapon_data[(int)WeaponStats.DAMAGE];
		armor_damage_modifier = (double)weapon_data[(int)WeaponStats.ARMOR_DAMAGE_MODIFIER];
		crit_chance = (double)weapon_data[(int)WeaponStats.CRIT_CHANCE];
		fire_rate = (double)weapon_data[(int)WeaponStats.FIRE_RATE];
		bulletUID = weapon_data[(int)WeaponStats.BULLET_UID].ToString();
		bullet_speed = (double)weapon_data[(int)WeaponStats.BULLET_SPEED];
		spread_radius = (int)weapon_data[(int)WeaponStats.SPREAD_RADIUS];
		
		bullet_scene = ResourceLoader.Load<PackedScene>(bulletUID);
	}
	

	private void _OnBattleStart()
	{
		
		fire_rate_timer.WaitTime = fire_rate;
		fire_rate_timer.Start();

		

		old_global_target_look_at = global_target_look_at;

		RandomNumberGenerator rng = new RandomNumberGenerator();
		int new_target_x = rng.RandiRange(-other_ship_width/2, other_ship_width/2);
		global_target_look_at = new Vector2(new_target_x, other_ship_start_point.Y);

		Vector2 curr_look_direction = new Vector2(old_global_target_look_at.X - GlobalPosition.X, old_global_target_look_at.Y-GlobalPosition.Y);
		Vector2 new_look__direction = new Vector2(global_target_look_at.X - GlobalPosition.X, global_target_look_at.Y-GlobalPosition.Y);
		
		rotation_angle = curr_look_direction.AngleTo(new_look__direction);


		is_active = true;
	}

	private void _OnReadyToFire()
	{
		//make lookat direction exact to smooth small errors in rotation
		LookAt(global_target_look_at);
		
		//Handle bullet firing
		Vector2 bullet_travel_direction = new Vector2(global_target_look_at.X - GlobalPosition.X, global_target_look_at.Y-GlobalPosition.Y);
		Projectile bullet = bullet_scene.Instantiate<Projectile>();
		GetNode("/root/GameManager").AddChild(bullet);
		bullet.is_player = is_player;
		bullet.travel_direction = bullet_travel_direction.Normalized();
		bullet.speed = bullet_speed;
		bullet.damage = damage;
		bullet.armor_damage_modifier = armor_damage_modifier;
		bullet.crit_chance = crit_chance;
		bullet.Translate(GlobalPosition);
		
		//Generate a new position to fire at and rotate towards
		old_global_target_look_at = global_target_look_at;

		RandomNumberGenerator rng = new RandomNumberGenerator();
		int left_x_bound = (int)old_global_target_look_at.X - spread_radius;
		int right_x_bound = (int)old_global_target_look_at.X + spread_radius;

		//Ensure spread does not go past the other ship's boundaries
		if(left_x_bound < -other_ship_width/2)
		{
			left_x_bound = -other_ship_width/2;
			right_x_bound = (-other_ship_width/2) + (spread_radius * 2);
		}

		if(right_x_bound > other_ship_width/2)
		{
			right_x_bound = other_ship_width/2;
			left_x_bound = (other_ship_width/2) - (spread_radius * 2);
		}

		int new_target_x = rng.RandiRange(left_x_bound, right_x_bound);
		global_target_look_at = new Vector2(new_target_x, other_ship_start_point.Y);

		Vector2 curr_look_direction = new Vector2(old_global_target_look_at.X - GlobalPosition.X, old_global_target_look_at.Y-GlobalPosition.Y);
		Vector2 new_look__direction = new Vector2(global_target_look_at.X - GlobalPosition.X, global_target_look_at.Y-GlobalPosition.Y);
		
		rotation_angle = curr_look_direction.AngleTo(new_look__direction);
		
	}
	
}
