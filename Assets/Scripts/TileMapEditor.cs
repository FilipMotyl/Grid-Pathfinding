using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileMapEditor : MonoBehaviour
{
    [SerializeField] private TileMap tileMap;
    [SerializeField] private AStar aStar;
    [SerializeField] private JumpPointSearch jps;
    [SerializeField] private LayerMask tileLayer;

    private Tile startTile;
    private Tile endTile;
    private List<Tile> path = new List<Tile>();

    public PlacementMode PlacementMode = PlacementMode.Start;
    public PathfindingMode PathfindingMode = PathfindingMode.AStar;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    public void FindAndShowPath()
    {
        if (startTile != null && endTile != null)
        {
            if (PathfindingMode == PathfindingMode.AStar)
            {
                path = aStar.FindPath(startTile, endTile);
            }
            else if (PathfindingMode == PathfindingMode.JumpPointSearch)
            {
                path = jps.FindPath(startTile, endTile);
            }
            

            if (path.Count > 0)
            {
                foreach (Tile tile in path)
                {
                    tile.GetRenderer().material.color = Color.green;
                }
                startTile.GetRenderer().material.color = Color.green;
            }
        }
    }

    private void HandleMouseClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, tileLayer))
        {
            tileMap.GetXZ(hit.point, out int x, out int z);

            Tile clickedTile = hit.collider.GetComponent<Tile>();

            if (clickedTile != null)
            {
                ResetPath();

                switch (PlacementMode)
                {
                    case PlacementMode.Start:
                        PlaceStart(clickedTile);
                        break;
                    case PlacementMode.End:
                        PlaceEnd(clickedTile);
                        break;
                    case PlacementMode.Obstacles:
                        PlaceOrRemoveObstacle(clickedTile);
                        break;
                }
            }
        }
    }
    
    private void PlaceStart(Tile clickedTile)
    {
        if (startTile != null)
        {
            startTile.ChangeText("");
            startTile = null;
        }

        if (!clickedTile.Node.IsWalkable)
        {
            clickedTile.ChangeWalkableState();
        }
        else if (clickedTile == endTile)
        {
            endTile = null;
        }
        startTile = clickedTile;
        startTile.ChangeText("S");
    }

    private void PlaceEnd(Tile clickedTile)
    {
        if (endTile != null)
        {
            endTile.ChangeText("");
            endTile = null;
        }

        if (!clickedTile.Node.IsWalkable)
        {
            clickedTile.ChangeWalkableState();
        }
        else if (clickedTile == startTile)
        {
            startTile = null;
        }
        endTile = clickedTile;
        endTile.ChangeText("E");
    }

    private void PlaceOrRemoveObstacle(Tile clickedTile)
    { 
        if (startTile && clickedTile == startTile)
        {
            startTile.ChangeText("");
            startTile = null;
        }
        else if (endTile && clickedTile == endTile)
        {
            endTile.ChangeText("");
            endTile = null;
        }
        clickedTile.ChangeWalkableState();
    }

    private void ResetPath()
    {
        if (path.Count > 0)
        {
            if (startTile != null)
            {
                startTile.ResetColor();
            }

            foreach (Tile tile in path)
            {
                if (tile != null)
                {
                    tile.ResetColor();
                }
            }
            path.Clear();
        }
    }
}