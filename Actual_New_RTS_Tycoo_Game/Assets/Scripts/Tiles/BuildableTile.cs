using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableTile : Tile
{
    [SerializeField] Material _canBuildMat, _cantBuildMat;
    Material _oldMat, _oldStructureMat;
    [SerializeField] Material _baseMaterial, _offsetMaterial, _hoverMaterial;
    StructureBase _highlightedStructure;

    public override void Init(int x, int z)
    {
        var isOffset = (x + z) % 2 == 1;
        if (isOffset) _renderer.SetFirstMaterial(_offsetMaterial);
        else _renderer.SetFirstMaterial(_baseMaterial);
    }

    void OnMouseEnter()
    {
        //if (!GameManager.Instance.IsGameInThisState(GameManager.GameStates.GameResumed)) return;

        _oldMat = _renderer.materials[0];

        _renderer.SetFirstMaterial(_hoverMaterial);

        var selectedStructure = StructureManager.Instance._selectedStructure;
        Tile tile = GrabTileFromRaycast();
        if (!tile || !selectedStructure) return;

        if (GridManager.Instance.IsTileBuildable(tile, selectedStructure.XZDimensions))
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
            tile = GridManager.Instance.GetTileAtPosition(hitPos);
        }
        return tile;
    }
}
