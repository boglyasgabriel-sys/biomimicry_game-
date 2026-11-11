using UnityEngine;

public class FloodManager : MonoBehaviour
{
    [Header("Paramètres d'inondation")]
    public Transform floodMesh;       // Glisser FloodWater ici
    public float maxWaterHeight = 0.4f; // Hauteur maximale Y en monde
    public float riseSpeed = 0.5f;     // Vitesse d'extension
    public bool isRising = false;      // Cocher pour démarrer

    private Vector3 initialScale;
    private Vector3 initialPosition;

    void Start()
    {
        if (floodMesh != null)
        {
            initialPosition = floodMesh.localPosition;
            initialScale = floodMesh.localScale;
            // On commence avec une longueur très petite pour le Quad
            floodMesh.localScale = new Vector3(initialScale.x, 0.01f, initialScale.z);
        }
    }

    void Update()
    {
        if (isRising && floodMesh != null)
        {
            Vector3 currentPos = floodMesh.localPosition;
            Vector3 currentScale = floodMesh.localScale;

            // Monte légèrement sur l'axe Y (monde)
            if (currentPos.y < initialPosition.y + maxWaterHeight)
            {
                currentPos.y += riseSpeed * 0.05f * Time.deltaTime;
                floodMesh.localPosition = currentPos;
            }

            // S'étend en longueur (axe Y local du Quad tourné)
            if (currentScale.y < initialScale.y)
            {
                currentScale.y += riseSpeed * 5f * Time.deltaTime;
                floodMesh.localScale = currentScale;
            }
        }
    }

    public void StartFlood() => isRising = true;
    public void StopFlood() => isRising = false;
}