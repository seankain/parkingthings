using Godot;
using System;
using System.Collections.Generic;

public partial class Spawner : Node
{
    [Export]
    public PackedScene NpcVehicleScene;

    [Export]
	PackedScene HumanNpcScene;

    private List<Node> npcVehicleNodes = new List<Node>();

    private List<Node> npcHumanNodes = new List<Node>();

    public void SpawnNpcHuman(Vector3 location,Vector3 rotation)
    {
        //SpawnNpc(HumanNpcScene.ResourcePath,location,rotation,npcHumanNodes);
        var p = GD.Load<PackedScene>(HumanNpcScene.ResourcePath);
        var npc = p.Instantiate();
        AddChild(npc);
        ((CharacterBody3D)npc).GlobalPosition = location;
        ((CharacterBody3D)npc).GlobalRotationDegrees = rotation;
        npcHumanNodes.Add(npc);
    }

    public void SpawnNpcVehicle(Vector3 location, Vector3 rotation)
    {
        //SpawnNpc(NpcVehicleScene.ResourcePath,location,rotation,npcVehicleNodes);
        var p = GD.Load<PackedScene>(NpcVehicleScene.ResourcePath);
        var npc = p.Instantiate();
        AddChild(npc);
        ((VehicleBody3D)npc).GlobalPosition = location;
        ((VehicleBody3D)npc).GlobalRotationDegrees = rotation;
        npcVehicleNodes.Add(npc);
    }

    private void SpawnNpc(string resourcePath,Vector3 location,Vector3 rotation,List<Node> nodePool)
    {
        var p = GD.Load<PackedScene>(resourcePath);
        var npc = p.Instantiate();
        if(npc is VehicleBody3D vehicle)
        {
            vehicle.GlobalPosition = location;
            vehicle.GlobalRotationDegrees = rotation;
            AddChild(npc);
            nodePool.Add(npc);
        }

        //((Node3D)npc).GlobalPosition = location;
        //((Node3D)npc).GlobalRotationDegrees = rotation;

    }

    public void ClearVehicles()
    {
        while (npcVehicleNodes.Count > 0)
        {

            //RemoveChild(npcNodes[0]);
            //npcNodes[0].QueueFree();
            npcVehicleNodes[0].Free();
            npcVehicleNodes.RemoveAt(0);
        }
    }
}
