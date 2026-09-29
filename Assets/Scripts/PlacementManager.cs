using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public GridSystem gridSystem;
    public GameObject buildingPrefab; // Le bâtiment à poser

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Lance un rayon depuis la souris pour trouver le sol
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // Calcule la case de la grille survolée
            Vector2Int gridPos = gridSystem.GetGridPosition(hitInfo.point);

            // Vérifie qu'on est bien dans les limites de la grille
            if (gridPos.x >= 0 && gridPos.x < gridSystem.width && gridPos.y >= 0 && gridPos.y < gridSystem.height)
            {
                // Clic gauche pour poser le bâtiment
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 spawnPos = gridSystem.GetWorldPosition(gridPos.x, gridPos.y);
                    Instantiate(buildingPrefab, spawnPos, Quaternion.identity);
                }
            }
        }
    }
}