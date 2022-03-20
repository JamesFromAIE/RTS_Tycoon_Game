using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableTile : Tile
{
    Material _oldMat;
    [SerializeField] Material _baseMaterial, _offsetMaterial, _hoverMaterial;

    public override void Init(int x, int z)
    {
        var isOffset = (x + z) % 2 == 1;
        Debug.Log(isOffset);
        if (isOffset) _renderer.SetFirstMaterial(_offsetMaterial);
        else _renderer.SetFirstMaterial(_baseMaterial);
    }

    void OnMouseEnter()
    {
        _oldMat = _renderer.materials[0];

        _renderer.SetFirstMaterial(_hoverMaterial);
    }

    void OnMouseExit()
    {
        _renderer.SetFirstMaterial(_oldMat);
    }
}
