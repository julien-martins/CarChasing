using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        if (!DrawGrid) return;
        if(Nodes == null || Nodes.Count == 0) return;
        
        foreach(var node in Nodes) {
            Gizmos.color = (!node.Active) ? Color.blue : node.ActiveColor;
            
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
                Nodes.Add(new Node(i * _width + j, i, j));
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

        return Nodes[(int) posIndex.z * _width + (int) posIndex.x];
    }

    public void ActiveNode(Node node, Color color)
    {
        node.Active = true;
        node.ActiveColor = color;
    }

    public void DesactiveNode(Node node) { node.Active = false; }

}
