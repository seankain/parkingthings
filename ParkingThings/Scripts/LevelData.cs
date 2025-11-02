using System;
using System.Collections.Generic;
using System.Threading.Channels;
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
    private static char[] RankMap = { 'A', 'B', 'C', 'D', 'F' };
    //In seconds
    public double OffroadingTime = 0;
    public List<ObstacleType> CollisionEvents = new();

    public double ParkingAngle = 0;

    public double CenterDistance = 0;

    public bool OverLeftLine = false;
    public bool OverRightLine = false;

    public char Grade { get { return CalculateFinalGrade(); } }
    public int GradeAsNumeric { get { return CalculateRank(); } }
    
    private int CalculateRank()
    {
         var angleRankNum = (int)ParkingAngle;
        if (angleRankNum > 3)
        {
            angleRankNum = 3;
        }
        var distRankNum = 3;
        if (CenterDistance <= 0.5)
        {
            distRankNum = 0;
        }
        if (CenterDistance > 0.5 && CenterDistance <= 0.9) { distRankNum = 1; }
        if (CenterDistance > 0.9 && CenterDistance <= 1.5) { distRankNum = 2; }
        if (CenterDistance > 1.5 && CenterDistance <= 2.0) { distRankNum = 3; }


        // Distance rank is:
        // 0.5 A
        // 0.9 B
        // 1.5 C
        // 2.0 D
        // > 2.5 F
        var rank = (int)((distRankNum + angleRankNum) / 2);
        if (OverLeftLine) { rank += 1; }
        if (OverRightLine) { rank += 1; }
        // todo: do better to factor in collisions with cars, animals etc
        // should it be an automatic F? should different collisions be weighted differently?
        rank += CollisionEvents.Count;
        if (rank > 4) { rank = 4; }
        return rank;
    }

    private char CalculateFinalGrade()
    {
        var rank = CalculateRank();
        return RankMap[rank];
    }
}

public class LevelDefaults
{
    public static uint LevelDefaultTimeSeconds = 60;
    public static uint LevelDefaultMinTimeSeconds = 25;
    public static uint LevelTimeDecrement = 5;
    public static uint StartingVehicleCount = 5;
    public static uint VehicleIncrement = 2;
}