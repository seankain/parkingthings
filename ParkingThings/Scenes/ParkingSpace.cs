using Godot;
using System;

public partial class ParkingSpace : Node3D
{
    [Export]
    public PackedScene VehicleScene;

    private ParkingSpaceArea area;

    public override void _Ready()
    {
        base._Ready();
    }

}
