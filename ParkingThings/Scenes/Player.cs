using Godot;
using System;

public partial class Player : VehicleBody3D
{
    [Export]
    public float MAX_STEER = 0.9f;
    [Export]
    public float ENGINE_POWER = 300;

    //private double Steering = 0;
    //private double EngineForce = 0;

    public override void _PhysicsProcess(double delta)
    {
        Steering = Mathf.MoveToward(Steering, Input.GetAxis("Right", "Left") * MAX_STEER, (float)delta * 10);
        EngineForce = Input.GetAxis("Back", "Forward") * ENGINE_POWER;
    }
}
