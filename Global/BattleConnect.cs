using Godot;
using System;

public partial class BattleConnect : Node
{
	public static BattleConnect Instance;
	[Signal] public delegate void PlayerHealthDamageTakenEventHandler();
	[Signal] public delegate void PlayerArmorDamageTakenEventHandler();
	[Signal] public delegate void EnemyHealthDamageTakenEventHandler();
	[Signal] public delegate void EnemyArmorDamageTakenEventHandler();
	
	[Signal] public delegate void ManeuverabilitySetEventHandler();
	
	public override void _Ready()
	{
		Instance = this;
	}

	public void emitdamagesignal(string signal_name, int damage)
	{
		EmitSignal(signal_name, damage);
	}

}
   
