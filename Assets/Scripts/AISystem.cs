using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AISystem : MonoBehaviour
{
    public GridSystem Grid;
    public List<Pathfinding> Pathfindings;

    public static bool StartIA = false;
    public bool MovingIA = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var pathfinding in Pathfindings)
        {
            pathfinding.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!StartIA) return;

        foreach (var node in Grid.Nodes)
        {
            node.Active = false;
        }

        foreach (var pathfinding in Pathfindings)
        {
            pathfinding.ExecutePath();
        }

        if (MovingIA)
        {
            foreach (var pathfinding in Pathfindings)
            {
                pathfinding.MoveNextNode();
            }
        }

    }
}
