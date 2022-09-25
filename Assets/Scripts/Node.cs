using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int Index { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public bool Active { get; set; }
    public Color ActiveColor { get; set; }

    public Node(int index, int x, int y)
    {
        Index = index;
        X = x;
        Y = y;
        Active = false;
    }

    public override bool Equals(object obj)
    {
        Node n = (Node)obj;
        return n == null ? false : Index.Equals(n.Index);
    }
}
