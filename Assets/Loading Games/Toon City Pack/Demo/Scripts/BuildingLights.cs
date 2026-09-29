using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLights : MonoBehaviour {
    public int windowMaterialIndex;
    public Color lightColor;
    public bool areLightsOn;
    private Color defaultColor;
    private MeshRenderer mr;

    private void Start() {
        // Le code ne modifie plus les matériaux au lancement pour éviter le rose URP
    }

    public void SetLights(bool isOn) {
        // Fonction neutralisée : conserve la structure sans altérer les shaders URP
        areLightsOn = isOn;
    }
}