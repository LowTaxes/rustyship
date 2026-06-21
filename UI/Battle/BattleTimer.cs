using Godot;
using System;

public partial class BattleTimer : Label
{
	Timer second_timer;
	int time = 3;
	public override void _Ready()
	{
		this.Visible = false;
		second_timer = GetChild<Timer>(0);
		second_timer.WaitTime = .6;

		SignalConnect.Instance.Connect(SignalConnect.SignalName.StartBattleTimer, new Callable(this, "_OnStartBattleTimer"));
	}
	
	private void _OnStartBattleTimer()
	{
		second_timer.Start();
		this.Text = time.ToString();
		this.Visible = true;
		
	}
	
	private void _OnTimeout()
	{
		time -= 1;

		if(time > 0)
		{
			this.Text = time.ToString();
			second_timer.Start();
		}
		else if(time == 0)
		{
			this.Text = "Start!";
			second_timer.WaitTime = .8;
			second_timer.Start();
		}
		else if(time == -1)
		{
			time = 3;
			this.Visible = false;
			SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.WeaponsFree);
		}
		
	}
}
