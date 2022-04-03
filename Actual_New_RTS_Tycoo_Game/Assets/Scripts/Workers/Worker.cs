using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public Tile OccupiedTile;
    public float MoveSpeed;
    [SerializeField] MeshRenderer _selectedCircle;
    public bool IsSelected { get; private set; } = false;
    public CancellationTokenSource TokenSource = null;
    public StructureBase TargetStructure = null;


    #region Public Methods

    public void ChangeWorkerState(WorkerManager.WorkerStates newState)
    {
        switch (newState)
        {
            case WorkerManager.WorkerStates.Stationary:
                ChangeWorkerState(WorkerManager.WorkerStates.Mining);
                if (OccupiedTile.OccupiedStructure != null) ChangeWorkerState(WorkerManager.WorkerStates.Constructing);
                
                break;
            case WorkerManager.WorkerStates.Moving:
                if (TargetStructure) TargetStructure.workers.Remove(this);
                    TargetStructure = null;
                if (OccupiedTile.TryGetComponent(out MinableTile mTile)) if (mTile.WorkerList.Contains(this))
                        mTile.WorkerList.Remove(this);
                    
                break;
                
            case WorkerManager.WorkerStates.Constructing:
                TargetStructure = OccupiedTile.OccupiedStructure;
                TargetStructure.workers.Add(this);
                break;
                
            case WorkerManager.WorkerStates.Mining:
                if (OccupiedTile.TryGetComponent(out MinableTile mineTile))
                    mineTile.WorkerList.Add(this);
                break;
        }
    }

    public async void MoveWorkerToTileList(List<Vector3Int> tilePosList)
    {
        TokenSource = new CancellationTokenSource();
        var token = TokenSource.Token;
        //IsMoving = true;
        ChangeWorkerState(WorkerManager.WorkerStates.Moving);

        foreach(Vector3Int tilePos in tilePosList)
        {
            bool isCancelled = false;
            try
            {
                await MoveWorkerToTile(tilePos, token);
                OccupiedTile = GridManager.Instance.GetWalkableTileAtPosition(tilePos);
            }
            catch (OperationCanceledException ex)
            {
                Debug.Log("CAUGHT IT");
                TokenSource.Dispose();
                TokenSource = null;
                ChangeWorkerState(WorkerManager.WorkerStates.Stationary);
            }
            finally
            {
                if (TokenSource == null) isCancelled = true;

            }
            if (isCancelled) break;
        }

        //TokenSource.Dispose();
        TokenSource = null;
        ChangeWorkerState(WorkerManager.WorkerStates.Stationary);
    }

    public void WorkerSelected(bool condition)
    {
        IsSelected = condition;
        _selectedCircle.enabled = condition;
    }

    public async void StallFunction(int time)
    {
        await Task.Delay(time);
    }

    #endregion

    #region Private Methods
    async Task MoveWorkerToTile(Vector3 destination, CancellationToken token)
    {
        destination += new Vector3 (0.5f, 1, -0.5f);
        var end = Time.time + (1 / MoveSpeed);
        while (Time.time < end)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, MoveSpeed * Time.deltaTime);
            await Task.Yield();

            if (token.IsCancellationRequested)
            {
                //todo: cleanup
                transform.position = OccupiedTile.GetTileToWorkerPos();
                token.ThrowIfCancellationRequested();
            }
        }
    }

    bool IsOnTile()
    {
        var targetPos = OccupiedTile.GetTileToWorkerPos();
        if (transform.position != targetPos) return false;
        else return true;
    }
    #endregion
}
