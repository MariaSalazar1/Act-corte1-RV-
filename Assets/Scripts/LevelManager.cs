using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Refs")]
    public GridManager grid;
    public PlayerController2D player;

    [Header("Timing")]
    public float showDelay = 0.2f;   // tiempo entre encender cada tile
    public float startDelay = 0.5f;  // espera antes de empezar a mostrar

    void Start()
    {
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        // Coloca al player en Start y lo bloquea
        player.enabled = false;
        player.ResetToStart();

        yield return new WaitForSeconds(startDelay);

        // Mostrar ruta: iluminar tiles de path uno a uno
        foreach (var p in grid.pathCells)
        {
            grid.tiles[p.x, p.y].SetType(TileType.Path);
            yield return new WaitForSeconds(showDelay);
        }

        // Espera un momento antes de ocultar
        yield return new WaitForSeconds(0.5f);

        // Oculta tiles (solo deja Start y Goal)
        grid.HidePathToNormal();

        // Activa el player para que empiece a jugar
        player.enabled = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        // Aquí puedes aumentar tamaño/dificultad y regenerar
        grid.width++;
        grid.height++;
        grid.goal = new Vector2Int(grid.width - 1, grid.height - 1);
        grid.Rebuild();
        StartCoroutine(StartLevel());
    }
}
