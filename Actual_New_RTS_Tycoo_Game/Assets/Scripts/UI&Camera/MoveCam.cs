using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public static MoveCam Instance;

    [SerializeField] float _buffer;
    [SerializeField] float _scrollSpeed;
    Camera _cam;
    Bounds _camBounds;

    Vector3 _dragOrigin;
    float _minSize;
    float _maxSize;

    private void Awake()
    {
        Instance = this;
        _cam = Camera.main;
    }

    void Update()
    {
        DragCamera();

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            _cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * _scrollSpeed;
    }

    void LateUpdate()
    {
        _cam.orthographicSize = ClampCamSize(_cam.orthographicSize);
    }

    #region Public Methods

    public void SetOrthoCamPosition()
    {
        var (center, size, bounds) = FindGridCamCenterAndSize();

        _cam.transform.position = center - (_cam.transform.forward * 30);

        _cam.orthographicSize = size;
        _maxSize = size;
        _minSize = size / 5;
        _camBounds = bounds;
    }
    #endregion

    #region Private Methods

    private bool IsCameraWithinBounds()
    {
        var currBounds = GrabCameraBounds();
        if (_camBounds.ContainBounds(currBounds)) return true;
        else
        {
            return false;
        }
    }

    private void DragCamera()
    {
        if (Input.GetMouseButtonDown(2))
            _dragOrigin = _cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(2))
        {
            //Debug.Log("Scroll Button Pressed");
            Vector3 difference = _dragOrigin - _cam.ScreenToWorldPoint(Input.mousePosition);
            _cam.transform.position += difference;
            if (!IsCameraWithinBounds())
            {
                _cam.transform.position -= difference * 1.005f;
            }

        }
    }

    private Bounds GrabCameraBounds()
    {
        var camToWorldPositions = new List<Vector3>();

        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 center = (ray.GetPoint(30));

        var bounds = new Bounds(center, new Vector3(_cam.orthographicSize, 2.5f, _cam.orthographicSize));
        //bounds.Expand(_buffer);

        return bounds;

    }

    private (Vector3 center, float size, Bounds camBounds) FindGridCamCenterAndSize()
    {
        var bounds = new Bounds();
        var v3Corners = GrabCornerPositions();

        foreach (var point in v3Corners) bounds.Encapsulate(point);

        bounds.Expand(_buffer);

        var vertical = bounds.size.z;
        var horizontal = bounds.size.x * _cam.pixelHeight / _cam.pixelWidth;

        var size = Mathf.Max(horizontal, vertical) * 0.5f;
        var center = bounds.center;

        return (center, size, bounds);
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
        return Mathf.Clamp(camSize, _minSize, _maxSize);
    }

    #endregion
}
