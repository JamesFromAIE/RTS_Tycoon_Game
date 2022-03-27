using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public static MoveCam Instance;

    [SerializeField] float _buffer;
    [SerializeField] float _scrollSpeed;
    Camera _cam;
    float _minSize;
    float _maxSize;

    private void Awake()
    {
        Instance = this;
        _cam = Camera.main;
    }

    void Update()
    {
        _cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * _scrollSpeed;
        _cam.orthographicSize = ClampCamSize(_cam.orthographicSize);
    }

    #region Public Methods

    public void SetOrthoCamPosition()
    {
        var (center, size) = FindGridCamSize();
        _cam.transform.position = center - (_cam.transform.forward * 30);
        _cam.orthographicSize = size;
        _maxSize = size;
        _minSize = size / 5;
    }
    #endregion

    #region Private Methods

    private (Vector3 center, float size) FindGridCamSize()
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

    private float ClampCamSize(float camSize)
    {
        if (camSize < _minSize) camSize = _minSize;
        else if (camSize > _maxSize) camSize = _maxSize;
        return camSize;
    }

    #endregion
}
