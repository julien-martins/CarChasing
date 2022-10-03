using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor.SearchService;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public CoinGenerator CoinGenerator;

    public ScoreManager ScoreManager;

    public CameraShake Shake;

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

    private Player _player;

    Collider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
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
        
        _rb.position += move * Time.deltaTime;

        move += _rb.transform.forward * vertical * MoveSpeed * Time.deltaTime;
        move *= Drag;
        move = Vector3.ClampMagnitude(move, MaxSpeed);

        if(move.magnitude > 0)
            _rb.transform.Rotate(Vector3.up * horizontal * move.magnitude * SteerAngle * Time.deltaTime);
        
        move = Vector3.Lerp(move.normalized, transform.forward, Traction * Time.deltaTime) * move.magnitude;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Coin"))
        {
            Object.Destroy(collider.gameObject);
            ScoreManager.AddScore(10);
            CoinGenerator.RemoveCoin();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            move *= 0.20f;

            Shake.Start = true;
        } else if (collision.gameObject.CompareTag("Police"))
        {
            Shake.Start = true;

            _player.RemoveLife();
            if (_player.getCurrentLife() == 0)
            {
                Time.timeScale = 0;
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
                Time.timeScale = 1;
            }
        }
    }

}
