using Godot;
using System;

public partial class SpectatorCamera : Camera3D
{
    [Export]
    public float OffsetDistance = 5f;

    [Export]
    public float Height = 0.5f;
    private Node3D playerNode;

    private float currAngle = 0;
    public override void _Ready()
    {
        playerNode = GetNode<Node3D>("Player");
    }

    public void StartSpin()
    {
        var centerPoint = playerNode.GlobalPosition;
        var camPos = new Vector3(OffsetDistance * Mathf.Cos(centerPoint.X), centerPoint.Y + Height, OffsetDistance * Mathf.Sin(centerPoint.Z));

    }
}
