using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper 
{
    public static MeshRenderer FindMeshRendererOnStructure(this StructureBase structure)
    {
        MeshRenderer meshRend = structure.GetComponent<MeshRenderer>();

        if (meshRend) return meshRend;
        else
        {
            meshRend = structure.GetComponentInChildren<MeshRenderer>();

            if (meshRend) return meshRend;
            else return null;
        }
    }

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

    public static Bounds OrthoBounds(this Camera cam)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = cam.orthographicSize * 2;
        Bounds bounds = new Bounds(
            cam.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}
