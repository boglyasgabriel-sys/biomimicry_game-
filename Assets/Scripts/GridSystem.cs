using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [Header("Paramètres de la Grille")]
    public int width = 20;         // Nombre de cases en X
    public int height = 20;        // Nombre de cases en Z
    public float cellSize = 1.5f;   // Taille d'une case en mètres

    [Header("Visuel (Optionnel)")]
    public Color gridColor = Color.cyan;

    // Convertit une position du monde 3D en coordonnées de grille (Vector2Int)
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int z = Mathf.FloorToInt(worldPosition.z / cellSize);
        return new Vector2Int(x, z);
    }

    // Convertit des coordonnées de grille en position dans le monde 3D (centre de la case)
    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x * cellSize + cellSize / 2f, 0, z * cellSize + cellSize / 2f);
    }

    // Dessine la grille dans la vue Scene pour t'aider à visualiser les limites
    private void OnDrawGizmos()
    {
        Gizmos.color = gridColor;
        for (int x = 0; x <= width; x++)
        {
            Vector3 start = transform.position + new Vector3(x * cellSize, 0, 0);
            Vector3 end = start + new Vector3(0, 0, height * cellSize);
            Gizmos.DrawLine(start, end);
        }

        for (int z = 0; z <= height; z++)
        {
            Vector3 start = transform.position + new Vector3(0, 0, z * cellSize);
            Vector3 end = start + new Vector3(width * cellSize, 0, 0);
            Gizmos.DrawLine(start, end);
        }
    }
}