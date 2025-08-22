using Godot;
using System;

public enum ParkingSpaceTriggerType
{
    Entry,
    Exit
}

public class ParkingSpaceTransitEventArgs { }

public delegate void ParkingSpaceEnteredEventHandler(object sender, ParkingSpaceTransitEventArgs e);
public delegate void ParkingSpaceExitedEventHandler(object sender, ParkingSpaceTransitEventArgs e);
public partial class ParkingSpaceArea : Area3D
{
    public event ParkingSpaceEnteredEventHandler ParkingSpaceEntered;
    public event ParkingSpaceExitedEventHandler ParkingSpaceExited;

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
            GD.Print("Vehicle entered parking space");

            OnParkingSpaceEntered(new ParkingSpaceTransitEventArgs());
        };

        this.BodyExited += (o) =>
        {
            GD.Print("Vehicle exited parking space");
            OnParkingSpaceExited(new ParkingSpaceTransitEventArgs());
        };
    }
}
