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

    private Player nodeToBeScored;

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
            GD.Print(nodeToBeScored.EngineForce);
            // TODO magic number found through logging since car kind of idly rolls
            // Handle this in the actual car controller
            if (nodeToBeScored.LinearVelocity.Length() < 0.02)
            {
                game.EndLevel();
            }
        }
    }

    private int CalculateCurrentParkingScore()
    {
        var scoreNodeForward = -nodeToBeScored.Transform.Basis.Z;
        var angle = scoreNodeForward.AngleTo(this.Transform.Basis.Z);
        var centerDist = nodeToBeScored.GlobalPosition.DistanceTo(this.GlobalPosition);
        // var fr_dist = nodeToBeScored.GlobalPosition.DistanceTo(this.FrontRightPost.GlobalPosition);
        // var fl_dist = nodeToBeScored.GlobalPosition.DistanceTo(this.FrontLeftPost.GlobalPosition);
        // var rr_dist = nodeToBeScored.GlobalPosition.DistanceTo(this.RearRightPost.GlobalPosition);
        // var rl_dist = nodeToBeScored.GlobalPosition.DistanceTo(this.RearLeftPost.GlobalPosition);
        // Angle rank is |angle-90.0|
        // 0 - A
        // 1 - B
        // 2 - C
        // 3 - D
        //>3 - F
        var angleConverted = Mathf.Abs(Mathf.RadToDeg(angle) - 90.0);
        var angleRankNum = (int)angleConverted;
        if (angleRankNum > 3)
        {
            angleRankNum = 3;
        }
        var distRankNum = 3;
        if (centerDist <= 0.5)
        {
            distRankNum = 0;
        }
        if (centerDist > 0.5 && centerDist <= 0.9) { distRankNum = 1; }
        if (centerDist > 0.9 && centerDist <= 1.5) { distRankNum = 2; }
        if (centerDist > 1.5 && centerDist <= 2.0) { distRankNum = 3; }


        // Distance rank is:
        // 0.5 A
        // 0.9 B
        // 1.5 C
        // 2.0 D
        // > 2.5 F
        // todo: factor in collisions with cars, animals etc
        var rank = (int)((distRankNum + angleRankNum) / 2);
        //GD.Print($"dist:{centerDist} angle:{Mathf.RadToDeg(angle)} {rank}");
        return rank;
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
