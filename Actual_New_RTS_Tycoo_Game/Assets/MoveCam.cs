using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public static MoveCam Instance;

    [SerializeField] float _buffer;
    Camera _cam;

    private void Awake()
    {
        Instance = this;
        _cam = Camera.main;
    }

    void Update()
    {
        //SetOrthoCamPosition();
    }

    public void SetOrthoCamPosition()
    {
        var (center, size) = CalculateOrthoSize();
        _cam.transform.position = center;
        _cam.orthographicSize = size;
    }

    private (Vector3 center, float size) CalculateOrthoSize()
    {
        var bounds = new Bounds();
        var tileColliders = new List<Collider>();
        
        var cornerTiles = GridManager.Instance.cornerTiles;

        foreach (Vector2Int tilePos in cornerTiles)
        {
            var tile = GridManager.Instance.GetStaticTileAtPosition(new Vector3Int(tilePos.x, 0, tilePos.y));
            var tileCol = tile.GetComponent<Collider>();
            tileColliders.Add(tileCol);
        }

        foreach(var col in tileColliders)
        {
            var colToCam = _cam.WorldToScreenPoint(col.transform.position);
            var camToCol = _cam.ScreenToWorldPoint(colToCam);

            bounds.Encapsulate(camToCol);
        }

        bounds.Expand(_buffer);

        var vertical = bounds.size.y;
        var horizontal = bounds.size.x * _cam.pixelHeight / _cam.pixelWidth;

        var size = Mathf.Max(horizontal, vertical) * 0.5f;
        var center = bounds.center + new Vector3(0, 0, -10);

        return (center, size);
    }
}
