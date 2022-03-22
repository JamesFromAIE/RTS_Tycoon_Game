using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] protected MeshRenderer _renderer;
    [SerializeField] private bool _isBuildable;

    public StructureBase OccupiedStructure;
    public bool Buildable => _isBuildable && OccupiedStructure == null;

    public virtual void Init(int x, int z)
    {
        
    }

    public void SetStructure(StructureBase structure)
    {
        if (structure.OccupiedTile != null) structure.OccupiedTile.OccupiedStructure = null;

        var offset = structure.transform.position;
        structure.transform.position = transform.position + offset;

        structure.OccupiedTile = this;

        var dimensions = structure.XZDimensions;
        for (int i = 0; i < dimensions.Length; i++)
        {
            var tilePos = Helper.XYToXZInt(dimensions[i]) +
                new Vector3Int((int)structure.transform.position.x, 0, (int)structure.transform.position.z);
            var tile = GridManager.Instance.GetTileAtPosition(tilePos);

            tile.OccupiedStructure = structure;
        }
    }

    public void UnassignStructure()
    {
        OccupiedStructure = null;
    }

    
}
