using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISystem : MonoBehaviour
{
    public GridSystem Grid;
    public List<Pathfinding> Pathfindings;

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
        foreach (var node in Grid.Nodes)
        {
            node.Active = false;
        }

        foreach (var pathfinding in Pathfindings)
        {
            pathfinding.ExecutePath();
        }

    }
}
