using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node : IComparable<Node>
{
    public Tile Tile;
    public bool IsWalkable;
    public int GCost;
    public int HCost;
    public Node Parent;
    public List<Node> Neighbors { get; } = new List<Node>();

    public Node(Tile tile)
    {
        this.Tile = tile;
        IsWalkable = true;
    }

    public int FCost => GCost + HCost;

    public int CompareTo(Node other)
    {
        int compare = FCost.CompareTo(other.FCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(other.HCost);
        }
        return compare;
    }
  
}