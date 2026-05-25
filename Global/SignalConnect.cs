using Godot;
using System;

public partial class SignalConnect : Node
{
	public static SignalConnect Instance;
	
	//Combat Signals
	[Signal] public delegate void PlayerDamageTakenEventHandler();
	[Signal] public delegate void EnemyDamageTakenEventHandler();
	[Signal] public delegate void BattleStartEventHandler();


	//Ship Editing signals

	[Signal] public delegate void InvItemClickedEventHandler();
	[Signal] public delegate void InvItemReleasedEventHandler();
	[Signal] public delegate void ActiveItemAddedEventHandler();
	[Signal] public delegate void ActiveItemRemovedEventHandler();
	[Signal] public delegate void HardpointAddedEventHandler();
	[Signal] public delegate void HardpointRemovedEventHandler();
	[Signal] public delegate void CanEditHardpointsEventHandler();
	[Signal] public delegate void WeaponInfoChangeEventHandler();



	
	//Scene Swapping Signals
	[Signal] public delegate void ChangeToNextSceneEventHandler();
	public override void _Ready()
	{
		Instance = this;
	}

	
}
   
