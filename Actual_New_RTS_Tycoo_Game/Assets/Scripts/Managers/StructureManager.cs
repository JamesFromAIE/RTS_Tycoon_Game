using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public static StructureManager Instance;
    public StructureBase _selectedStructure { get; private set; }

    private List<ScriptableStructure> _structures;
    private Dictionary<Vector3, StructureBase> _currStructures;

    [SerializeField] LayerMask _structureLayer;

    void Awake()
    {
        Instance = this;

        _structures = Resources.LoadAll<ScriptableStructure>("Structures").ToList();
        _currStructures = new Dictionary<Vector3, StructureBase>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
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
            }
        }
    }

    public void SpawnBuilding()
    {
        var structureCount = 10;

        for (int i = 0; i < structureCount; i++)
        {
            var randomPrefab = GetRandomStructure<BuildingBase>(StructureType.Building);
            var spawnedBuilding = Instantiate(randomPrefab);
            var dimensions = spawnedBuilding.XZDimensions;
            var randomSpawnTile = GridManager.Instance.GetRandomStructureSpawnTile(dimensions);

            randomSpawnTile.SetStructure(spawnedBuilding);

        }
    }

    public void RemoveStructureFromTiles(StructureBase structure, Tile tile)
    {
        if (!structure || !tile) return;

        var dimensions = structure.XZDimensions;

        for (int i = 0; i < dimensions.Length; i++)
        {
            var tilePos = Helper.XYToXZInt(dimensions[i]) +
                new Vector3Int((int)tile.transform.position.x, 0, (int)tile.transform.position.z);
            var occupiedTile = GridManager.Instance.GetTileAtPosition(tilePos);

            occupiedTile.UnassignStructure();
        }

        _currStructures.Remove(new Vector3(
            structure.transform.position.x, 
            structure.transform.position.y,
            structure.transform.position.z));
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
        
        var dimensions = structPrefab.XZDimensions;

        if (GridManager.Instance.IsTileBuildable(tile, dimensions))
        {
            var spawnedStructure = Instantiate(structPrefab);
            tile.SetStructure(spawnedStructure);

            _currStructures[spawnedStructure.transform.position] = spawnedStructure;
        }
        else
        {
            //var structure = tile.OccupiedStructure;
            Debug.Log("Cannot Build Structure HERE!!!");
        }
        
    }

    public void SpawnLandmark()
    {
        var landmarkCount = 20;

        for (int i = 0; i < landmarkCount; i++)
        {
            var randomPrefab = GetRandomStructure<LandmarkBase>(StructureType.Landmark);
            var spawnedLankmark = Instantiate(randomPrefab);
            var dimensions = spawnedLankmark.XZDimensions;
            var randomSpawnTile = GridManager.Instance.GetRandomStructureSpawnTile(dimensions);

            randomSpawnTile.SetStructure(spawnedLankmark);
        }
    }

    private T GetRandomStructure<T>(StructureType sType) where T : StructureBase
    {
        return (T)_structures.Where(u => u.SType == sType).OrderBy(o => Random.value).First().StructurePrefab;

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

}
