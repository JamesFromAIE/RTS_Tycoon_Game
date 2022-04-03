using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MinableTile : Tile
{
    public Resource MinedResource;
    [ReadOnly]
    public List<Worker> WorkerList;
    public float MineMaxTimer;
    [ReadOnly]
    public float MineTimer;
    public int ResourceWorth;

    public override void Init(int x, int z)
    {
        MineTimer = MineMaxTimer;

        Neighbours = GetWalkableNeighbours();
    }
}

public enum Resource
{
    Gold = 0,
    Stone = 1,
}
