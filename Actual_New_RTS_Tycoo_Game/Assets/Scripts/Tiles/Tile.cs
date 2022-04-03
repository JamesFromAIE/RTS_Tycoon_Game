using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Tile : MonoBehaviour
{
    public Tile Connection { get; private set; }
    public int G { get; private set; }
    public int H { get; private set; }
    public int F => G + H;

    public void SetConnection(Tile tile) => Connection = tile;

    public void SetG(int g) => G = g;

    public void SetH(int h) => H = h;

    public List<Tile> Neighbours;
    [SerializeField] protected MeshRenderer _renderer;
    [SerializeField] private bool _isBuildable;
    [ReadOnly]
    public StructureBase OccupiedStructure;
    [ReadOnly] public bool IsWalkable = true;
    [ReadOnly] public bool Buildable => _isBuildable && OccupiedStructure == null;

    public virtual void Init(int x, int z)
    {
        
    }

    public void SetEmptyStructure(StructureBase structure)
    {
        var offset = structure.transform.position;
        structure.transform.position = transform.position + offset;
    }

    public void SetStructure(StructureBase structure)
    {
        if (structure.OccupiedTile != null) structure.OccupiedTile.OccupiedStructure = null;

        var offset = structure.transform.position;
        structure.transform.position = transform.position + offset;

        structure.OccupiedTile = this;

        var dimensions = Helper.Vector2ToGridCoordinates(structure.XZDimensions);
        var walklableDimensions = Helper.GetWalkableCoordinates(structure.XZDimensions);
        var structPos = new Vector3Int((int)structure.transform.position.x, 0, (int)structure.transform.position.z);


        for (int i = 0; i < dimensions.Length; i++)
        {
            var tilePos = Helper.XYToXZInt(dimensions[i]) + structPos;

            var tile = GridManager.Instance.GetWalkableTileAtPosition(tilePos);

            tile.OccupiedStructure = structure;
            tile.IsWalkable = Helper.IsTileWalkable(walklableDimensions, tilePos, structPos);

        }
    }

    public void UnassignStructure()
    {
        OccupiedStructure = null;
    }

    public int GetMoveDistanceFromTile(Tile tile)
    {
        Vector3Int tilePos = Helper.CastV3ToInt(tile.transform.position);
        Vector3Int thisPos = Helper.CastV3ToInt(transform.position);

        var xDiff = Helper.Positive(thisPos.x - tilePos.x);
        var zDiff = Helper.Positive(thisPos.z - tilePos.z);

        var moveDist = xDiff + zDiff;
        return moveDist;
    }

    public List<Tile> GetWalkableNeighbours()
    {
        List<Tile> tempNeighbours = new List<Tile>();
        List<Tile> neighbours = new List<Tile>();

        var tilePos = Helper.CastV3ToInt(transform.position);

        var tile1 = GridManager.Instance.GetWalkableTileAtPosition(tilePos + new Vector3Int(1, 0, 0));
        var tile2 = GridManager.Instance.GetWalkableTileAtPosition(tilePos + new Vector3Int(0, 0, 1));
        var tile3 = GridManager.Instance.GetWalkableTileAtPosition(tilePos + new Vector3Int(-1, 0, 0));
        var tile4 = GridManager.Instance.GetWalkableTileAtPosition(tilePos + new Vector3Int(0, 0, -1));

        tempNeighbours.Add(tile1);
        tempNeighbours.Add(tile2);
        tempNeighbours.Add(tile3);
        tempNeighbours.Add(tile4);

        foreach (Tile tile in tempNeighbours)
        {
            if (tile)
            {
                neighbours.Add(tile);
            }

        }
        tempNeighbours.Clear();

        return neighbours;
    }


}
