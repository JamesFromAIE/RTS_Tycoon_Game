using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [MinValue(0)]
    [SerializeField] int _width, _length, _outerThickness;
    
    [SerializeField] List<Vector2Int> _bufferList;
    [SerializeField] List<Vector2Int> _goldTileList;
    [SerializeField] List<Vector2Int> _stoneTileList;

    [Required]
    [SerializeField] Tile _buildableTile, _staticTile, _goldTile, _stoneTile;
    [SerializeField] LayerMask _tileLayer;

    private Dictionary<Vector3Int, Tile> _tiles;
    private Dictionary<Vector3Int, Tile> _staticTiles;
    private Dictionary<Vector3Int, Tile> _mineableTiles;
    public List<Vector2Int> cornerTiles { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameInThisState(GameManager.GameStates.GameResumed)) return;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, _tileLayer))
            {
                var structMan = StructureManager.Instance;
                var hitPos = new Vector3Int(
                    (int)hit.transform.position.x, 0,
                    (int)hit.transform.position.z);
                Tile tile = GetWalkableTileAtPosition(hitPos);
                structMan.SpawnStructureOnTile(tile);
                
                structMan.SetSelectedStructure(structMan._selectedStructure);
            }
        }

    }

    

    #region Private Methods

    void SpawnBorderStaticTiles(int gridWidth, int gridLength)
    {
        for (int i = 0; i < _outerThickness; i++)
        {
            SpawnStaticTilesOnZAxis(-1 - i, gridLength);
            SpawnStaticTilesOnZAxis(gridWidth + i, gridLength);
            SpawnStaticTilesOnXAxis(-1 - i, gridWidth);
            SpawnStaticTilesOnXAxis(gridLength + i, gridWidth);
        }
        SpawnStaticTilesInCorners(gridWidth, gridLength, _outerThickness);
        
    }

    void SpawnStaticTilesInCorners(int gridWidth, int gridLength, int cornerThickness)
    {
        var grid00 = GetGridCorners(cornerThickness, 0, 0);
        var grid10 = GetGridCorners(cornerThickness, gridWidth, 0);
        var grid01 = GetGridCorners(cornerThickness, 0, gridLength);
        var grid11 = GetGridCorners(cornerThickness, gridWidth, gridLength);

        SpawnStaticCornerTiles(grid00, grid10, grid01, grid11);
    }

    void SpawnStaticCornerTiles(
        List<Vector2Int> list1, 
        List<Vector2Int> list2, 
        List<Vector2Int> list3, 
        List<Vector2Int> list4)
    {
        foreach (Vector2Int cornerTile in list1)
        {
            SpawnStaticTile(cornerTile.x, cornerTile.y);
        }
        foreach (Vector2Int cornerTile in list2)
        {
            SpawnStaticTile(cornerTile.x, cornerTile.y);
        }
        foreach (Vector2Int cornerTile in list3)
        {
            SpawnStaticTile(cornerTile.x, cornerTile.y);
        }
        foreach (Vector2Int cornerTile in list4)
        {
            SpawnStaticTile(cornerTile.x, cornerTile.y);
        }
    }

    List<Vector2Int> GetGridCorners(int cornerThickness, int xSpawnPoint, int zSpawnPoint)
    {
        List<Vector2Int> tileList = new List<Vector2Int>();

        int xDirec = xSpawnPoint > 0 ? 1 : -1;
        int zDirec = zSpawnPoint > 0 ? 1 : -1;

        int xOffset = xSpawnPoint > 0 ? 0 : -1;
        int zOffset = zSpawnPoint > 0 ? 0 : -1;

        for (int x = xSpawnPoint; x < cornerThickness + xSpawnPoint; x++)
        {
            for (int z = zSpawnPoint; z < cornerThickness + zSpawnPoint; z++)
            {
                Vector2Int newVec2 = new Vector2Int((x * xDirec) + xOffset, (z * zDirec) + zOffset);
                tileList.Add(newVec2);
            }
        }
        cornerTiles.Add(tileList[tileList.Count - 1]);

        return tileList;
    }

    void SpawnStaticTilesOnZAxis(int staticAxis, int iterativeAxis)
    {
        for (int i = 0; i < iterativeAxis; i++)
        {
            SpawnStaticTile(staticAxis, i);
        }
    }
    void SpawnStaticTilesOnXAxis(int staticAxis, int iterativeAxis)
    {
        for (int i = 0; i < iterativeAxis; i++)
        {
            SpawnStaticTile(i, staticAxis);
        }
    }

    bool CheckIfTileIsStatic(int x, int z, List<Vector2Int> bufferPoints)
    {
        if (x < bufferPoints[0].x &&
            z < bufferPoints[0].y ||
            x > _width - bufferPoints[1].x &&
            z < bufferPoints[1].y ||
            x < bufferPoints[2].x &&
            z > _length - bufferPoints[2].y ||
            x > _width - bufferPoints[3].x &&
            z > _length - bufferPoints[3].y) return true;
        else return false;
    }

    bool CheckIfTileIsMineable(int x, int z, List<Vector2Int> goldTiles, List<Vector2Int> stoneTiles, out Resource resource)
    {
        var checkPos = new Vector2Int(x, z);
        resource = Resource.Gold;

        foreach (Vector2Int tilePos in goldTiles)
        {
            if (checkPos == tilePos)
            {
                return true;
            }
        }

        foreach (Vector2Int tilePos in stoneTiles)
        {
            if (checkPos == tilePos)
            {
                resource = Resource.Stone;
                return true;
            }
        }

        return false;
    }

    void SpawnBuildableTile(int x, int z)
    {
        var spawnedTile = Instantiate(_buildableTile, new Vector3(x, 0, z), Quaternion.identity);
        spawnedTile.name = $"Tile {x} {z}";
        
        spawnedTile.transform.parent = transform;

        _tiles[new Vector3Int(x, 0, z)] = spawnedTile;
    }

    void SpawnStaticTile(int x, int z)
    {
        //if (GetTileAtPosition(new Vector3Int(x, 0, z))) return;
        var spawnedTile = Instantiate(_staticTile, new Vector3(x, 0, z), Quaternion.identity);
        spawnedTile.name = $"Tile {x} {z}";

        spawnedTile.transform.parent = transform;

        _staticTiles[new Vector3Int(x, 0, z)] = spawnedTile;
    }

    void SpawnMineableTile(int x, int z, Resource resource)
    {
        Tile chosenTile;
        if (resource == Resource.Gold) chosenTile = _goldTile;
        else chosenTile = _stoneTile;

        var spawnedTile = Instantiate(chosenTile, new Vector3(x, 0, z), Quaternion.identity);
        spawnedTile.name = $"Tile {x} {z}";

        spawnedTile.transform.parent = transform;

        _mineableTiles[new Vector3Int(x, 0, z)] = spawnedTile;
    }

    
    #endregion

    #region Public Methods

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector3Int, Tile>();
        _staticTiles = new Dictionary<Vector3Int, Tile>();
        _mineableTiles = new Dictionary<Vector3Int, Tile>();
        cornerTiles = new List<Vector2Int>();
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _length; z++)
            {
                if (CheckIfTileIsMineable(x, z, _goldTileList, _stoneTileList, out Resource resource))
                {

                    SpawnMineableTile(x, z, resource);
                }
                else if (CheckIfTileIsStatic(x, z, _bufferList))
                {
                    SpawnStaticTile(x, z);
                }
                else
                {
                    SpawnBuildableTile(x, z);
                }
            }
        }
        SpawnBorderStaticTiles(_width, _length);

        foreach (KeyValuePair<Vector3Int, Tile> tile in _tiles)
        {
            tile.Value.Init((int)tile.Value.transform.position.x, (int)tile.Value.transform.position.z);
        }
        foreach (KeyValuePair<Vector3Int, Tile> tile in _mineableTiles)
        {
            tile.Value.Init((int)tile.Value.transform.position.x, (int)tile.Value.transform.position.z);
        }


        //GameManager.Instance.ChangeState(GameManager.GameStates.GameStopped);
    }

    public bool IsTileBuildable(Tile spawnTile, Vector2[] dimensions)
    {
        if (!spawnTile || dimensions == null) return false;

        for (int i = 0; i < dimensions.Length; i++)
        {
            var tilePos = Helper.XYToXZInt(dimensions[i]) +
                new Vector3Int((int)spawnTile.transform.position.x, 0, (int)spawnTile.transform.position.z);
            var tile = GetWalkableTileAtPosition(tilePos);

            if (tile == null ||
                tile.Buildable == false ||
                tile.OccupiedStructure != null)
            {
                return false;
            }
        }

        return true;
    }

    public void MoveWorkerToSpawnPoint(Worker worker, Vector3Int tilePos)
    {
        _tiles.TryGetValue(tilePos, out Tile tile);
        if (!tile) return;

        worker.transform.position = tile.transform.position + new Vector3(0.5f, 1, -0.5f);
        worker.OccupiedTile = tile;
    }

    public Tile GetWalkableTileAtPosition(Vector3Int pos)
    {
        if(_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        if (_mineableTiles.TryGetValue(pos, out var mTile))
        {
            return mTile;
        }
        return null;
    }

    public Tile GetStaticTileAtPosition(Vector3Int pos)
    {
        if (_staticTiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    public Tile GetMineableTileAtPosition(Vector3Int pos)
    {
        if (_mineableTiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    public Vector3Int GetRandomTilePosition()
    {
        Random rand = new Random();
        var randomTile = _tiles.ElementAt(rand.Next(0, _tiles.Count)).Value;

        return Helper.CastV3ToInt(randomTile.transform.position);
    }

    public List<Tile> GetPathingList(Tile startingTile, Tile targetTile)
    {
        var toSearch = new List<Tile>() { startingTile };
        var processed = new List<Tile>();
        var tree = new BinaryTreeInt();

        while (toSearch.Any())
        {

            var bestTile = GetLowestFCostTile(toSearch);

            processed.Add(bestTile);
            toSearch.Remove(bestTile);

            if (bestTile == targetTile)
            {
                int iterations = 0;
                var currentPathTile = targetTile;
                var path = new List<Tile>();
                while (currentPathTile != startingTile)
                {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                    iterations++;
                    if (iterations > 100) return null;
                }
                
                return path;
            }

            foreach (var neighbour in bestTile.Neighbours.Where(t => t.IsWalkable && !processed.Contains(t)))
            {
                var inSearch = toSearch.Contains(neighbour);

                var costToNeighbour = bestTile.G + bestTile.GetMoveDistanceFromTile(neighbour);

                if (!inSearch || costToNeighbour < neighbour.G)
                {

                    neighbour.SetG(costToNeighbour);
                    neighbour.SetConnection(bestTile);

                    if (!inSearch)
                    {
                        neighbour.SetH(neighbour.GetMoveDistanceFromTile(targetTile));
                        toSearch.Add(neighbour);
                    }
                }
            }
        }
        return null;
    }

    public Tile GetLowestFCostTile(List<Tile> tileList)
    {
        var bTree = new BinaryTreeInt();

        foreach (var tile in tileList)
        {
            bTree.Insert(tile);
        }

        return bTree.GetLowestTile();
    }

    public List<Vector3Int> FindPathingTilePositions(List<Tile> tileList)
    {
        List<Vector3Int> vector3Ints = new List<Vector3Int>();

        foreach (Tile tile in tileList)
        {
            vector3Ints.Add(Helper.CastV3ToInt(tile.transform.position));
        }

        return vector3Ints;
    }

    #endregion


}
