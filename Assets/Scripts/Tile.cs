using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Renderer tileRenderer;
    [SerializeField] private Color walkableColor = Color.green;
    [SerializeField] private Color unwalkableColor = Color.gray;
    [SerializeField] private TextMeshPro text;

    public Node Node { get; set; }
    public int X { get; set; }
    public int Z { get; set; }

    public void ChangeWalkableState()
    {
        if (Node.IsWalkable)
        {
            Node.IsWalkable = false;
            tileRenderer.material.color = unwalkableColor;
        }
        else
        {
            Node.IsWalkable = true;
            tileRenderer.material.color = walkableColor;
        }
    }

    public void ChangeText(string str)
    {
        text.text = str;
    }

    public Renderer GetRenderer()
    {
        return tileRenderer;
    }

    public void ResetColor()
    {
        if (Node.IsWalkable)
        {
            tileRenderer.material.color = walkableColor;
        }
        else
        {
            tileRenderer.material.color = unwalkableColor;
        }
    }
}