using Godot;
using System;
using System.Diagnostics;

using WeaponDataEnum = Constants.WeaponDataEnum;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;

public partial class Weapon : Node2D
{
	public string weapon_ID;
	public double damage;
	public double armor_damage_modifier;
	public double fire_rate;
	public double multi_shot_timespan;
	public int spread_radius = 1;
	public int volley_spread_radius = 1;
	public int volley_count = 1;
	public Timer volley_timer;
	public Timer multi_shot_timer;
	public PackedScene bullet_scene;
	public double bullet_speed;
	public int shots_fired = 0;

	public Vector2 other_ship_start_pos = new Vector2(0,0);
	public Vector2 cur_target_pos = new Vector2(0,0);
	public Vector2 old_target_pos = new Vector2(0,0);
	
	public int other_ship_width;
	public bool is_player = false;
	public bool is_active = false;

    public override void _Ready()
    {
        SignalConnect.Instance.Connect(SignalConnect.SignalName.WeaponsFree, new Callable(this, "_OnWeaponsFree"));
		volley_timer = new Timer();
		AddChild(volley_timer);
		volley_timer.WaitTime = fire_rate;
		
		multi_shot_timer = new Timer();
		AddChild(multi_shot_timer);
		multi_shot_timer.WaitTime = multi_shot_timespan;

		volley_timer.Timeout += _StartVolley;
		multi_shot_timer.Timeout += _OnMultiShotTimerTimeout;

		
    }


	public void InititializeWeaponConstants(string weapon_ID)
	{
		this.weapon_ID = weapon_ID;
		damage = ConstantData.GetWeaponDamage(weapon_ID);
		armor_damage_modifier = ConstantData.GetWeaponArmorDamageModifier(weapon_ID);
		fire_rate = ConstantData.GetWeaponFirerate(weapon_ID);
		multi_shot_timespan = ConstantData.GetWeaponMultishotTimespan(weapon_ID);
		bullet_scene = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponBulletUID(weapon_ID));
		bullet_speed = ConstantData.GetWeaponBulletSpeed(weapon_ID);
		volley_count = ConstantData.GetWeaponVolleyCount(weapon_ID);
		spread_radius = ConstantData.GetWeaponSpreadRadius(weapon_ID);
		volley_spread_radius = ConstantData.GetWeaponVolleySpreadRadius(weapon_ID);


	}
	

	public virtual void FireProjectile(){}

	private void _OnWeaponsFree()
	{
		cur_target_pos = new Vector2(this.GlobalPosition.X, other_ship_start_pos.Y);
		old_target_pos = cur_target_pos;
		volley_timer.Start();

	}

	private void _StartVolley()
	{
		shots_fired = 0;
		multi_shot_timer.Start();
	}

	private void _OnMultiShotTimerTimeout()
	{
		if(shots_fired < volley_count)
		{
			shots_fired+=1;
			FireProjectile();
			multi_shot_timer.Start();
		}
	}
	












}







    //OLD WEAPON SCRIPT
 /*
 public bool is_active = false;
	public bool is_player = false;
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
		
		SignalConnect.Instance.Connect(SignalConnect.SignalName.BattleStart, new Callable(this, "_OnBattleStart"));

		Sprite2D weapon_model = ResourceLoader.Load<PackedScene>(ConstantData.GetWeaponModelUID(weapon_name)).Instantiate<Sprite2D>();
		AddChild(weapon_model);
		
		global_target_look_at = new Vector2(GlobalPosition.X, other_ship_start_point.Y);
		LookAt(global_target_look_at);

		for(int i = 0; i < GetChildCount(); i++)
		{
			if(GetChild(i) is Timer)
			{
				fire_rate_timer = GetChild<Timer>(i);
			}
		}
		

		//All values stored in WeaponData file
		damage = (double) ((Array)ConstantData.WeaponData[weapon_name])[(int)WeaponDataEnum.DAMAGE];
		armor_damage_modifier = (double) ((Array)ConstantData.WeaponData[weapon_name])[(int)WeaponDataEnum.ARMOR_DAMAGE_MODIFIER];
	
		fire_rate = (double) ((Array)ConstantData.WeaponData[weapon_name])[(int)WeaponDataEnum.FIRE_RATE];
		bulletUID = ((Array)ConstantData.WeaponData[weapon_name])[(int)WeaponDataEnum.BULLET_UID].ToString();
		bullet_speed = (double) ((Array)ConstantData.WeaponData[weapon_name])[(int)WeaponDataEnum.BULLET_SPEED];
		spread_radius = (int) ((Array)ConstantData.WeaponData[weapon_name])[(int)WeaponDataEnum.SPREAD_RADIUS];
		
		bullet_scene = ResourceLoader.Load<PackedScene>(bulletUID);
	}
	

	private void _OnBattleStart()
	{
		
		fire_rate_timer.WaitTime = fire_rate;
		fire_rate_timer.Start();

		

		old_global_target_look_at = global_target_look_at;

		RandomNumberGenerator rng = new RandomNumberGenerator();
		int new_target_x = (int)rng.RandfRange((-other_ship_width/2) + other_ship_start_point.X, (other_ship_width/2)+other_ship_start_point.X);

		
		global_target_look_at = new Vector2(new_target_x, other_ship_start_point.Y);

		//Debug.Print("leftbound: " + ((-other_ship_width/2) + other_ship_start_point.X).ToString());
		//Debug.Print("rightbound: " + ((other_ship_width/2)+other_ship_start_point.X).ToString());

		Vector2 curr_look_direction = new Vector2(old_global_target_look_at.X - GlobalPosition.X, old_global_target_look_at.Y-GlobalPosition.Y);
		Vector2 new_look__direction = new Vector2(global_target_look_at.X - GlobalPosition.X, global_target_look_at.Y-GlobalPosition.Y);
		
		rotation_angle = curr_look_direction.AngleTo(new_look__direction);


		is_active = true;
	}

	private void _On_Ready_To_Fire()
	{
		//make lookat direction exact to smooth small errors in rotation
		LookAt(global_target_look_at);
		
		//Handle bullet firing
		Vector2 bullet_travel_direction = new Vector2(global_target_look_at.X - GlobalPosition.X, global_target_look_at.Y-GlobalPosition.Y);
		Projectile bullet = bullet_scene.Instantiate<Projectile>();
		GetNode("/root").AddChild(bullet);
		bullet.is_player = is_player;
		bullet.travel_direction = bullet_travel_direction.Normalized();
		bullet.speed = bullet_speed;
		bullet.damage = damage;
		bullet.armor_damage_modifier = armor_damage_modifier;
	
		bullet.is_player = is_player;
		bullet.Translate(GlobalPosition);
		
		//Generate a new position to fire at and rotate towards
		old_global_target_look_at = global_target_look_at;

		RandomNumberGenerator rng = new RandomNumberGenerator();
		int left_x_bound = (int)(old_global_target_look_at.X - spread_radius);
		int right_x_bound = (int)(old_global_target_look_at.X + spread_radius);

		//Ensure spread does not go past the other ship's boundaries
		if(left_x_bound < -other_ship_width/2 + other_ship_start_point.X)
		{
			left_x_bound = (int)(-other_ship_width/2 + other_ship_start_point.X);
			right_x_bound = (int)((-other_ship_width/2) + (spread_radius * 2) + other_ship_start_point.X);
		}

		if(right_x_bound > other_ship_width/2 + other_ship_start_point.X)
		{
			right_x_bound = (int)(other_ship_width/2 + other_ship_start_point.X);
			left_x_bound = (int)((other_ship_width/2) - (spread_radius * 2) + other_ship_start_point.X);
		}
		//Debug.Print("")

		int new_target_x = rng.RandiRange(left_x_bound, right_x_bound);
		global_target_look_at = new Vector2(new_target_x, other_ship_start_point.Y);

		Vector2 curr_look_direction = new Vector2(old_global_target_look_at.X - GlobalPosition.X, old_global_target_look_at.Y-GlobalPosition.Y);
		Vector2 new_look__direction = new Vector2(global_target_look_at.X - GlobalPosition.X, global_target_look_at.Y-GlobalPosition.Y);
		
		rotation_angle = curr_look_direction.AngleTo(new_look__direction);
		
	}
	*/

