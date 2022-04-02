using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBase : StructureBase
{
    public int Population;
    public List<LandmarkBase> InRangeLankmarks;
    private LandmarkBase _landmark;

    void Update()
    {
        ConstructingStructure();
    }

    public override void Placed()
    {
        ConstructedMesh(isConstructed);
        for (int i = 0; i < WorkerPoints.Count; i++)
        {
            var pos = Helper.CastV3ToInt(WorkerPoints[i].position);
            var tile = GridManager.Instance.GetBuildableTileAtPosition(pos);
            WorkerTiles.Add(tile);
        }

        workers = new List<Worker>();
        _buildTime = StructureStats.BuildTime;
        isConstructed = false;
    }

    public override void Constructed()
    {
        isConstructed = true;
        workers.Clear();
        
        UIManager.Instance.TriggerConstructionEvent();
    }

    public void SendPopulation()
    { 
        UIManager.Instance.GainPopulation(FindPopulation(Population));
    }

    int FindPopulation(int oldPop)
    {
        int iterations = 0;
        foreach (LandmarkBase landmark in InRangeLankmarks)
        {
            if (iterations < 1) _landmark = landmark;
            iterations++;
        }

        if (!_landmark) return oldPop;

        int newPop = _landmark.Effect(oldPop);

        return newPop;
    }

    public void RemoveLandmarkFromList(LandmarkBase lBase)
    {
        if (InRangeLankmarks.Contains(lBase)) InRangeLankmarks.Remove(lBase);

        if (_landmark == lBase) _landmark = null;
    }
}
