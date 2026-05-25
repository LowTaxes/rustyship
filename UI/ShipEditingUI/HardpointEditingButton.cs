using Godot;
using System;

public partial class HardpointEditingButton : Button
{
	private void _on_toggled(bool is_toggled)
	{
		SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.CanEditHardpoints.ToString(), is_toggled);
	}

	
}
