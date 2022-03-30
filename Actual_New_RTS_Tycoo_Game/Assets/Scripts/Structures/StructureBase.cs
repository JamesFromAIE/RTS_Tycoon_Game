using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBase : MonoBehaviour
{
    public Tile OccupiedTile;
    public Vector2Int XZDimensions;
    public ScriptableStructure StructureStats;
    public List<Transform> WorkerPoints;
    public List<Tile> WorkerTiles { get; private set; } = new List<Tile>();

    public virtual void Placed()
    {
        
    }

    public virtual void Constructed()
    {

    }
}
