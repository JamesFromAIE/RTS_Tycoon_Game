using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTreeInt
{
    public NodeInt root;
    public BinaryTreeInt()
    {
        root = null;
    }

    public void Insert(BuildableTile value)
    {
        NodeInt newNode = new NodeInt(value);

        if (root == null) root = newNode;
        else
        {
            NodeInt current = root;
            NodeInt parent;
            while (true)
            {
                parent = current;
                if (value.F < current.Data.F)
                {
                    current = current.Left;
                    if (current == null)
                    {
                        parent.Left = newNode;
                        break;
                    }

                }
                else if (value.F > current.Data.F)
                {
                    current = current.Right;
                    if (current == null)
                    {
                        parent.Right = newNode;
                        break;
                    }
                }
                else
                {
                    if (value.G < current.Data.G)
                    {
                        current = current.Left;
                        if (current == null)
                        {
                            parent.Left = newNode;
                            break;
                        }

                    }
                    else
                    {
                        current = current.Right;
                        if (current == null)
                        {
                            parent.Right = newNode;
                            break;
                        }
                    }
                }
            }
        }
    }

    public BuildableTile GetLowestTile()
    {
        NodeInt current = root;

        while (current.Left != null)
        {
            current = current.Left;
        }

        return current.Data;
    }


}

public class NodeInt
{
    public BuildableTile Data;
    public NodeInt Left;
    public NodeInt Right;

    public NodeInt(BuildableTile data)
    {
        Data = data;
        Left = null;
        Right = null;
    }
}

