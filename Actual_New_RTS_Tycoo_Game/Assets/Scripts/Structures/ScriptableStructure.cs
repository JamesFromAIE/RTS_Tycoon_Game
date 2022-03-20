using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure", menuName = "Scriptable Structure")]
public class ScriptableStructure : ScriptableObject
{
    public StructureBase StructurePrefab;
    public StructureType SType;
    public int BuildTime;
}

public enum StructureType
{
    Building = 0,
    Landmark = 1,
}
