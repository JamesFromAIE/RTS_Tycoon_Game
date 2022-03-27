using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineLandmark : LandmarkBase
{
    public int popMultiplier;

    public override int Effect(int oldPos)
    {
        return oldPos * (popMultiplier + 1);
    }
}
