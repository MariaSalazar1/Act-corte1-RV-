using UnityEngine;

public enum TileType { Normal, Path, Start, Goal }

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [Header("Colors")]
    public Color normalColor = new Color32(45, 50, 60, 255);
    public Color pathColor   = new Color32(75, 210, 120, 255);
    public Color startColor  = new Color32(255, 190, 80, 255);
    public Color goalColor   = new Color32(80, 170, 255, 255);

    private SpriteRenderer sr;
    public TileType type = TileType.Normal;

    void Awake() => sr = GetComponent<SpriteRenderer>();

    public void SetType(TileType newType)
    {
        type = newType;
        switch (type)
        {
            case TileType.Normal: sr.color = normalColor; break;
            case TileType.Path:   sr.color = pathColor;   break;
            case TileType.Start:  sr.color = startColor;  break;
            case TileType.Goal:   sr.color = goalColor;   break;
        }
        
    }
}
