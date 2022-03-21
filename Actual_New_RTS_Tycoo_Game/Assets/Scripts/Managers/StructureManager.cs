using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public static StructureManager Instance;

    private List<ScriptableStructure> _structures;

    void Awake()
    {
        Instance = this;

        _structures = Resources.LoadAll<ScriptableStructure>("Structures").ToList();
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

    public void SpawnBuildingOnTile(Tile tile)
    {
        if (!tile) return;

        var randomPrefab = GetRandomStructure<StructureBase>(StructureType.Building);
        
        var dimensions = randomPrefab.XZDimensions;

        if (GridManager.Instance.IsTileBuildable(tile, dimensions))
        {
            var spawnedStructure = Instantiate(randomPrefab);
            tile.SetStructure(spawnedStructure);
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

    

}
