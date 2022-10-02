using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    public GridSystem Grid;
    public GameObject CoinPrefab;

    public float Timer = 2.0f;
    private float _timerProgress = 0;

    public int nbOfCoinsAtSameTime = 10;

    private int nbOfCoins = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nbOfCoins >= nbOfCoinsAtSameTime) return;

        _timerProgress += Time.deltaTime;

        if (_timerProgress >= Timer)
        {

            int x = Random.Range(0, Grid.GetWidth());
            int y = Random.Range(0, Grid.GetHeight());
            Node node = Grid.GetNodeWithIndex(x, y);

            while (!node.Accesible)
            {
                x = Random.Range(0, Grid.GetWidth());
                y = Random.Range(0, Grid.GetHeight());
                node = Grid.GetNodeWithIndex(x, y);
            }

            GameObject.Instantiate(CoinPrefab, Grid.GetPos(node) + Vector3.up * 0.5f, Quaternion.Euler(0, 0, 90), this.transform);
            nbOfCoins++;

            _timerProgress = 0;
        }

    }

    public void RemoveCoin() => nbOfCoins--;
}
