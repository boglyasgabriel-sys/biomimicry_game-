using UnityEngine;

[RequireComponent(typeof(Light))]
[ExecuteAlways] // Applique les changements directement dans l'éditeur sans lancer le jeu !
public class QuizBeaconSetup : MonoBehaviour
{
    [Header("Paramètres du Faisceau")]
    public Color beaconColor = new Color(0.2f, 1f, 0.4f); // Vert vif/subtil comme ton croquis
    public float range = 150f;          // Monte très haut dans le ciel
    public float spotAngle = 12f;        // Faisceau cylindrique / très serré
    public float intensity = 80f;        // Intensité lumineuse
    public bool lightUpwards = true;     // Oriente vers le haut

    void OnValidate()
    {
        ApplyBeaconSettings();
    }

    void Start()
    {
        ApplyBeaconSettings();
    }

    public void ApplyBeaconSettings()
    {
        Light lightComp = GetComponent<Light>();

        // 1. Définir le type Spot
        lightComp.type = LightType.Spot;

        // 2. Orienter vers le haut (ou vers le bas si décoché)
        if (lightUpwards)
        {
            transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        }

        // 3. Appliquer la couleur et les dimensions du faisceau
        lightComp.color = beaconColor;
        lightComp.range = range;
        lightComp.spotAngle = spotAngle;
        lightComp.innerSpotAngle = spotAngle * 0.5f; // Adoucit les bords
        lightComp.intensity = intensity;

        // 4. Ombres douces pour ne pas être trop agressif
        lightComp.shadows = LightShadows.Soft;
    }
}