using Godot;
using System;
using System.Diagnostics;

public partial class Defensive : Node2D
{
	
	public string weapon_ID;
	public double healing;
	public double fire_rate;
	public double multi_shot_timespan;
	public int volley_count = 1;
	public Timer volley_timer;
	public Timer multi_shot_timer;
	public int shots_fired = 0;
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


	public void InititializeDefensiveConstants(string weapon_ID)
	{
		this.weapon_ID = weapon_ID;
		
		healing = ConstantData.GetDefensiveHealing(weapon_ID);
		fire_rate = ConstantData.GetDefensiveFirerate(weapon_ID);
		multi_shot_timespan = ConstantData.GetDefensiveMultishotTimespan(weapon_ID);
		volley_count = ConstantData.GetDefensiveVolleyCount(weapon_ID);
		


	}
	

	public virtual void Heal(){}

	private void _OnWeaponsFree()
	{
		
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
			//Debug.Print("4");
			shots_fired+=1;
			Heal();
			multi_shot_timer.Start();
		}
	}
	









}
