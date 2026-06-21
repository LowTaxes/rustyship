using Godot;
using System;
using System.Diagnostics;

public partial class Cannons : Weapon
{
    
	public override void FireProjectile()
    {
        this.LookAt(cur_target_pos);
        RandomNumberGenerator rng = new RandomNumberGenerator();
        float bullet_target_x = rng.RandfRange(cur_target_pos.X-volley_spread_radius, cur_target_pos.X + volley_spread_radius);
        if(bullet_target_x < -other_ship_width/2)
        {
            bullet_target_x = -other_ship_width/2;
        }
        else if(bullet_target_x > other_ship_width/2)
        {
            bullet_target_x = other_ship_width/2;
        }
        Vector2 bullet_target = new Vector2(bullet_target_x,cur_target_pos.Y);

        Vector2 bullet_travel_direction = new Vector2(bullet_target.X - GlobalPosition.X, bullet_target.Y-GlobalPosition.Y);
		Projectile bullet = bullet_scene.Instantiate<Projectile>();
		GetNode("/root").AddChild(bullet);
        bullet.Position = this.GlobalPosition;
		bullet.is_player = is_player;
		bullet.travel_direction = bullet_travel_direction.Normalized();
		bullet.speed = bullet_speed;
		bullet.damage = damage;
		bullet.armor_damage_modifier = armor_damage_modifier;


        if(shots_fired == volley_count)
        {
            float new_target_pos_x = rng.RandfRange(cur_target_pos.X-spread_radius, cur_target_pos.X + spread_radius);
            if(new_target_pos_x < -other_ship_width/2)
            {
                new_target_pos_x = -other_ship_width/2;
            }
            else if(new_target_pos_x > other_ship_width/2)
            {
                new_target_pos_x = other_ship_width/2;
            }

            old_target_pos = cur_target_pos;
            cur_target_pos = new Vector2(new_target_pos_x,old_target_pos.Y);

            Tween tween = GetTree().CreateTween();
		    tween.TweenProperty(this, "rotation", this.Rotation + old_target_pos.AngleTo(cur_target_pos), fire_rate);
            
        }





    }
    

}
