using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGridPos : MonoBehaviour
{
    public GridSystem Grid;
    private Rigidbody _rb;
    public Node ActualNode { get; set; }
    public Node PreviousNode { get; set; }

    void Awake(){
        _rb = GetComponent<Rigidbody>();
        ActualNode = Grid.GetNode(_rb.position);
        PreviousNode = ActualNode;
    }

    public void ForceInit() {
        _rb = GetComponent<Rigidbody>();
        ActualNode = Grid.GetNode(_rb.position);
        PreviousNode = ActualNode;
    }

    void Update(){
        var node = Grid.GetNode(_rb.position);

        if (node != null)
        {
            if (!node.Equals(ActualNode))
            {
                PreviousNode = ActualNode;
                ActualNode = node;
            }
            
        }
    }
}
