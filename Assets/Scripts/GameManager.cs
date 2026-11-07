using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Ensemble des ID de quiz réussis (ex: "TrainNoise", "UrbanHeat", etc.)
    private HashSet<string> completedQuizzes = new HashSet<string>();

    void Awake()
    {
        // Pattern Singleton persistant entre les scènes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Enregistre un quiz comme complété
    /// </summary>
    public void MarkQuizCompleted(string quizID)
    {
        if (!completedQuizzes.Contains(quizID))
        {
            completedQuizzes.Add(quizID);
        }
    }

    /// <summary>
    /// Vérifie si un quiz a déjà été réussi
    /// </summary>
    public bool IsQuizCompleted(string quizID)
    {
        return completedQuizzes.Contains(quizID);
    }
}