using System;
using System.Collections.Generic;
public enum ObstacleType
{
    Vehicle,
    Person,
    Wildlife,
    Curb,
    TrafficControl
}

public class LevelData
{
    public uint OffroadingIncidents = 0;
    public List<ObstacleType> CollisionEvents = new();

    public float ParkingAngle = 0;

    public bool OverLeftLine = false;
    public bool OverRightLIne = false;
}

public class LevelDefaults
{
    public static uint LevelDefaultTimeSeconds = 60;
    public static uint LevelDefaultMinTimeSeconds = 10;
    public static uint LevelTimeDecrement = 10;
    public static uint StartingVehicleCount = 5;
    public static uint VehicleIncrement = 5;
}