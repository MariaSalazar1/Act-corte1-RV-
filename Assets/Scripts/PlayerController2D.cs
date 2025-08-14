using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Refs")]
    public GridManager grid;

    [Header("Movement")]
    public float moveDuration = 0.12f;   // tiempo del pasito
    private bool isMoving = false;

    [Header("State")]
    public Vector2Int gridPos;           // posición en la grilla

    private SpriteRenderer sr;
    private Color baseColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;

        // Colocar al jugador en el Start
        gridPos = grid.start;
        transform.position = grid.GridToWorld(gridPos);
    }

    void Update()
    {
        if (isMoving) return;

        Vector2Int dir = Vector2Int.zero;

        // Flechas o WASD (solo 4 direcciones)
        if (Input.GetKeyDown(KeyCode.UpArrow)    || Input.GetKeyDown(KeyCode.W)) dir = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.DownArrow)  || Input.GetKeyDown(KeyCode.S)) dir = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.LeftArrow)  || Input.GetKeyDown(KeyCode.A)) dir = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) dir = Vector2Int.right;

        if (dir == Vector2Int.zero) return;

        TryMove(dir);
    }

    void TryMove(Vector2Int dir)
    {
    Vector2Int target = gridPos + dir;
    if (!IsInBounds(target)) return;

    // ¿Cuál es el siguiente paso correcto?
    bool isCorrectNext = (stepIndex < grid.pathCells.Count && target == grid.pathCells[stepIndex]);
    bool isGoalMove    = (stepIndex == grid.pathCells.Count && target == grid.goal);

    if (!(isCorrectNext || isGoalMove))
    {
        // Paso incorrecto
        StartCoroutine(BlinkAndReset());
        return;
    }

    // Mover un paso
    StartCoroutine(MoveStep(target, isGoalMove));
    }


    IEnumerator MoveStep(Vector2Int target, bool isGoal)
    {
        isMoving = true;
        Vector3 from = transform.position;
        Vector3 to   = grid.GridToWorld(target);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }

        gridPos = target;
        isMoving = false;

        // Si pisaste el siguiente paso de la ruta, avanza el índice
        if (!isGoal && stepIndex < grid.pathCells.Count && target == grid.pathCells[stepIndex])
            stepIndex++;

        if (isGoal)
        {
            Debug.Log("¡Ganaste! Llegaste a la meta.");
            // Ejemplo: FindObjectOfType<LevelManager>().NextLevel();
        }
    }


    IEnumerator BlinkAndReset()
    {
        // Parpadeo rojo
        for (int i = 0; i < 3; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.06f);
            sr.color = baseColor;
            yield return new WaitForSeconds(0.06f);
        }

        // Volver al inicio
        gridPos = grid.start;
        transform.position = grid.GridToWorld(gridPos);
    }

    bool IsInBounds(Vector2Int p) => p.x >= 0 && p.y >= 0 && p.x < grid.width && p.y < grid.height;

    public void ResetToStart()
    {
        stepIndex = 0;
        gridPos = grid.start;
        transform.position = grid.GridToWorld(gridPos);
    }

    public int stepIndex = 0;   // próximo índice en grid.pathCells



}
