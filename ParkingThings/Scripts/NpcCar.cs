using Godot;
using System;

public partial class NpcCar : Godot.VehicleBody3D
{
    [Export]
    public float MAX_STEER = 0.9f;

    [Export]
    public float SteeringSpeed = 10.0f;
    [Export]
    public float ENGINE_POWER = 300;

    public override void _PhysicsProcess(double delta)
    {
        //Steering = Mathf.MoveToward(Steering, Input.GetAxis("Right", "Left") * MAX_STEER, (float)delta * SteeringSpeed);
        //EngineForce = Input.GetAxis("Back", "Forward") * ENGINE_POWER;
    }

    public override void _Ready()
    {
        base._Ready();
        this.SetRandomColor();
    }


    public void SetRandomColor()
    {
        GD.Randomize();
        var bottom = GetNode<MeshInstance3D>("CollisionShape3D/BottomMesh");
        var top = GetNode<MeshInstance3D>("CollisionShape3D2/TopMesh");

        var material = bottom.GetSurfaceOverrideMaterial(0).Duplicate() as StandardMaterial3D;
        Color randomColor = new Color(GD.Randf(), GD.Randf(), GD.Randf());
        material.AlbedoColor = randomColor;
        bottom.SetSurfaceOverrideMaterial(0, material);
        top.SetSurfaceOverrideMaterial(0, material);
    }
}
