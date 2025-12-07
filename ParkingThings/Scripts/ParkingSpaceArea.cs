using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
// TODO move all of this up to parking space which already has a reference to the area3d and can 
// listen to these events this is a needless subcomponent
public enum ParkingSpaceTriggerType
{
    Entry,
    Exit
}

public class ParkingSpaceTransitEventArgs
{
    public Player EnteringNode;
}

public delegate void ParkingSpaceEnteredEventHandler(object sender, ParkingSpaceTransitEventArgs e);
public delegate void ParkingSpaceExitedEventHandler(object sender, ParkingSpaceTransitEventArgs e);
public partial class ParkingSpaceArea : Area3D
{
    public event ParkingSpaceEnteredEventHandler ParkingSpaceEntered;
    public event ParkingSpaceExitedEventHandler ParkingSpaceExited;

    public bool SpaceHasNpcVehicle {get{return currentOccupyingBodies.Any(o=>o.IsInGroup("NpcVehicle"));}}
    private readonly HashSet<Node3D> currentOccupyingBodies =  [];

    protected virtual void OnParkingSpaceEntered(ParkingSpaceTransitEventArgs e)
    {
        ParkingSpaceEnteredEventHandler handler = ParkingSpaceEntered;
        handler?.Invoke(this, e);
    }

    protected virtual void OnParkingSpaceExited(ParkingSpaceTransitEventArgs e)
    {
        ParkingSpaceExitedEventHandler handler = ParkingSpaceExited;
        handler?.Invoke(this, e);
    }

    public ParkingSpaceTriggerType TriggerType;
    // Start is called before the first frame update

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.BodyEntered += (o) =>
        {
            currentOccupyingBodies.Add(o);
            if (o.IsInGroup("Player"))
            {
                //GD.Print($"{o.Name} entered parking space");
                OnParkingSpaceEntered(new ParkingSpaceTransitEventArgs { EnteringNode = o as Player });
            }
        };

        this.BodyExited += (o) =>
        {
            currentOccupyingBodies.Remove(o);
            if (o.IsInGroup("Player"))
            {
                // GD.Print($"{o.Name} exited parking space");
                // foreach (var p in this.GetOverlappingBodies())
                // {
                //     GD.Print($"overlapping body {p.Name}");
                // }
                OnParkingSpaceExited(new ParkingSpaceTransitEventArgs { EnteringNode = o as Player });
            }
        };
    }
}
