using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBase : StructureBase
{
    public int Population;
    public List<LandmarkBase> InRangeLankmarks;

    public override void Placed()
    {

    }

    public override void Constructed()
    {
       
    }

    public void SendPopulation()
    { 
        UIManager.Instance.GainPopulation(FindPopulation(Population));
    }

    int FindPopulation(int oldPop)
    {
        int newPop = oldPop;

        foreach (LandmarkBase l in InRangeLankmarks)
        {
            newPop += l.Effect(oldPop);
        }

        return newPop;
    }
}
