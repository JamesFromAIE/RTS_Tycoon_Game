using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public Tile OccupiedTile;
    public float MoveSpeed;
    [SerializeField] MeshRenderer _selectedCircle;
    public bool IsSelected { get; private set; } = false;

    #region Public Methods
    public async void MoveWorkerToTileList(List<Vector3Int> tilePosList)
    {
        foreach(Vector3Int tilePos in tilePosList)
        {
            await MoveWorkerToTile(tilePos);
            OccupiedTile = GridManager.Instance.GetBuildableTileAtPosition(tilePos);
        }
    }

    public void WorkerSelected(bool condition)
    {
        IsSelected = condition;
        _selectedCircle.enabled = condition;
    }
    #endregion

    #region Private Methods
    async Task MoveWorkerToTile(Vector3 destination)
    {
        destination += new Vector3 (0.5f, 1, -0.5f);
        var end = Time.time + (1 / MoveSpeed);
        while (Time.time < end)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, MoveSpeed * Time.deltaTime);
            await Task.Yield();
        }
    }
    #endregion
}
