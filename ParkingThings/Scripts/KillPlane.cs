using Godot;
using System;

public partial class KillPlane : Area3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		this.BodyEntered += HandleBodyEntered;
    }

    private void HandleBodyEntered(Node3D body)
    {
        if(body is Player player)
        {
		player.PlayerRespawned?.Invoke(this,new());
         player.Respawn();

        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
