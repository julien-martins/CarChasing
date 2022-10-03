using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Live = 3;
    public float InvinsibleTime = 2.0f;
    private int _currentLive;

    private bool _invinsible = false;
    private float _timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _currentLive = Live;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_invinsible) return;

        _timer += Time.deltaTime;
        if (_timer >= InvinsibleTime)
        {
            _invinsible = false;
            _timer = 0f;
        }
    }

    public void RemoveLife()
    {
        if (_invinsible) return;
        
        _currentLive--;
        _invinsible = true;
    }

    public int getMaxLife() => Live;
    public int getCurrentLife() => _currentLive;
}
