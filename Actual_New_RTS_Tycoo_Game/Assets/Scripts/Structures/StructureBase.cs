using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StructureBase : MonoBehaviour
{
    [ReadOnly]
    public Tile OccupiedTile;
    public Vector2Int XZDimensions;
    public ScriptableStructure StructureStats;
    public List<Transform> WorkerPoints;

    public List<Tile> WorkerTiles { get; private set; } = new List<Tile>();
    public float _buildTime;
    [ReadOnly]
    public List<Worker> workers;
    [ReadOnly]
    public bool isEmpty = false;
    [ReadOnly]
    public bool isConstructed;
    [SerializeField] MeshRenderer oldMesh, newMesh;

    public virtual void Placed()
    {
        
    }

    public virtual void Constructed()
    {
        
    }

    public void ConstructedMesh(bool isConstructed)
    {
        if (isConstructed)
        {
            oldMesh.enabled = false;
            newMesh.enabled = true;
        }
        else
        {
            oldMesh.enabled = true;
            newMesh.enabled = false;
        }
    }

    public void ConstructingStructure()
    {
        if (isConstructed) return;
        else
        {
            int factor = workers.Count;

            _buildTime -= Time.deltaTime * factor;

            if (_buildTime <= 0)
            {
                _buildTime = 0;
                ConstructedMesh(true);
                if (TryGetComponent(out BuildingBase bScript) && !isEmpty)
                    UIManager.Instance.UpdateBuildingPopulation.AddListener(bScript.SendPopulation);
                else if (TryGetComponent(out LandmarkBase lScript) && !isEmpty)
                    UIManager.Instance.UpdateBuildingPopulation.AddListener(lScript.CopySelfOnNearbyBuildings);
                Constructed();
            }
        }
    }
}
