using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] protected MeshRenderer _renderer;
    [SerializeField] private bool _isBuildable;

    public StructureBase OccupiedStructure;
    public bool IsWalkable = true;
    public bool Buildable => _isBuildable && OccupiedStructure == null;

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

            var tile = GridManager.Instance.GetBuildableTileAtPosition(tilePos);

            tile.OccupiedStructure = structure;
            tile.IsWalkable = Helper.IsTileWalkable(walklableDimensions, tilePos, structPos);

        }
    }

    public void UnassignStructure()
    {
        OccupiedStructure = null;
    }

    
}
