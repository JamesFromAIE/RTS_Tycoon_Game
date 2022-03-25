using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure", menuName = "Scriptable Structure")]
public class ScriptableStructure : ScriptableObject
{
    public int GoldCost { get { return goldCost; } private set { goldCost = value; } }
    [SerializeField]
    private int goldCost = 0;
    public int StoneCost { get { return stoneCost; } private set { stoneCost = value; } }
    [SerializeField]
    private int stoneCost = 0;
    public int BuildTime { get { return buildTime; } private set { buildTime = value; } }
    [SerializeField]
    private int buildTime = 0;
}

