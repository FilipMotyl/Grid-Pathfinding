using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [SerializeField] private TileMap tileMap;

    public List<Tile> FindPath(Tile startTile, Tile targetTile)
    {
        Node startNode = startTile.Node;
        Node targetNode = targetTile.Node;

        MinHeapPriorityQueue<Node> openSet = new MinHeapPriorityQueue<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Enqueue(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.Dequeue();
            closedSet.Add(currentNode);

            if (currentNode.Tile == targetTile)
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (Node neighbour in currentNode.Neighbors)
            {

                if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Enqueue(neighbour);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }

        return new List<Tile>();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.Tile.X - nodeB.Tile.X);
        int dstZ = Mathf.Abs(nodeA.Tile.Z - nodeB.Tile.Z);
        return dstX + dstZ;
    }


    List<Tile> RetracePath(Node startNode, Node endNode)
    {
        List<Tile> path = new List<Tile>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.Tile);
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        return path;
    }
}