using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineLandmark : LandmarkBase
{
    public int AdditionalPopulation;

    public override int Effect(int oldPos)
    {
        return oldPos + 5;
    }
}
