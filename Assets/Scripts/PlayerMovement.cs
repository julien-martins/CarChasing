using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GridSystem grid;

    private Rigidbody _rb;

    private Node? _previousNode;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
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
        var node = grid.GetNode(_rb.position);

        if (node != null)
        {
            grid.ActiveNode(node);
        }
    }
}
