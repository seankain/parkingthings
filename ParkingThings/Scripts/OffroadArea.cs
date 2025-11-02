using Godot;
using System;

public partial class OffroadArea : Area3D
{
    private bool playerOffroading = false;
    private double secondsOffroading = 0;

    private Level level;

    public override void _Process(double delta)
    {
        if (playerOffroading)
        {
            this.level.levelData.OffroadingTime += delta;
            secondsOffroading += delta;
        }
    }
    public override void _Ready()
    {
        this.level = GetTree().Root.GetNode<Level>("/root/Main/Level");
        this.BodyEntered += (o) =>
        {

            if (o.IsInGroup("Player"))
            {
                playerOffroading = true;
                //GD.Print($"{o.Name} entered parking space");
               
            }
        };

        this.BodyExited += (o) =>
        {

            if (o.IsInGroup("Player"))
            {
                playerOffroading = false;
                // GD.Print($"{o.Name} exited parking space");
                // foreach (var p in this.GetOverlappingBodies())
                // {
                //     GD.Print($"overlapping body {p.Name}");
                // }       
            }
        };
    }
}
