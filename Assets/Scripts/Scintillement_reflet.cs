using UnityEngine;
using UnityEngine.Rendering;

public class Scintillement_reflet : MonoBehaviour
{
    public float tailleMin = 0.7f;
    public float tailleMax = 1.3f;
    public float vitesse = 1.5f;

    LensFlareComponentSRP flare;

    void Start()
    {
        flare = GetComponent<LensFlareComponentSRP>();
    }

    void Update()
    {
        float t = Mathf.PerlinNoise(Time.time * vitesse, 0f);
        flare.scale = Mathf.Lerp(tailleMin, tailleMax, t);
    }
}
