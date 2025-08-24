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

    private Node3D nodeToBeScored;

    private bool isScoring = false;

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

    public override void _Process(double delta)
    {
        if (isScoring)
        {
            game.Score = CalculateCurrentParkingScore();
        }
    }

    private uint CalculateCurrentParkingScore()
    {
        var scoreNodeForward = -nodeToBeScored.Transform.Basis.Z;
        var angle = scoreNodeForward.AngleTo(this.Transform.Basis.Z);
        var fr_dist = nodeToBeScored.GlobalPosition.DistanceTo(this.FrontRightPost.GlobalPosition);
        var fl_dist = nodeToBeScored.GlobalPosition.DistanceTo(this.FrontLeftPost.GlobalPosition);
        var rr_dist = nodeToBeScored.GlobalPosition.DistanceTo(this.RearRightPost.GlobalPosition);
        var rl_dist = nodeToBeScored.GlobalPosition.DistanceTo(this.RearLeftPost.GlobalPosition);
        GD.Print($"RF:{fr_dist} LF:{fl_dist} RR:{rr_dist} RL:{rl_dist} angle:{angle}");
        // todo actually make a score
        return (uint)angle;
    }


    private void OnParkingSpaceExited(object sender, ParkingSpaceTransitEventArgs e)
    {
        isScoring = false;
        nodeToBeScored = null;
    }


    private void OnParkingSpaceEntered(object sender, ParkingSpaceTransitEventArgs e)
    {
        isScoring = true;
        nodeToBeScored = e.EnteringNode;
    }

}
