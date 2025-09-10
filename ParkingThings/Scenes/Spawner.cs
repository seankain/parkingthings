using Godot;
using System;
using System.Collections.Generic;

public partial class Spawner : Node
{
    [Export]
    public PackedScene NpcVehicleScene;

    private List<Node> npcNodes = new List<Node>();

    public void SpawnNpcVehicle(Vector3 location, Vector3 rotation)
    {
        var p = GD.Load<PackedScene>(NpcVehicleScene.ResourcePath);
        var npc = p.Instantiate();
        AddChild(npc);
        GD.Print("Setting player position");
        ((Node3D)npc).GlobalPosition = location;
        ((Node3D)npc).GlobalRotation = rotation;
        npcNodes.Add(npc);
    }

    public void ClearVehicles()
    {
        while (npcNodes.Count > 0)
        {

            //RemoveChild(npcNodes[0]);
            //npcNodes[0].QueueFree();
            npcNodes[0].Free();
            npcNodes.RemoveAt(0);
        }
    }
}
