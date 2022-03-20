using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper 
{
    public static void SetFirstMaterial(this MeshRenderer meshRend, Material newMat)
    {
        var currMats = meshRend.materials;
        currMats[0] = newMat;
        meshRend.materials = currMats;
    }

    public static Vector3Int XYToXZInt(Vector2 vec2)
    {
        var vec3 = new Vector3(vec2.x, 0, vec2.y);
        return new Vector3Int((int)vec3.x, (int)vec3.y, (int)vec3.z);
    }

    public static Vector2[] GetStructureDimensions(this ScriptableStructure scriptableStructure)
    {
        return null;
    }
}
