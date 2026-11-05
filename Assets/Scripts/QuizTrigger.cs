using UnityEngine;

[RequireComponent(typeof(Collider))]
public class QuizTrigger : MonoBehaviour
{
    [Header("ID de la question")]
    [Tooltip("Laisse vide si l'objet s'appelle exactement comme l'ID (ex: TrainNoise)")]
    public string customQuestionID = "";

    private void OnMouseDown()
    {
        string idToUse = string.IsNullOrEmpty(customQuestionID) ? gameObject.name : customQuestionID;

        if (QuizManager.Instance != null)
        {
            QuizManager.Instance.TriggerQuizByName(idToUse, this);
        }
    }

    public void OnQuestionAnsweredCorrectly()
    {
        gameObject.SetActive(false); // Le point d'exclamation disparaît après la bonne réponse
    }
}