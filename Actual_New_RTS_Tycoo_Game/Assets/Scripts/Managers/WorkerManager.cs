using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public static WorkerManager Instance;

    private Dictionary<int, Worker> _workers;

    [SerializeField] Worker _workerPrefab;
    [SerializeField] int _startingWorkers;

    void Awake()
    {
        Instance = this;
        _workers = new Dictionary<int, Worker>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit, 100, (1 << 7)) &&
                SelectedDictionary.Instance.SelectedTable.Count > 0)
            {
                StructureBase hitStructure = StructureManager.Instance.GetStructureAtPosition(hit);

                var structureWorkTiles = hitStructure.WorkerTiles;
                var workers = SelectedDictionary.Instance.SelectedTable.Values.ToArray();

                for (int i = 0; i < workers.Length; i++)
                {
                    var worker = workers[i];
                    worker.TokenSource?.Cancel();
                    var wTile = (BuildableTile)worker.OccupiedTile;
                    var bTile = (BuildableTile)structureWorkTiles[i % structureWorkTiles.Count];

                    var pathList = GridManager.Instance.GetPathingList(wTile, bTile);


                    if (pathList == null) Debug.LogError("There is NO path in this list");
                    else
                    {
                        pathList.Reverse();
                        worker.MoveWorkerToTileList(GridManager.Instance.FindPathingTilePositions(pathList));
                    }
                }
            }
            else if (Physics.Raycast(ray, out hit, 100, (1 << 6)))
            {
                Debug.Log("Heard RMB on tile layer in " + this);
                var hitPos = new Vector3Int(
                    (int)hit.transform.position.x, 0,
                    (int)hit.transform.position.z);
                var bTile = (BuildableTile)GridManager.Instance.GetBuildableTileAtPosition(hitPos);
                var workers = SelectedDictionary.Instance.SelectedTable.Values.ToArray();

                for (int i = 0; i < workers.Length; i++)
                {
                    var worker = workers[i];
                    worker.TokenSource?.Cancel();
                    var wTile = (BuildableTile)worker.OccupiedTile;

                    var pathList = GridManager.Instance.GetPathingList(wTile, bTile);
                    

                    if (pathList == null) Debug.LogWarning("Path for generic Move Position messed up by another Move Position");
                    else
                    {
                        pathList.Reverse();
                        worker.MoveWorkerToTileList(GridManager.Instance.FindPathingTilePositions(pathList));
                    }
                }
            }
            
        }
    }

    public void SpawnWorkers()
    {
        for (int i = 0; i < _startingWorkers; i++)
        {
            Worker worker = Instantiate(_workerPrefab);
            _workers[i] = worker;
            var spawnPos = GridManager.Instance.GetRandomTilePosition();
            GridManager.Instance.MoveWorkerToSpawnPoint(worker, spawnPos);
            
        }
    }

    public Worker GetFirstWorker()
    {
        _workers.TryGetValue(0, out Worker value);
        return value;
    }

}
