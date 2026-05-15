using Godot;
using System;
using System.Diagnostics;

public partial class BattleStartButton : Button
{
	private void _OnButtonPressed()
	{
		Debug.Print("battle start");
		SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.BattleStart);
		Disabled = true;
		Visible = false;

	}
}
