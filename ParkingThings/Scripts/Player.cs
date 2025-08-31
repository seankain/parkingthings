using Godot;
using System;
public class PlayerRespawnArgs { }

public delegate void PlayerRespawnedEventHandler(object sender, PlayerRespawnArgs e);
public partial class Player : VehicleBody3D
{
    [Export]
    public float MAX_STEER = 0.9f;

    [Export]
    public float SteeringSpeed = 10.0f;
    [Export]
    public float ENGINE_POWER = 300;

    public PlayerRespawnedEventHandler PlayerRespawned;

    private bool respawnPressed = false;

    private bool inputPaused = false;

    //private double Steering = 0;
    //private double EngineForce = 0;

    public override void _PhysicsProcess(double delta)
    {
        if (inputPaused) { return; }
        Steering = Mathf.MoveToward(Steering, Input.GetAxis("Right", "Left") * MAX_STEER, (float)delta * SteeringSpeed);
        EngineForce = Input.GetAxis("Back", "Forward") * ENGINE_POWER;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
            {
                GetTree().Quit();
            }
            if (eventKey.Pressed && eventKey.Keycode == Key.R)
            {
                //Respawn();
                respawnPressed = true;
            }
        }
    }

    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        base._IntegrateForces(state);
        if (respawnPressed)
        {
            Respawn();
            respawnPressed = false;
        }

    }

    public void PauseInput()
    {
        inputPaused = true;
    }
    public void ResumeInput()
    {
        inputPaused = false;
    }

    private void Respawn()
    {
        var respawn = GetTree().GetNodesInGroup("Respawn")[0] as Node3D;
        this.GlobalTransform = new Transform3D(respawn.GlobalTransform.Basis, respawn.GlobalPosition);
        this.PlayerRespawned?.Invoke(this, new PlayerRespawnArgs());
        this.LinearVelocity = Vector3.Zero;
        this.AngularVelocity = Vector3.Zero;
        this.SetPhysicsProcess(true);
    }
}
