using UnityEngine;

public class FloodingReplacer : MonoBehaviour
{
    [Header("Environnement Inondé")]
    public GameObject withFlooding; // L'objet "withFlooding"

    [Header("Environnement Aménagé (Sans inondation)")]
    public GameObject withoutFlooding; // L'objet "withoutFlooding"

    public bool isReplaced = false;

    void Start()
    {
        // Si le quiz n'a pas encore été résolu via la sauvegarde, on affiche la version inondée par défaut
        if (!isReplaced)
        {
            if (withFlooding != null) withFlooding.SetActive(true);
            if (withoutFlooding != null) withoutFlooding.SetActive(false);
        }
    }

    /// <summary>
    /// Bascule de la rue inondée vers l'aménagement perméable
    /// </summary>
    public void ReplaceFloodingEnvironment()
    {
        if (withFlooding == null || withoutFlooding == null) return;

        isReplaced = true;

        withFlooding.SetActive(false);
        withoutFlooding.SetActive(true);

        Debug.Log("Zone inondée remplacée avec succès par l'aménagement biomimétique !");
    }
}