using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StructureManager : MonoBehaviour
{
    public static StructureManager Instance;
    public StructureBase _selectedStructure { get; private set; }
    

    private Dictionary<Vector3, StructureBase> _currStructures;

    [SerializeField] LayerMask _structureLayer;

    void Awake()
    {
        Instance = this;

        _currStructures = new Dictionary<Vector3, StructureBase>();
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameInThisState(GameManager.GameStates.GameResumed)) return;
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, _structureLayer))
            {
                var offset = new Vector3(0, 0.5f, 0);
                var hitPos = new Vector3(
                    (int)hit.transform.position.x,
                    (int)hit.transform.position.y,
                    (int)hit.transform.position.z) + offset;
                StructureBase hitStructure = GetStructureAtPosition(hitPos);

                Tile tile = hitStructure.OccupiedTile;
                RemoveStructureFromTiles(hitStructure, tile);
                UIManager.Instance.TriggerConstructionEvent();
            }
        }
    }

    #region Public Methods

    public void RemoveStructureFromTiles(StructureBase structure, Tile tile)
    {
        if (!structure || !tile) return;

        var sStats = structure.StructureStats;
        UIManager.Instance.SellStructure(sStats.GoldCost, sStats.StoneCost);

        var dimensions = Helper.Vector2ToGridCoordinates(structure.XZDimensions);
        for (int i = 0; i < dimensions.Length; i++)
        {
            var tilePos = Helper.XYToXZInt(dimensions[i]) +
                new Vector3Int((int)tile.transform.position.x, 0, (int)tile.transform.position.z);
            var occupiedTile = GridManager.Instance.GetBuildableTileAtPosition(tilePos);

            occupiedTile.UnassignStructure();
        }



        _currStructures.Remove(new Vector3(
            structure.transform.position.x, 
            structure.transform.position.y,
            structure.transform.position.z));

        if (structure.TryGetComponent(out BuildingBase bScript))
            UIManager.Instance.UpdateBuildingPopulation.RemoveListener(bScript.SendPopulation);
        else if (structure.TryGetComponent(out LandmarkBase lScript))
        {
            lScript.RemoveSelfFromNearbyBuildings();
            UIManager.Instance.UpdateBuildingPopulation.RemoveListener(lScript.CopySelfOnNearbyBuildings);
        }
            

        
        Destroy(structure.gameObject);

    }

    public StructureBase SpawnEmptyStructureOnTile(Tile tile)
    {
        if (!tile || !_selectedStructure) return null;

        var structPrefab = _selectedStructure;

        var spawnedStructure = Instantiate(structPrefab);
        tile.SetEmptyStructure(spawnedStructure);

        return spawnedStructure;
        
    }

    public void SpawnStructureOnTile(Tile tile)
    {
        if (!tile || !_selectedStructure) return;

        var structPrefab = _selectedStructure;

        var dimensions = Helper.Vector2ToGridCoordinates(structPrefab.XZDimensions);
        var sStats = structPrefab.StructureStats;

        if (GridManager.Instance.IsTileBuildable(tile, dimensions) && 
            UIManager.Instance.IsStructureBuyable(sStats.GoldCost, sStats.StoneCost))
        {
            var spawnedStructure = Instantiate(structPrefab);

            spawnedStructure.Placed();
            spawnedStructure.Constructed(); // RELOCATE THIS ONCE 'CONSTRUCTION' IS IMPLEMENTED
            UIManager.Instance.BuyStructure(sStats.GoldCost, sStats.StoneCost);
            tile.SetStructure(spawnedStructure);

            _currStructures[spawnedStructure.transform.position] = spawnedStructure;

            if (spawnedStructure.TryGetComponent(out BuildingBase bScript))
                UIManager.Instance.UpdateBuildingPopulation.AddListener(bScript.SendPopulation);
            else if (spawnedStructure.TryGetComponent(out LandmarkBase lScript))
                UIManager.Instance.UpdateBuildingPopulation.AddListener(lScript.CopySelfOnNearbyBuildings);


            //UIManager.Instance.SetPopulation(GetCurrentPopulation());
        }
        else
        {
            //var structure = tile.OccupiedStructure;
            //Debug.Log("Cannot Build Structure HERE!!!");
        }
        
    }


    public StructureBase GetStructureAtPosition(Vector3 pos)
    {
        if (_currStructures.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    public void SetSelectedStructure(StructureBase structure)
    {
        _selectedStructure = structure;
    }

    #endregion

    #region Private Methods


    #endregion
}
