using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] int _width, _length;

    [SerializeField] Tile _buildableTile, _staticTile;

    private Dictionary<Vector3Int, Tile> _tiles;

    void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector3Int, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _length; z++)
            {
                if (x < 7 && z < 9) SpawnStaticTile(x, z);
                else SpawnBuildableTile(x, z);

            }
        }

        GameManager.Instance.ChangeState(GameManager.GameStates.GameStopped);
    }

    void SpawnBuildableTile(int x, int z)
    {
        var spawnedTile = Instantiate(_buildableTile, new Vector3(x, 0, z), Quaternion.identity);
        spawnedTile.name = $"Tile {x} {z}";
        
        spawnedTile.Init(x, z);

        spawnedTile.transform.parent = transform;

        _tiles[new Vector3Int(x, 0, z)] = spawnedTile;
    }

    void SpawnStaticTile(int x, int z)
    {
        var spawnedTile = Instantiate(_staticTile, new Vector3(x, 0, z), Quaternion.identity);
        spawnedTile.name = $"Tile {x} {z}";

        spawnedTile.Init(x, z);

        spawnedTile.transform.parent = transform;

        //_tiles[new Vector3Int(x, 0, z)] = spawnedTile;
    }

    public Tile GetStructureSpawnTile(Vector2[] dimensions)
    {
        bool isPlaceable = false;
        int iterationCount = 0;
        Tile possibleTile = null;
        while (!isPlaceable)
        {
            //isPlaceable = true;
            possibleTile = _tiles.Where(t => t.Value.Buildable/* &&*/).OrderBy(t => Random.value).First().Value;
            for (int i = 0; i < dimensions.Length; i++)
            {
                var tilePos = Helper.XYToXZInt(dimensions[i]) + 
                    new Vector3Int((int)possibleTile.transform.position.x,0, (int)possibleTile.transform.position.z);
                var tile = GetTileAtPosition(tilePos);

                if (tile == null || tile.Buildable == false || tile.OccupiedStructure != null)
                {
                    break;
                }
                else if (i == dimensions.Length - 1)
                {
                    isPlaceable = true;
                }
            }

            iterationCount++;
            if (iterationCount > 100)
                Debug.LogError("Cant find Tile");
        }

        return possibleTile;
    }

    public Tile GetTileAtPosition(Vector3Int pos)
    {
        if(_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }
}
