using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;


public class GridSystem : MonoBehaviour
{
    private int _width;
    private int _height;

    private Vector3 _offset;

    public List<Node> Nodes { get; private set; }

    public bool DrawGrid = false;

    // Start is called before the first frame update
    void Awake()
    {
        var bounds = GetComponent<MeshFilter>().mesh.bounds;

        _width = (int) (transform.localScale.x * bounds.size.x);
        _height = (int) (transform.localScale.z * bounds.size.z);

        _offset = new Vector3(_width / 2 - 0.5f, 0, _height / 2 - 0.5f);

        Nodes = new();

        CreateGrid();
    }

    void OnDrawGizmos()
    {
        if (!DrawGrid) return;
        if(Nodes == null || Nodes.Count == 0) return;
        
        foreach(var node in Nodes) {
            Color color = Color.blue;
            if (node.Active)
            {
                if (node.Accesible) color = node.ActiveColor;
                else color = Color.magenta;
            }
            Gizmos.color = color;

            var center = new Vector3(node.X, 0, node.Y) - _offset;
            var size = new Vector3(0.5f, 0.5f, 0.5f);

            Gizmos.DrawCube(center, size);
        }

    }

    void CreateGrid()
    {
        Nodes.Clear();

        for (int j = 0; j < _height; ++j)
        {
            for (int i = 0; i < _width; ++i)
            {
                Node nodeToAdd = new Node(i * _width + j, i, j);
                //Test if there are object top of it
                Collider[] colliders = Physics.OverlapBox(new Vector3(i, 0.0f, j) - _offset, new Vector3(0.5f, 0.5f, 0.5f));
                foreach(Collider collider in colliders)
                {
                    if (collider.CompareTag("Obstacle"))
                    {
                        nodeToAdd.Accesible = false;
                    }
                }

                Nodes.Add(nodeToAdd);
            }
        }

    }

    [CanBeNull]
    public Node GetNode(float x, float y)
    {
        return GetNode(new Vector3(x, 0, y));
    }

    [CanBeNull]
    public Node GetNode(Vector3 pos)
    {
        var posIndex = Vector3Int.RoundToInt(pos + _offset);

        if (posIndex.x < 0 || posIndex.z < 0 || posIndex.x >= _width || posIndex.z >= _height) return null;

        return Nodes[(int) posIndex.z * _width + (int) posIndex.x];
    }

    public Vector3 GetPos(Node n)
    {
        return new Vector3(n.X, 0, n.Y) - _offset;
    }

    public Node GetNodeWithIndex(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height) return null; 
        
        return Nodes[y * _width + x];
    }

    public void ActiveNode(Node node, Color color)
    {
        node.Active = true;
        node.ActiveColor = color;
    }

    public void DesactiveNode(Node node) { node.Active = false; }

    public bool AllExplored()
    {
        foreach (Node n in Nodes)
        {
            if (!n.Explored) return false;
        }

        return true;
    }
    public void ShowAllDistanceNodes()
    {
        foreach (Node n in Nodes)
        {
            Debug.Log(n);
        }
    }
}
