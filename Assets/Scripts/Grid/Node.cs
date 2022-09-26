using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int Index { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public int Distance { get; set; }

    public int Poids { get; set; }

    public int Heuristique { get; set; }
    public int Cout { get; set; }

    public Node Pred { get; set; }

    public bool Explored { get; set; }

    public bool Active { get; set; }
    public Color ActiveColor { get; set; }

    public Node(int index, int x, int y)
    {
        Index = index;
        X = x;
        Y = y;
        Active = false;
        Distance = int.MaxValue;
        Poids = 1;
        Pred = null;
        Explored = false;
        
        //Astar
        Heuristique = 0;
        Cout = 0;
    }

    public override bool Equals(object obj)
    {
        Node n = (Node)obj;
        return n == null ? false : Index.Equals(n.Index);
    }

    public override string ToString()
    {
        return X + "/" + Y + " ==> " + Distance + " | " + Explored;
    }
}
