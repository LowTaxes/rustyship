using Godot;
using System;
using Godot.Collections;
using System.Diagnostics;

public partial class ActiveInventoryItemBox : Control
{
	public InventoryItem attatched_item = null;
	public Control inventory_item_holder;
	
	public Area2D area2D;
	public override void _Ready()
	{
		inventory_item_holder = GetChild<Panel>(1).GetChild<Control>(0);
		area2D = GetChild<Area2D>(2);
	}

	
	public void UpdateItem(InventoryItem inv_item)
	{
		//Debug.Print(inventory_item_holder.Size.X.ToString());
		float scale = (float)inventory_item_holder.Size.Y / inv_item.sprite2D.Texture.GetHeight();
		inv_item.sprite2D.Scale = new Vector2(scale,scale);
		inv_item.Reparent(inventory_item_holder);
		inv_item.Position = new Vector2(inventory_item_holder.Size.X/2, inventory_item_holder.Size.Y/2);
		
	}

	public void SwapItem(InventoryItem inv_item)
	{
		
	}

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouse mouse_event)
		{
			if(mouse_event.IsActionReleased("left_click"))
			{
				
				Array<Area2D> over_lapping_areas = area2D.GetOverlappingAreas();
				for(int i = 0; i < over_lapping_areas.Count; i++)
				{
					if(over_lapping_areas[i].GetParent() is InventoryItem inv_item)
					{
						if(inv_item.mouse_dragging == true && attatched_item == null)
						{
							Debug.Print("hi");
							UpdateItem(inv_item);
						}
						else if(inv_item.mouse_dragging == true)
						{
							SwapItem(inv_item);
						}
					}
				}
			}
		}
	}
}
