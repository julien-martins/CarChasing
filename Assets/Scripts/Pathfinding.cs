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

    private bool once = false;
    // Start is called before the first frame update
    void Start()
    {
        _startNode = GetComponent<EntityGridPos>();
        _rb = GetComponent<Rigidbody>();
        
        Grid.ActiveNode(_startNode.ActualNode, Color.green);
        Grid.ActiveNode(EndNode.ActualNode, Color.red);
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (Algorithm)
        {
            case AlgorithmType.Astar:
                UpdateAstar();
                break;
            case AlgorithmType.Djikstra:
                UpdateDjikstra();
                ShorterPathDjikstra();
                break;
        }
    }

    #region Astar
    void UpdateAstar(){

    }
    #endregion

    #region Djikstra

    void InitDjikstra()
    {
        foreach (Node n in Grid.Nodes)
        {
            n.Distance = int.MaxValue;
            n.Explored = false;
            n.Active = false;
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
        List<Vector2Int> neighborsDir = new() { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };

        InitDjikstra();

        
        //List<Node> Q = new(Grid.Nodes);
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

    List<Node> ShorterPathDjikstra()
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
    #endregion

}

