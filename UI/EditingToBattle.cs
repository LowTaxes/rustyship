using Godot;
using System;
using System.Diagnostics;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;

public partial class EditingToBattle : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _On_Pressed()
	{
		for(int i = 0; i < RunData.GetPlayerActiveInventoryItems().Count; i++)
		{
			Debug.Print(RunData.GetPlayerActiveInventoryItems()[i].weapon_name);
		}
		
		SignalConnect.Instance.EmitSignal(SignalConnect.SignalName.ChangeToNextScene.ToString());

		

		//Update Run Data

		Dictionary run_data = new Dictionary();
		run_data.Add("player", new Array
		{
			RunData.Instance.p_ship_template_id,
			RunData.Instance.p_health_m_count,
			RunData.Instance.p_armor_m_count,
			RunData.Instance.p_crit_chance_m_count,
			RunData.Instance.p_level,
			RunData.Instance.p_active_inv,
			RunData.Instance.p_storage_inv,
		});
		run_data.Add("enemy", new Array
		{
			RunData.Instance.e_ship_template_id,
			RunData.Instance.e_health_m_count,
			RunData.Instance.e_armor_m_count,
			RunData.Instance.e_crit_chance_m_count,
			RunData.Instance.e_level,
			RunData.Instance.e_active_inv,
		});
		RunData.Instance.SaveToUserData(Json.Stringify(run_data));


		Debug.Print("-----------------------");
		for(int i = 0; i < RunData.GetPlayerActiveInventoryItems().Count; i++)
		{
			Debug.Print(RunData.GetPlayerActiveInventoryItems()[i].weapon_name);
		}

		PackedScene battle_scene = ResourceLoader.Load<PackedScene>("uid://b7a2h0hw00vtn");
		GetTree().ChangeSceneToPacked(battle_scene);
	}
}
