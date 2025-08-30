using Godot;
using System;

public partial class CameraControl : Node3D
{
    [Export]
    public float TiltMax = 75f;
    [Export]
    public float MouseSensitivity = 0.1f;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion)
        {
            var mouseMotionEvent = (InputEventMouseMotion)@event;
            var rot = this.Rotation;
            rot.X -= mouseMotionEvent.Relative.Y * MouseSensitivity;
            rot.X = Mathf.Clamp(this.Rotation.X, -TiltMax, TiltMax);
            rot.Y += -mouseMotionEvent.Relative.X * MouseSensitivity;
            this.Rotation = rot;
        }
    }



}
