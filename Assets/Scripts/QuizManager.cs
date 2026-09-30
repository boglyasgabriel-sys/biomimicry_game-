using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("Elements UI")]
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionTexts;

    [Header("Panneau Quizz")]
    public GameObject quizWindow;

    private int correctAnswerIndex;

    void Start()
    {
        // Exemple de test en anglais sur le biomimétisme
        SetupQuestion(
            "Which animal should we imitate to solve urban heat issues in the city?",
            new string[] { "Termite", "Camel", "Toucan" },
            0 // Exemple : 0 = Toucan (index 0, 1, 2)
        );
    }

    public void SetupQuestion(string question, string[] options, int correctIndex)
    {
        questionText.text = question;
        correctAnswerIndex = correctIndex;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionTexts[i].text = options[i];
            
            int index = i; // Copie locale nécessaire pour le listener
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    void OnOptionSelected(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex)
        {
            Debug.Log("Correct answer!");
            quizWindow.SetActive(false); // On ferme la fenêtre
        }
        else
        {
            Debug.Log("Wrong answer, try again!");
        }
    }
}