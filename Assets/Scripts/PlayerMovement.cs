using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GridSystem Grid;

    private Rigidbody _rb;

    private Node _previousNode;
    public Node ActualNode { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        ActualNode = Grid.GetNode(_rb.position);
        Grid.ActiveNode(ActualNode, Color.green);
        _previousNode = ActualNode;
    }

    void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        Camera cam = Camera.main;

        Vector3 move = cam.transform.right * horizontal + cam.transform.forward * vertical;

        _rb.AddForce(move);
    }
    
    // Update is called once per frame
    void Update()
    {
        var node = Grid.GetNode(_rb.position);

        if (node != null)
        {
            if (!node.Equals(ActualNode))
            {
                Grid.ActiveNode(node, Color.green);
                Grid.DesactiveNode(ActualNode);

                _previousNode = ActualNode;
                ActualNode = node;
            }
            
        }
    }
}
