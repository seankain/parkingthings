using Godot;
using System;

public partial class RagdollTest : Node3D
{
	[Export]
	public MobileNpc Npc;

	private bool ragdolled = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Keycode == Key.R)
            {
				if (ragdolled)
				{
					Npc.Unragdoll();
				}
				else
				{
					Npc.Ragdoll();
				}
				ragdolled = !ragdolled;
            }
        }
    }
}
