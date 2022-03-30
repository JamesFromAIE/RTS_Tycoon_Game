using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkBase : StructureBase
{
    public float AoEDistance;
    public LayerMask BuildingLayer;
    [SerializeField] MeshRenderer AoERenderer;

    public virtual int Effect(int population)
    {
        return population;
    }

    public override void Placed()
    {
        for (int i = 0; i < WorkerPoints.Count; i++)
        {
            var pos = Helper.CastV3ToInt(WorkerPoints[i].position);
            WorkerTiles.Add(GridManager.Instance.GetBuildableTileAtPosition(pos));
        }
    }

    public override void Constructed()
    {
        //CopySelfOnNearbyBuildings();
    }

    public void CopySelfOnNearbyBuildings()
    {
        Collider[] structures =  Physics.OverlapSphere(AoERenderer.transform.position, AoEDistance, BuildingLayer);

        foreach(Collider building in structures)
        {
            if (building.TryGetComponent(out BuildingBase bScript))
            {
                if (!bScript.InRangeLankmarks.Contains(this))
                    bScript.InRangeLankmarks.Add(this);
            }
        }
    }

    public void RemoveSelfFromNearbyBuildings()
    {
        Collider[] colliders = Physics.OverlapSphere(AoERenderer.transform.position, AoEDistance, BuildingLayer);

        foreach (Collider building in colliders)
        {
            if (building.TryGetComponent(out BuildingBase bScript))
            {
                bScript.RemoveLandmarkFromList(this);
            }
        }
    }


}
