using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TileMap tileMap;
    [SerializeField] private TileMapEditor tileMapEditor;
    [SerializeField] private Button placeStartButton;
    [SerializeField] private Button placeEndButton;
    [SerializeField] private Button placeObstacleButton;
    [SerializeField] private TMP_InputField heightInputField;
    [SerializeField] private TMP_InputField widthInputField;

    public void PlaceStartButtonPressed()
    {
        tileMapEditor.PlacementMode = PlacementMode.Start;
    }

    public void PlaceEndButtonPressed()
    {
        tileMapEditor.PlacementMode = PlacementMode.End;
    }

    public void PlaceObstaclesButtonPressed()
    {
        tileMapEditor.PlacementMode = PlacementMode.Obstacles;
    }

    public void OnHeightTextChanged()
    {
        if(heightInputField.text.Length > 0)
        {
            tileMap.Height = int.Parse(heightInputField.text);
        }
        else
        {
            Debug.Log("0");
            tileMap.Height = 0;
        }
    }

    public void OnWidthTextChanged()
    {
        if (widthInputField.text.Length > 0)
        {
            tileMap.Width = int.Parse(widthInputField.text);
        }
        else
        {
            Debug.Log("0");
            tileMap.Width = 0;
        }
        
    }

    public void GenerateMapButtonPressed()
    {
        tileMap.TryDestroyMap();
        tileMap.GenerateMap();
    }

    public void SearchForPathButtonPressed()
    {
        tileMapEditor.FindAndShowPath();
    }

    public void AStarButtonPressed()
    {
        tileMapEditor.PathfindingMode = PathfindingMode.AStar;
    }

    public void JPSButtonPressed()
    {
        tileMapEditor.PathfindingMode = PathfindingMode.JumpPointSearch;
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }
}
