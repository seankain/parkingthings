using Godot;
using System;
using System.ComponentModel;
using System.Threading;

public partial class ParkingSpace : Node3D
{
    [Export]
    public PackedScene VehicleScene;

    [Export]
    private ParkingSpaceArea area;

    private Level level;

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

    [Export]
    public Area3D LeftLine;

    [Export]
    public Area3D RightLine;

    private bool overLeftLine = false;

    private bool overRightLine = false;

    //TODO delete
    private DebugHud debugHud;


    public override void _Ready()
    {
        area.ParkingSpaceEntered += OnParkingSpaceEntered;
        area.ParkingSpaceExited += OnParkingSpaceExited;
        LeftLine.BodyEntered += (node) => { if (node.IsInGroup("Player")) { overLeftLine = true; } };
        LeftLine.BodyExited += (node) => { if (node.IsInGroup("Player")) { overLeftLine = false; } };
        RightLine.BodyEntered += (node) => { if (node.IsInGroup("Player")) { overRightLine = true; } };
        RightLine.BodyExited += (node) => { if (node.IsInGroup("Player")) { overRightLine = false; } };
        level = GetNode<Level>("/root/Main/Level");
        debugHud = GetNode<DebugHud>("/root/Main/Level/DebugHud");
        base._Ready();
    }

    public override void _Process(double delta)
    {
        if (isScoring)
        {
            level.Score = CalculateCurrentParkingScore();
            //GD.Print(nodeToBeScored.EngineForce);
            // TODO magic number found through logging since car kind of idly rolls
            // Handle this in the actual car controller
            if (nodeToBeScored.LinearVelocity.Length() < 0.02)
            {
                isScoring = false;
                level.EndLevel();
            }
        }
    }

    private int CalculateCurrentParkingScore()
    {
        var scoreNodeForward = -nodeToBeScored.Transform.Basis.Z;
        var angle = scoreNodeForward.AngleTo(this.Transform.Basis.Z);
        var centerDist = nodeToBeScored.GlobalPosition.DistanceTo(this.GlobalPosition);
        level.levelData.CenterDistance = centerDist;
        // Angle rank is |angle-90.0|
        // 0 - A
        // 1 - B
        // 2 - C
        // 3 - D
        //>3 - F
        var angleConverted = Mathf.Abs(Mathf.RadToDeg(angle) - 90.0);
        level.levelData.ParkingAngle = angleConverted;
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
        if (overLeftLine) { rank += 1; }
        if (overRightLine) { rank += 1; }
        debugHud.AngleLabel.Text = $"Angle: {angleConverted} : {angleRankNum}";
        debugHud.CenterDistLabel.Text = $"{centerDist} : {distRankNum}";
        debugHud.OverLineLabel.Text = (overLeftLine || overRightLine).ToString();
        level.levelData.OverLeftLine = overLeftLine;
        level.levelData.OverRightLine = overRightLine;
        //GD.Print($"dist:{centerDist} angle:{Mathf.RadToDeg(angle)} {rank}");
        return rank;
    }


    private void OnParkingSpaceExited(object sender, ParkingSpaceTransitEventArgs e)
    {
        isScoring = false;
        level.PlayerInSpace = false;
        nodeToBeScored = null;
    }


    private void OnParkingSpaceEntered(object sender, ParkingSpaceTransitEventArgs e)
    {
        isScoring = true;
        level.PlayerInSpace = true;
        nodeToBeScored = e.EnteringNode;
    }

}
