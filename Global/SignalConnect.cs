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

	[Signal] public delegate void HardpointAddedEventHandler();
	[Signal] public delegate void HardpointRemovedEventHandler();
	[Signal] public delegate void CanEditHardpointsEventHandler();
	[Signal] public delegate void HardpointInfoChangeEventHandler();

	//UI movement signals n shit

	[Signal] public delegate void EditablePlayerReadyEventHandler();
	[Signal] public delegate void EnemySelectPanelRetractedEventHandler();
	[Signal] public delegate void LootTakenEventHandler();
	[Signal] public delegate void HardpointBarDropdownEventHandler();
	
	[Signal] public delegate void ToBattleEventHandler();
	[Signal] public delegate void BattleSequenceBeginsEventHandler();
	[Signal] public delegate void StartBattleTimerEventHandler();
	[Signal] public delegate void WeaponsFreeEventHandler();

	[Signal] public delegate void StartNewLoopEventHandler();



	
	//Scene Swapping Signals
	[Signal] public delegate void ChangeToNextSceneEventHandler();
	public override void _Ready()
	{
		Instance = this;
	}

	
}
   
