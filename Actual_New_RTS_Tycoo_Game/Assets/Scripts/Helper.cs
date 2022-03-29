using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper 
{

    public static BuildableTile LowestFAndHCost(List<BuildableTile> tileList)
    {
        return null;
    }
    public static int Positive(int number)
    {
        return number < 0 ? number * -1 : number;
    }

    public static Vector3Int CastV3ToInt(Vector3 oldV3)
    {
        int v3X = (int)oldV3.x;
        int v3Y = (int)oldV3.y;
        int v3Z = (int)oldV3.z;

        Vector3Int newV3 = new Vector3Int(v3X, v3Y, v3Z);

        return newV3;
    }


    public static bool AreTheseTooClose(Vector3 v1, Vector3 v2, float distance)
    {
        if (v1.magnitude - v2.magnitude < distance) return true;
        else return false;
    }
    public static bool ContainBounds(this Bounds bounds, Bounds target)
    {
        return bounds.Contains(target.center);
    }

    public static Vector2[] Vector2ToGridCoordinates(Vector2Int dimensions)
    {
        List<Vector2> coordinatesList = new List<Vector2>();
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int z = 0; z < dimensions.y; z++)
            {
                coordinatesList.Add(new Vector2(x, z));
            }
        }

        var coordinatesArray = coordinatesList.ToArray();
        return coordinatesArray;
    }

    public static void ToggleUIElementVisibility(this List<GameObject> list,bool condition)
    {
        foreach(GameObject element in list)
        {
            element.SetActive(condition);
        }
    }
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
