using Godot;
using System;

public partial class SignalConnect : Node
{
	public static SignalConnect Instance;
	
	//Combat Signals
	[Signal] public delegate void PlayerHealthDamageTakenEventHandler();
	[Signal] public delegate void PlayerArmorDamageTakenEventHandler();
	[Signal] public delegate void EnemyHealthDamageTakenEventHandler();
	[Signal] public delegate void EnemyArmorDamageTakenEventHandler();
	[Signal] public delegate void BattleStartEventHandler();


	//Ship Editing signals

	[Signal] public delegate void NewItemAttatchedEventHandler();
	
	public override void _Ready()
	{
		Instance = this;
	}

	
}
   
