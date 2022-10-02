using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyController : MonoBehaviour
{
    public float Speed = 2.5f;

    private Pathfinding _brain;

    private Vector3 _target;

    // Start is called before the first frame update
    void Start()
    {
        _brain = GetComponent<Pathfinding>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_brain.Path.Count <= 0) return;

        _target = _brain.Grid.GetPos(_brain.Path[0]);
        _target.y = transform.position.y;
        var _targetRot = Quaternion.LookRotation(_target - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot, Speed * Time.deltaTime);

        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _brain.Grid.GetPos(_brain.Path[0]));
    }
}
