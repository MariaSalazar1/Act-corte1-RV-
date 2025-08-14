using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Grid")]
    public int width = 5;
    public int height = 5;
    public float cellSize = 1f;
    public Vector2 origin = Vector2.zero;
    public GameObject tilePrefab;

    [Header("Start / Goal (grid coords)")]
    public Vector2Int start = new Vector2Int(0, 0);
    public Vector2Int goal  = new Vector2Int(4, 4);

    [HideInInspector] public Tile[,] tiles;
    [HideInInspector] public List<Vector2Int> pathCells = new List<Vector2Int>(); // SOLO ruta (sin start/goal)

    void Start()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        // Limpia hijos (tiles anteriores)
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        GenerateGrid();
        GeneratePath();
    }

    void GenerateGrid()
    {
        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = (Vector3)origin + new Vector3(x * cellSize, y * cellSize, 0f);
                GameObject go = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                go.name = $"Tile {x},{y}";

                Tile t = go.GetComponent<Tile>();
                t.SetType(TileType.Normal);
                tiles[x, y] = t;
            }
        }
    }

    void GeneratePath()
    {
        pathCells.Clear();
        if (!InBounds(start) || !InBounds(goal)) { Debug.LogWarning("Start/Goal fuera de rango"); return; }

        tiles[start.x, start.y].SetType(TileType.Start);
        tiles[goal.x,  goal.y].SetType(TileType.Goal);

        List<Vector2Int> steps = new List<Vector2Int>();
        int dx = goal.x - start.x, dy = goal.y - start.y;
        int sx = dx >= 0 ? 1 : -1, sy = dy >= 0 ? 1 : -1;

        for (int i = 0; i < Mathf.Abs(dx); i++) steps.Add(new Vector2Int(sx, 0));
        for (int i = 0; i < Mathf.Abs(dy); i++) steps.Add(new Vector2Int(0, sy));

        // shuffle
        for (int i = 0; i < steps.Count; i++)
        {
            int r = Random.Range(i, steps.Count);
            (steps[i], steps[r]) = (steps[r], steps[i]);
        }

        Vector2Int current = start;
        foreach (var step in steps)
        {
            current += step;
            if (current == start || current == goal) continue;
            pathCells.Add(current);
            tiles[current.x, current.y].SetType(TileType.Path);
        }
    }

    public void HidePathToNormal()
    {
        foreach (var p in pathCells)
            tiles[p.x, p.y].SetType(TileType.Normal);
    }

    public Vector3 GridToWorld(Vector2Int g)
    {
        return (Vector3)origin + new Vector3(g.x * cellSize, g.y * cellSize, 0f);
    }

    public TileType GetTileType(Vector2Int g)
    {
        if (!InBounds(g)) return TileType.Normal;
        return tiles[g.x, g.y].type;
    }

    public bool InBounds(Vector2Int p) => p.x >= 0 && p.y >= 0 && p.x < width && p.y < height;

    public bool IsPathCell(Vector2Int g) => pathCells.Contains(g);

}
