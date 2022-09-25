using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AlgorithmType
{
    Astar,
    Djikstra
}

public class Pathfinding : MonoBehaviour
{
    private Rigidbody _rb;
    private Node _actualNode;
    private Node _previousNode;

    public GridSystem Grid;
    public PlayerMovement Player;
    public AlgorithmType Algorithm;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _actualNode = Grid.GetNode(_rb.position);
        Grid.ActiveNode(_actualNode, Color.red);
        _previousNode = _actualNode;
    }

    // Update is called once per frame
    void Update()
    {
        var node = Grid.GetNode(_rb.position); 
        if (node != null)
        {
            if (!node.Equals(_actualNode))
            {
                Grid.ActiveNode(node, Color.red);
                Grid.DesactiveNode(_actualNode);

                _previousNode = _actualNode;
                _actualNode = node;
            }
        }
    }
}
