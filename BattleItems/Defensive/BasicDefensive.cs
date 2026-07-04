using Godot;
using System;
using System.Diagnostics;
public partial class BasicDefensive : Defensive
{
    public override void Heal()
    {
        if(ConstantData.GetDefensiveHealingType(weapon_ID).Equals("health"))
        {
            SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.HealthHealed, healing, is_player);
        }
        else if(ConstantData.GetDefensiveHealingType(weapon_ID).Equals("shield"))
        {
            SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.ShieldHealed, healing, is_player);
        }
    }

	
}
