using UnityEngine;

public class Chaleur_batiment : MonoBehaviour
{
    public Color couleur = new Color(1f, 0.15f, 0.05f);
    public float intensiteMin = 0.2f;
    public float intensiteMax = 0.8f;
    public float vitesse = 1.5f;

    Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        float t = Mathf.PerlinNoise(Time.time * vitesse, 0f);
        float intensite = Mathf.Lerp(intensiteMin, intensiteMax, t);
        mat.SetColor("_EmissionColor", couleur * intensite);
    }
}
