using System.Collections;
using System.Collections.Generic;
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
