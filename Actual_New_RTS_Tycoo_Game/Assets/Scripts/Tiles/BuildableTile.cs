using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableTile : Tile
{
    public BuildableTile Connection { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public void SetConnection(BuildableTile bTile) => Connection = bTile;

    public void SetG(float g) => G = g;

    public void SetH(float h) => H = h;


    public List<BuildableTile> Neighbours;
    StructureBase _highlightedStructure;

    [SerializeField] Material _canBuildMat, _cantBuildMat, _baseMaterial, _offsetMaterial, _hoverMaterial;
    Material _oldMat, _oldStructureMat;
    
    public override void Init(int x, int z)
    {
        var isOffset = (x + z) % 2 == 1;
        if (isOffset) _renderer.SetFirstMaterial(_offsetMaterial);
        else _renderer.SetFirstMaterial(_baseMaterial);

        Neighbours = GetBuildableNeighbours();

    }
    #region MouseOnOff Methods

    void OnMouseEnter()
    {
        //if (!GameManager.Instance.IsGameInThisState(GameManager.GameStates.GameResumed)) return;

        _oldMat = _renderer.materials[0];

        _renderer.SetFirstMaterial(_hoverMaterial);

        var selectedStructure = StructureManager.Instance._selectedStructure;

        Tile tile = GrabTileFromRaycast();
        if (!tile || !selectedStructure) return;

        var dimensions = Helper.Vector2ToGridCoordinates(selectedStructure.XZDimensions);

        if (GridManager.Instance.IsTileBuildable(tile, dimensions))
        {
            _highlightedStructure = StructureManager.Instance.SpawnEmptyStructureOnTile(tile);

            var rend = _highlightedStructure.FindMeshRendererOnStructure();
            //_oldStructureMat = rend.material;

            rend.SetFirstMaterial(_canBuildMat);
        }
        else
        {
            _highlightedStructure = StructureManager.Instance.SpawnEmptyStructureOnTile(tile);

            var rend = _highlightedStructure.FindMeshRendererOnStructure();
            //_oldStructureMat = rend.material;

            rend.SetFirstMaterial(_cantBuildMat);
        }

        _highlightedStructure.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        
    }

    void OnMouseExit()
    {
        //if (!GameManager.Instance.IsGameInThisState(GameManager.GameStates.GameResumed)) return;

        if (_highlightedStructure) Destroy(_highlightedStructure.gameObject);

        _renderer.SetFirstMaterial(_oldMat);
    }

    #endregion

    Tile GrabTileFromRaycast()
    {
        Tile tile = null;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            var hitPos = new Vector3Int(
                (int)hit.transform.position.x, 0,
                (int)hit.transform.position.z);
            tile = GridManager.Instance.GetBuildableTileAtPosition(hitPos);
        }
        return tile;
    }

    public int GetMoveDistanceFromTile(BuildableTile tile)
    {
        Vector3Int tilePos = Helper.CastV3ToInt(tile.transform.position);
        Vector3Int thisPos = Helper.CastV3ToInt(transform.position);

        var xDiff = Helper.Positive(thisPos.x - tilePos.x);
        var zDiff = Helper.Positive(thisPos.z - tilePos.z);

        var moveDist = xDiff + zDiff;
        return moveDist;
    }

    List<BuildableTile> GetBuildableNeighbours()
    {
        List<Tile> tempNeighbours = new List<Tile>();
        List<BuildableTile> neighbours = new List<BuildableTile>();

        var tilePos = Helper.CastV3ToInt(transform.position);

        var tile1 = GridManager.Instance.GetBuildableTileAtPosition(tilePos + new Vector3Int(1, 0, 0));
        var tile2 = GridManager.Instance.GetBuildableTileAtPosition(tilePos + new Vector3Int(0, 0, 1));
        var tile3 = GridManager.Instance.GetBuildableTileAtPosition(tilePos + new Vector3Int(-1, 0, 0));
        var tile4 = GridManager.Instance.GetBuildableTileAtPosition(tilePos + new Vector3Int(0, 0, -1));

        tempNeighbours.Add(tile1);
        tempNeighbours.Add(tile2);
        tempNeighbours.Add(tile3);
        tempNeighbours.Add(tile4);

        foreach (Tile tile in tempNeighbours)
        {
            if (tile)
            {
                neighbours.Add((BuildableTile)tile);
            }
            
        }
        tempNeighbours.Clear();

        return neighbours;
    }

}
