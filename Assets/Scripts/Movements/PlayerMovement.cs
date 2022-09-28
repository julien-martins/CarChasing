using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor.TextCore.Text;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 2.0f;
    public float MaxSpeed = 4.0f;

    public float SteerAngle = 50.0f;

    public float Drag = 0.98f;

    public float Traction = 10.0f;
    public bool Drifting = false;

    public TrailRenderer left;
    public TrailRenderer right;

    private Rigidbody _rb;

    private Vector3 move;

    Collider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetAxisRaw("Drift") > 0)
        {
            Traction = 1.0f;
            left.emitting = true;
            right.emitting = true;
        }
        else
        {
            Traction = 10.0f;
            left.emitting = false;
            right.emitting = false;
        }

    }

    void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if( (horizontal > 0 || vertical > 0) && !AISystem.StartIA)
        {
            AISystem.StartIA = true;
        }

        _rb.position += move * Time.deltaTime;

        move += _rb.transform.forward * vertical * MoveSpeed * Time.deltaTime;
        move *= Drag;
        move = Vector3.ClampMagnitude(move, MaxSpeed);

        if(move.magnitude > 0)
            _rb.transform.Rotate(Vector3.up * horizontal * move.magnitude * SteerAngle * Time.deltaTime);
        
        move = Vector3.Lerp(move.normalized, transform.forward, Traction * Time.deltaTime) * move.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            move *= 0.20f;
        } else if (collision.gameObject.CompareTag("Police"))
        {
            Debug.Log("Vous avez ete capture !");
        }
    }
}
