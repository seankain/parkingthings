using Godot;
using System;

public partial class NpcRagdoll : Node3D
{
	[Export]
	public PhysicalBoneSimulator3D psb;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        psb.PhysicalBonesStartSimulation();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
