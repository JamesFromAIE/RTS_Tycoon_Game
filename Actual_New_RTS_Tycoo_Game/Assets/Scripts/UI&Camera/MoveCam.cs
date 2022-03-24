using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public static MoveCam Instance;

    [SerializeField] float _buffer;
    [SerializeField] float _distance;
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
        _cam.transform.position = center - (_cam.transform.forward * 30);
        _cam.orthographicSize = size;

    }

    private List<Vector3> GrabCornerPositions()
    {
        var tilePositions = new List<Vector3>();
        var cornerTiles = GridManager.Instance.cornerTiles;

        foreach (Vector2Int tilePos in cornerTiles)
        {
            var tile = GridManager.Instance.GetStaticTileAtPosition(new Vector3Int(tilePos.x, 0, tilePos.y));
            tilePositions.Add(tile.transform.position);
        }

        return tilePositions;
    }

    private (Vector3 center, float size) CalculateOrthoSize()
    {
        var bounds = new Bounds();
        var v3Corners = GrabCornerPositions();

        foreach (var point in v3Corners) bounds.Encapsulate(point);

        bounds.Expand(_buffer);

        var vertical = bounds.size.z;
        var horizontal = bounds.size.x * _cam.pixelHeight / _cam.pixelWidth;

        var size = Mathf.Max(horizontal, vertical) * 0.5f;
        var center = bounds.center;

        return (center, size);
    }

    
}
