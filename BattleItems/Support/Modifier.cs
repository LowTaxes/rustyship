using Godot;
using System;

public class Modifier
{
    public string support_type;
    public string modifier_type;
    public string weight_class_restriction;
    public double support_amount;
    public Hardpoint original_hardpoint;

    public Modifier (string support_type, string modifier_type, string weight_class_restriction, double support_amount, Hardpoint original_hardpoint)
    {
        this.support_type = support_type;
        this.modifier_type = modifier_type;
        this.weight_class_restriction = weight_class_restriction;
        this.support_amount = support_amount;
        this.original_hardpoint = original_hardpoint;
    }
}
