using Godot;
using System;

public partial class WeaponInfo : Control
{
	Control sprite_control;
	Sprite2D info_sprite;
	Label damage_label;
	Label firerate_label;
	Label armor_damage_label;
	Label volley_count_label;
	public override void _Ready()
	{
		sprite_control = GetChild<Control>(0);
		damage_label = GetChild<Label>(1);
		firerate_label = GetChild<Label>(2);
		armor_damage_label = GetChild<Label>(3);
		volley_count_label = GetChild<Label>(4);

		info_sprite = sprite_control.GetChild<Sprite2D>(0);
		info_sprite.Position = new Vector2(sprite_control.Size.X/2, sprite_control.Size.Y/2);

		damage_label.Text = "";
		firerate_label.Text = "";
		armor_damage_label.Text = "";
		volley_count_label.Text = "";

		//SignalConnect.Instance.Connect(SignalConnect.SignalName.WeaponInfoChange, new Callable(this, "_WeaponInfoChange"));

	}

	private void _WeaponInfoChange(string weaponID)
	{
		info_sprite.Texture = ResourceLoader.Load<Texture2D>(ConstantData.GetWeaponInfoSpriteUID(weaponID));
		damage_label.Text = "Damage: " + ConstantData.GetWeaponDamage(weaponID).ToString();
		firerate_label.Text = "Firerate: " + (1/ConstantData.GetWeaponFirerate(weaponID)).ToString();
		armor_damage_label.Text = "Armor Damage: " + (ConstantData.GetWeaponArmorDamageModifier(weaponID) * 100).ToString() + "%";
		volley_count_label.Text = "Per Volley: " + ConstantData.GetVolleyCount(weaponID).ToString();


	}

	
}
