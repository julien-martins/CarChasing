using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public enum AlgorithmType
{
    Astar,
    Djikstra
}
[RequireComponent(typeof(EntityGridPos))]
public class Pathfinding : MonoBehaviour
{
    public GridSystem Grid;
    public AlgorithmType Algorithm;

    public bool DrawDebug = false;

    public EntityGridPos EndNode;
    private EntityGridPos _startNode;

    private Rigidbody _rb;

    private List<Vector2Int> neighborsDir;

    private List<Node> _closed;
    private List<Node> _open;

    // Start is called before the first frame update
    public void Init()
    {
        _startNode = GetComponent<EntityGridPos>();
        _rb = GetComponent<Rigidbody>();

        neighborsDir = new() { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };

        _closed = new();
        _open = new();

        //Force the initialisation of the position on the grid
        _startNode.ForceInit();
        EndNode.ForceInit();

        Grid.ActiveNode(_startNode.ActualNode, Color.green);
        Grid.ActiveNode(EndNode.ActualNode, Color.red);
    }

    // Update is called once per frame
    public void ExecutePath()
    {
        switch (Algorithm)
        {
            case AlgorithmType.Astar:
                UpdateAstar();
                ShorterPath();
                break;
            case AlgorithmType.Djikstra:
                UpdateDjikstra();
                ShorterPath();
                break;
        }
    }

    #region Astar

    Node MinAstar(List<Node> nodes)
    {
        Node result = null;

        foreach (var node in nodes)
        {
            if (result == null || node.Heuristique < result.Heuristique)
                result = node;
        }

        return result;
    }

    int DistAstar(Node u, Node v) => Math.Abs(u.X - v.X) + Math.Abs(u.Y + v.Y);

    //Noeud existe dans la liste avec un cout inferieur
    bool NodeInOpenList(Node n, List<Node> open)
    {
        foreach (Node node in open)
        {
            if (node.Equals(n) && node.Cout < n.Cout) return true;
        }

        return false;
    }

    void InitAstar()
    {
        _open.Clear();
        _closed.Clear();

        foreach (Node n in Grid.Nodes)
        {
            n.Heuristique = 0;
            n.Cout = 0;
            n.Pred = null;
        }
    }

    void UpdateAstar()
    {
        InitAstar();
        
        _open.Add(_startNode.ActualNode);

        while (_open.Count > 0)
        {
            Node u = MinAstar(_open);
            _open.Remove(u);

            //Si Astar trouve l'objectif
            if (u.Equals(EndNode.ActualNode)) return;

            //Explore les voisins
            foreach (Vector2Int dir in neighborsDir)
            {
                Node v = Grid.GetNodeWithIndex(u.X + dir.x, u.Y + dir.y);
                if(v == null) continue;

                if (!(NodeInOpenList(v, _open) || _closed.Contains(v)))
                {
                    v.Pred = u;
                    v.Cout = u.Cout + 1;
                    v.Heuristique = v.Cout + DistAstar(v, EndNode.ActualNode);

                    _open.Add(v);
                }
                
            }
            _closed.Add(u);
        }

    }
    #endregion

    #region Djikstra
    void InitDjikstra()
    {
        foreach (Node n in Grid.Nodes)
        {
            n.Distance = int.MaxValue;
            n.Explored = false;
        }

        _startNode.ActualNode.Distance = 0;
    }

    Node MinDjikstra(List<Node> nodes)
    {
        Node result = null;
        int mini = int.MaxValue;

        foreach (Node n in nodes)
        {
            if (!n.Explored && n.Distance <= mini)
            {
                mini = n.Distance;
                result = n;
            }
        }

        return result;
    }

    void DistDjikstra(Node n1, Node n2)
    {
        if (!n2.Explored && n2.Distance > n1.Distance + n2.Poids)
        {
            n2.Distance = n1.Distance + n2.Poids;
            n2.Pred = n1;
        }
    }

    void UpdateDjikstra()
    {
        InitDjikstra();

        while(!Grid.AllExplored())
        {
            Node n1 = MinDjikstra(Grid.Nodes);
            n1.Explored = true;
            

            foreach (var dir in neighborsDir)
            {
                Node n2 = Grid.GetNodeWithIndex(n1.X + dir.x, n1.Y + dir.y);
                if(n2 == null) continue;

                DistDjikstra(n1, n2);
            }

        }
    }
    #endregion

    List<Node> ShorterPath()
    {
        List<Node> result = new();

        Node n = EndNode.ActualNode;

        while (!n.Equals(_startNode.ActualNode))
        {
            result.Add(n);
            n = n.Pred;
            Grid.ActiveNode(n, Color.cyan);
        }
        result.Add(_startNode.ActualNode);

        Grid.ActiveNode(_startNode.ActualNode, Color.green);
        Grid.ActiveNode(EndNode.ActualNode, Color.red);

        return result;
    }

}

