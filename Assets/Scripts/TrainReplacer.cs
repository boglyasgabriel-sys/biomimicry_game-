using UnityEngine;

public class TrainReplacer : MonoBehaviour
{
    [Header("Train d'origine (Locomotive)")]
    public GameObject oldTrain; // L'objet "train0"

    [Header("Nouveau train (Bec du Martin Pêcheur)")]
    public GameObject newTrain; // L'objet "replace_train"

    public bool isReplaced = false;

    void Start()
    {
        // Si le train n'a pas encore été remplacé par la sauvegarde, on masque le nouveau modèle
        if (!isReplaced && newTrain != null)
        {
            newTrain.SetActive(false);
        }
    }

    /// <summary>
    /// Remplace la locomotive actuelle par le nouveau modèle à la position exacte
    /// </summary>
    public void ReplaceTrainModel()
    {
        if (oldTrain == null || newTrain == null) return;

        isReplaced = true;

        // 1. Activer le nouveau train avant de copier les transform/waypoints
        newTrain.SetActive(true);

        // 2. Copie exacte de la position et de la rotation actuelles de la locomotive
        newTrain.transform.position = oldTrain.transform.position;
        newTrain.transform.rotation = oldTrain.transform.rotation;

        // 3. Transférer la progression des waypoints et la vitesse
        TrainFollowPath oldScript = oldTrain.GetComponent<TrainFollowPath>();
        TrainFollowPath newScript = newTrain.GetComponent<TrainFollowPath>();

        if (oldScript != null && newScript != null)
        {
            newScript.waypoints = oldScript.waypoints;
            newScript.speed = oldScript.speed;
            newScript.rotationSpeed = oldScript.rotationSpeed;

            // Transfère l'index du waypoint actuel pour que la nouvelle locomotive continue sans saut
            newScript.SetCurrentWaypointIndex(oldScript.GetCurrentWaypointIndex());
        }

        // 4. Masquer l'ancien train
        oldTrain.SetActive(false);

        Debug.Log("Train remplacé avec succès par le modèle profilé !");
    }
}