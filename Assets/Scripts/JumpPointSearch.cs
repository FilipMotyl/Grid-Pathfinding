using System.Collections.Generic;
using UnityEngine;
/*
 * 
 * NOT FINISHED YET
 */
public class JumpPointSearch : MonoBehaviour
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

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, currentNode);
            }

            IdentifySuccessors(currentNode, targetNode, openSet, closedSet);
        }

        return new List<Tile>();
    }

    void IdentifySuccessors(Node node, Node targetNode, MinHeapPriorityQueue<Node> openSet, HashSet<Node> closedSet)
    {
        foreach (Node neighbor in PrunedNeighbors(node))
        {
            Node jumpPoint = Jump(neighbor, node, targetNode);
            if (jumpPoint != null && !closedSet.Contains(jumpPoint))
            {
                int newCost = node.GCost + GetDistance(node, jumpPoint);
                if (newCost < jumpPoint.GCost || !openSet.Contains(jumpPoint))
                {
                    jumpPoint.GCost = newCost;
                    jumpPoint.HCost = GetDistance(jumpPoint, targetNode);
                    jumpPoint.Parent = node;

                    if (!openSet.Contains(jumpPoint))
                    {
                        openSet.Enqueue(jumpPoint);
                    }
                    else
                    {
                        openSet.UpdateItem(jumpPoint);
                    }
                }
            }
        }
    }

    Node Jump(Node currentNode, Node parent, Node targetNode)
    {
        if (currentNode == null || !currentNode.IsWalkable)
            return null;

        if (currentNode == targetNode)
            return currentNode;

        if (HasForcedNeighbor(currentNode, parent))
            return currentNode;

        int dx = currentNode.Tile.X - parent.Tile.X;
        int dz = currentNode.Tile.Z - parent.Tile.Z;

        if (dx != 0)
        {
            if (IsWalkable(currentNode.Tile.X + dx, currentNode.Tile.Z) && !IsWalkable(currentNode.Tile.X + dx, currentNode.Tile.Z + 1) ||
                IsWalkable(currentNode.Tile.X + dx, currentNode.Tile.Z) && !IsWalkable(currentNode.Tile.X + dx, currentNode.Tile.Z - 1))
            {
                return currentNode;
            }
        }
        else if (dz != 0)
        {
            if (IsWalkable(currentNode.Tile.X, currentNode.Tile.Z + dz) && !IsWalkable(currentNode.Tile.X + 1, currentNode.Tile.Z + dz) ||
                IsWalkable(currentNode.Tile.X, currentNode.Tile.Z + dz) && !IsWalkable(currentNode.Tile.X - 1, currentNode.Tile.Z + dz))
            {
                return currentNode;
            }
        }

        if (dx != 0 && dz == 0)
        {
            if (Jump(tileMap.GetTile(currentNode.Tile.X + dx, currentNode.Tile.Z)?.Node, currentNode, targetNode) != null)
                return currentNode;
        }
        else if (dz != 0 && dx == 0)
        {
            if (Jump(tileMap.GetTile(currentNode.Tile.X, currentNode.Tile.Z + dz)?.Node, currentNode, targetNode) != null)
                return currentNode;
        }

        return null;
    }

    bool HasForcedNeighbor(Node currentNode, Node parent)
    {
        int dx = currentNode.Tile.X - parent.Tile.X;
        int dz = currentNode.Tile.Z - parent.Tile.Z;

        if (dx != 0)
        {
            if (IsWalkable(currentNode.Tile.X, currentNode.Tile.Z + 1) && !IsWalkable(currentNode.Tile.X - dx, currentNode.Tile.Z + 1))
                return true;
            if (IsWalkable(currentNode.Tile.X, currentNode.Tile.Z - 1) && !IsWalkable(currentNode.Tile.X - dx, currentNode.Tile.Z - 1))
                return true;
        }
        else if (dz != 0)
        {
            if (IsWalkable(currentNode.Tile.X + 1, currentNode.Tile.Z) && !IsWalkable(currentNode.Tile.X + 1, currentNode.Tile.Z - dz))
                return true;
            if (IsWalkable(currentNode.Tile.X - 1, currentNode.Tile.Z) && !IsWalkable(currentNode.Tile.X - 1, currentNode.Tile.Z - dz))
                return true;
        }

        return false;
    }

    bool IsWalkable(int x, int z)
    {
        Tile tile = tileMap.GetTile(x, z);
        return tile != null && tile.Node.IsWalkable;
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

    List<Node> PrunedNeighbors(Node node)
    {
        List<Node> pruned = new List<Node>();
        Node parent = node.Parent;

        if (parent != null)
        {
            int dx = Mathf.Clamp(node.Tile.X - parent.Tile.X, -1, 1);
            int dz = Mathf.Clamp(node.Tile.Z - parent.Tile.Z, -1, 1);

            if (dx != 0)
            {
                if (IsWalkable(node.Tile.X + dx, node.Tile.Z))
                {
                    pruned.Add(tileMap.GetTile(node.Tile.X + dx, node.Tile.Z).Node);
                }
                if (IsWalkable(node.Tile.X, node.Tile.Z + 1))
                {
                    pruned.Add(tileMap.GetTile(node.Tile.X, node.Tile.Z + 1).Node);
                }
                if (IsWalkable(node.Tile.X, node.Tile.Z - 1))
                {
                    pruned.Add(tileMap.GetTile(node.Tile.X, node.Tile.Z - 1).Node);
                }
            }
            else if (dz != 0)
            {
                if (IsWalkable(node.Tile.X, node.Tile.Z + dz))
                {
                    pruned.Add(tileMap.GetTile(node.Tile.X, node.Tile.Z + dz).Node);
                }
                if (IsWalkable(node.Tile.X + 1, node.Tile.Z))
                {
                    pruned.Add(tileMap.GetTile(node.Tile.X + 1, node.Tile.Z).Node);
                }
                if (IsWalkable(node.Tile.X - 1, node.Tile.Z))
                {
                    pruned.Add(tileMap.GetTile(node.Tile.X - 1, node.Tile.Z).Node);
                }
            }
        }
        else
        {
            pruned = node.Neighbors;
        }

        return pruned;
    }
}