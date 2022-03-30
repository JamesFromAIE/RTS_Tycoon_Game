using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSelection : MonoBehaviour
{
    SelectedDictionary _selectedTable;
    RaycastHit _hit;

    bool _dragSelect;

    //Collider variables
    //=======================================================//

    MeshCollider _selectionBox;
    Mesh _selectionMesh;

    Vector3 _p1;
    Vector3 _p2;

    //the corners of our 2d selection box
    Vector2[] _corners;

    //the vertices of our meshcollider
    Vector3[] _verts;
    Vector3[] _vecs;

    // Start is called before the first frame update
    void Start()
    {
        _selectedTable = GetComponent<SelectedDictionary>();
        _dragSelect = false;
    }

    // Update is called once per frame
    void Update()
    {
        //1. when left mouse button clicked (but not released)
        if (Input.GetMouseButtonDown(0))
        {
            _p1 = Input.mousePosition;
        }

        //2. while left mouse button held
        if (Input.GetMouseButton(0))
        {
            if((_p1 - Input.mousePosition).magnitude > 40)
            {
                _dragSelect = true;
            }
        }

        //3. when mouse button comes up
        if (Input.GetMouseButtonUp(0))
        {
            if(_dragSelect == false) //single select
            {
                Ray ray = Camera.main.ScreenPointToRay(_p1);

                if(Physics.Raycast(ray,out _hit, 100.0f, (1 << 9)))
                {
                    if (Input.GetKey(KeyCode.LeftShift)) //inclusive select
                    {
                        _selectedTable.AddSelected(_hit.transform.GetComponent<Worker>());
                    }
                    else //exclusive selected
                    {
                        _selectedTable.DeselectAll();
                        _selectedTable.AddSelected(_hit.transform.GetComponent<Worker>());
                    }
                }
                else //if we didnt _hit something
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //do nothing
                    }
                    else
                    {
                        _selectedTable.DeselectAll();
                    }
                }
            }
            else //marquee select
            {
                _verts = new Vector3[4];
                _vecs = new Vector3[4];
                int i = 0;
                _p2 = Input.mousePosition;
                _corners = GetBoundingBox(_p1, _p2);

                foreach (Vector2 corner in _corners)
                {
                    Ray ray = Camera.main.ScreenPointToRay(corner);

                    if (Physics.Raycast(ray, out _hit, 100.0f))
                    {
                        _verts[i] = new Vector3(_hit.point.x, _hit.point.y, _hit.point.z);
                        _vecs[i] = ray.origin - _hit.point;
                        Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), _hit.point, Color.red, 1.0f);
                    }
                    i++;
                }

                //generate the mesh
                _selectionMesh = GenerateSelectionMesh(_verts,_vecs);

                _selectionBox = gameObject.AddComponent<MeshCollider>();
                _selectionBox.sharedMesh = _selectionMesh;
                _selectionBox.convex = true;
                _selectionBox.isTrigger = true;

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    _selectedTable.DeselectAll();
                }

               Destroy(_selectionBox, 0.02f);

            }//end marquee select

            _dragSelect = false;

        }
       
    }

    private void OnGUI()
    {
        if(_dragSelect == true)
        {
            var rect = Helper.GetScreenRect(_p1, Input.mousePosition);
            Helper.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Helper.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    //create a bounding box (4 corners in order) from the start and end mouse position
    Vector2[] GetBoundingBox(Vector2 p1,Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) //if p1 is to the left of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else //if p1 is below p2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else //if p1 is to the right of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else //if p1 is below p2
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }

        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;

    }

    //generate a mesh from the 4 bottom points
    Mesh GenerateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for(int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for(int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        var worker = other.GetComponent<Worker>();
        if (!worker) return;
        _selectedTable.AddSelected(worker);
    }

}
