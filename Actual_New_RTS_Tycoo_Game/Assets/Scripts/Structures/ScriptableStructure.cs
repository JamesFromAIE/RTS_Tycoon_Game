using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure", menuName = "Scriptable Structure")]
public class ScriptableStructure : ScriptableObject
{
    public int GoldCost { get { return _goldCost; } private set { _goldCost = value; } }
    [SerializeField]
    private int _goldCost = 0;
    public int StoneCost { get { return _stoneCost; } private set { _stoneCost = value; } }
    [SerializeField]
    private int _stoneCost = 0;
    public int BuildTime { get { return _buildTime; } private set { _buildTime = value; } }
    [SerializeField]
    private int _buildTime = 0;
}

