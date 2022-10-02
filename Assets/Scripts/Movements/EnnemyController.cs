using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyController : MonoBehaviour
{
    public float Speed = 2.5f;

    private Pathfinding _brain;

    private Vector3 _target;

    private Node _previousNode;
    private Node _actualNode;

    private Rigidbody _rb;


    // Start is called before the first frame update
    void Start()
    {
        _brain = GetComponent<Pathfinding>();

        _rb = GetComponent<Rigidbody>();

        _actualNode = _brain.Grid.GetNode(transform.position);
        _previousNode = _brain.Grid.GetNode(transform.position);
    }

    void FixedUpdate()
    {
        if (_brain.Path.Count <= 0) return;

        _target = _brain.Grid.GetPos(_brain.Path[0]);
        _target.y = transform.position.y;

        var _targetRot = Quaternion.LookRotation(_target - _rb.transform.position);
        _rb.transform.rotation = Quaternion.Slerp(_rb.transform.rotation, _targetRot, Speed * Time.deltaTime);

        _rb.transform.position += transform.forward * Speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        Node node = _brain.Grid.GetNode(transform.position);
        if (node != null && node != _actualNode )
        {
            _previousNode = _actualNode;
            _actualNode = node;

            _actualNode.Accesible = false;

            _previousNode.Accesible = true;
        }
    }

    void OnDrawGizmos()
    {
        if (_brain.Path.Count == 0) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _brain.Grid.GetPos(_brain.Path[0]));
    }
}
