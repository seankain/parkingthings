using System;
using System.Collections.Generic;
using Godot;

public class NodePool<T>() where T : Node3D
{
    public Queue<T> existingNodes = new();
    public void Stow(){}
    public T? Recall()
    {
        while(existingNodes.TryDequeue(out var e))
        {
            if (!e.IsVisibleInTree())
            {
                e.Show();
                e.SetProcess(true);
                e.SetPhysicsProcess(true);
                break;
            }
            else
            {
                existingNodes.Enqueue(e);
            }
        }
        return null;
    }
}