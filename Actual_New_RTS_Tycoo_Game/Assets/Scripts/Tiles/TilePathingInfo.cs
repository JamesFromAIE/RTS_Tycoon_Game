using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePathingInfo
{
    public BuildableTile Connection { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public void SetConnection(BuildableTile bTile) => Connection = bTile;

    public void SetG(float g) => G = g;

    public void SetH(float h) => H = h;
}
