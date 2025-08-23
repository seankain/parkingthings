using Godot;
using System;
using System.ComponentModel;

public partial class ParkingSpace : Node3D
{
    [Export]
    public PackedScene VehicleScene;

    [Export]
    private ParkingSpaceArea area;

    private Game game;

    [Export]
    public Node3D FrontRightPost;
    [Export]
    public Node3D FrontLeftPost;
    [Export]
    public Node3D RearRightPost;
    [Export]
    public Node3D RearLeftPost;

    public override void _Ready()
    {
        area.ParkingSpaceEntered += OnParkingSpaceEntered;
        area.ParkingSpaceExited += OnParkingSpaceExited;
        game = GetNode<Game>("/root/Game");
        base._Ready();
    }

    private void OnParkingSpaceExited(object sender, ParkingSpaceTransitEventArgs e)
    {
        throw new NotImplementedException();
    }


    private void OnParkingSpaceEntered(object sender, ParkingSpaceTransitEventArgs e)
    {
        throw new NotImplementedException();
    }

}
