using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public enum Algorithm
{
    Astar,
    Djikstra
}

public class Pathfinding : MonoBehaviour
{
    public GridSystem Grid;

    public Transform startPos;
    public Transform endPos;

    public Algorithm Algorithm;
    
    public void ExecutePath()
    {
        if (Algorithm == Algorithm.Astar)
            FindPathAstar(startPos.position, endPos.position);
        else if(Algorithm == Algorithm.Djikstra)
            FindPathDjikstra(startPos.position, endPos.position);
    }

    #region Astar

    public List<Node> FindPathAstar(Vector3 startPos, Vector3 targetPos)
    {
        Node start = Grid.GetNode(startPos);
        Node end = Grid.GetNode(targetPos);

        List<Node> open = new();
        HashSet<Node> closed = new();

        open.Add(start);

        while (open.Count > 0)
        {
            Node currentNode = open[0];
            for (int i = 1; i < open.Count; ++i)
            {
                if (open[i].Heuristique < currentNode.Heuristique)
                    currentNode = open[i];
            }

            open.Remove(currentNode);
            closed.Add(currentNode);

            if (currentNode == end)
            {
                return RetracePath(start, end);
            }

            foreach (Node neighbour in Grid.GetNeighbours(currentNode))
            {
                if (!neighbour.Accesible || closed.Contains(neighbour)) continue;

                int newCost = currentNode.Cout + 1;
                if (newCost < neighbour.Cout || !open.Contains(neighbour))
                {
                    neighbour.Cout = newCost;
                    neighbour.Heuristique = neighbour.Cout + GetDistance(neighbour, end);
                    neighbour.Pred = currentNode;

                    if (!open.Contains(neighbour))
                        open.Add(neighbour);
                }

            }

        }

        return new();
    }
    int GetDistance(Node a, Node b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    #endregion

    #region Djikstra

    public List<Node> FindPathDjikstra(Vector3 startPos, Vector3 endPos)
    {
        Node start = Grid.GetNode(startPos);
        Node end = Grid.GetNode(endPos);

        List<Node> Q = new();

        //Initialisation
        foreach (Node node in Grid.Nodes)
        {
            node.Distance = Int32.MaxValue;
            node.Poids = 1;
            node.Pred = null;
            node.Explored = false;
            
            if(node.Accesible) Q.Add(node);
        }

        start.Distance = 0;

        while (Q.Count > 0)
        {
            //Find the node with the smallest distance
            Q.Sort((n1, n2) => n1.Distance.CompareTo(n2.Distance));
            Node current = Q[0];
            Q.Remove(current);

            //Update neightbors's distance of the current nodes
            List<Node> neightbors = new();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    int checkX = current.X + x;
                    int checkY = current.Y + y;

                    if (checkX >= 0 && checkX < Grid.GetWidth() && checkY >= 0 && checkY < Grid.GetHeight())
                    {
                        Node node = Grid.GetNodeWithIndex(checkX, checkY);
                        neightbors.Add(node);
                    }
                }
            }

            if (current == end) break;

            foreach (Node neighbor in neightbors)
            {
                if (!(Q.Contains(neighbor) || neighbor.Accesible)) continue;

                if (neighbor.Distance > current.Distance + current.Poids)
                {
                    neighbor.Distance = current.Distance + current.Poids;
                    neighbor.Pred = current;
                }
            }

        }

        return RetracePath(start, end);
    }

    #endregion
    List<Node> RetracePath(Node start, Node end)
    {
        List<Node> result = new();

        Node current = end;
        while (current != start)
        {
            result.Add(current);

            current.Active = true;
            current.ActiveColor = Color.cyan;

            current = current.Pred;
        }

        result.Reverse();

        return result;
    }
}

