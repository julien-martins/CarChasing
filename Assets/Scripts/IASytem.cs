using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IASytem : MonoBehaviour
{
    public GridSystem Grid;
    public List<Pathfinding> Brains;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Node node in Grid.Nodes)
        {
            node.Active = false;
        }

        foreach (Pathfinding pathfinding in Brains)
        {
            pathfinding.ExecutePath();
        }
    }
}
