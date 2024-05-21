using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Vector3 startingPosition;

    public int Width;
    public int Height;

    [SerializeField] private float tileSize;

    private int currentWidth;
    private int currentHeight;
    private Tile[,] tiles;

    public void GenerateMap()
    {
        if (Height <= 0 && Width <= 0)
        {
            return;
        }

        currentHeight = Height;
        currentWidth = Width;
        tiles = new Tile[currentWidth, currentHeight];
        for (int x = 0; x < currentWidth; x++)
        {
            for (int z = 0; z < currentHeight; z++)
            {
                tiles[x, z] = Instantiate(tilePrefab, GetWorldPosition(x, z) + new Vector3(tileSize, 0, tileSize) * 0.5f, Quaternion.identity, transform).GetComponent<Tile>();
                tiles[x, z].name = $"Tile {x},{z}";
                tiles[x, z].transform.localScale = new Vector3(tiles[x, z].transform.localScale.x * tileSize, tiles[x, z].transform.localScale.y, tiles[x, z].transform.localScale.z * tileSize);
                tiles[x, z].Node = new Node(tiles[x, z]);
                tiles[x, z].X = x;
                tiles[x, z].Z = z;
            }
        }

        for (int x = 0; x < currentWidth; x++)
        {
            for (int z = 0; z < currentHeight; z++)
            {
                Node currentNode = tiles[x, z].Node;

                if (x > 0) currentNode.Neighbors.Add(tiles[x - 1, z].Node);
                if (x < currentWidth - 1) currentNode.Neighbors.Add(tiles[x + 1, z].Node);
                if (z > 0) currentNode.Neighbors.Add(tiles[x, z - 1].Node);
                if (z < currentHeight - 1) currentNode.Neighbors.Add(tiles[x, z + 1].Node); 
            }
        }
    }

    public void TryDestroyMap()
    {
        if (tiles != null && tiles.GetLength(0) > 0 && tiles.GetLength(1) > 0)
        {
            for (int x = 0; x < currentWidth; x++)
            {
                for (int z = 0; z < currentHeight; z++)
                {
                    Destroy(tiles[x, z].gameObject);
                }
            }
            tiles = null;
        }
    }

    public Tile GetTile(int x, int z)
    {
        if (x >= 0 && x < currentWidth && z >= 0 && z < currentHeight)
        {
            return tiles[x, z];
        }
        return null;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition.x - startingPosition.x) / tileSize);
        z = Mathf.FloorToInt((worldPosition.z - startingPosition.z) / tileSize);
    }

    private Vector3 GetWorldPosition(int x, int z)
    {
        return startingPosition + (new Vector3(x, 0, z) * tileSize);
    }
}