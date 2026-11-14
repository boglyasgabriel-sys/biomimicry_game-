using UnityEngine;

public class WindTurbineRotation : MonoBehaviour
{
    public float vitesse = 60f; // degrés par seconde

    void Update()
    {
        transform.Rotate(0f, 0f, vitesse * Time.deltaTime);
    }
}
