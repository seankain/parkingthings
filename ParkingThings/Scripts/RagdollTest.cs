using Godot;
using System;

public partial class RagdollTest : Node3D
{
	[Export]
	public MobileNpc Npc;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        Npc.Ragdoll();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
