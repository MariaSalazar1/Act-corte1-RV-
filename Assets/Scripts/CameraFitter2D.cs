using UnityEngine;

public class CameraFitter2D : MonoBehaviour
{
    [Header("Manual grid size (use this if you don't have GridManager yet)")]
    public int gridWidth = 5;
    public int gridHeight = 5;

    [Header("Origin of the grid in world units")]
    public Vector2 origin = Vector2.zero;

    [Header("Extra border space")]
    public float padding = 0.6f;

    void LateUpdate()
    {
        Camera cam = GetComponent<Camera>();
        if (cam == null) return;

        // Center of the grid assuming cells are 1x1 starting at 'origin'
        Vector2 center = origin + new Vector2((gridWidth - 1) * 0.5f, (gridHeight - 1) * 0.5f);
        transform.position = new Vector3(center.x, center.y, -10f);

        float halfW = gridWidth * 0.5f;
        float halfH = gridHeight * 0.5f;
        float targetSize = Mathf.Max(halfH, halfW / cam.aspect) + padding;

        // Lerp suave; puedes usar 1f para ajuste instantáneo
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, 0.25f);
    }
}
