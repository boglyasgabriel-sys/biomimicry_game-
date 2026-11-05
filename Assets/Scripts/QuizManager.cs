using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class QuestionData
{
    public string id; // Nom/Identifiant du point d'exclamation (ex: "TrainNoise")
    [TextArea(2, 5)]
    public string questionText;
    public string[] options;
    public int correctAnswerIndex;
    public Sprite questionIllustration; // Image spécifique au parchemin
}

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance { get; private set; }

    [Header("Elements UI du Canvas")]
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionTexts;
    public Image questionImage; // Cadre d'illustration sur le parchemin

    [Header("Panneau Quizz")]
    public GameObject quizWindow;

    [Header("Banque de Questions")]
    public List<QuestionData> questionsList = new List<QuestionData>();

    private Dictionary<string, QuestionData> questionsDictionary = new Dictionary<string, QuestionData>();
    private int correctAnswerIndex;
    private QuizTrigger currentTrigger;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeQuestions();
    }

    void Start()
    {
        // Masque le Canvas au lancement du jeu
        if (quizWindow != null)
        {
            quizWindow.SetActive(false);
        }
    }

    /// <summary>
    /// Remplit le dictionnaire avec toutes les questions configurées
    /// </summary>
    void InitializeQuestions()
    {
        // 1. Train Noise (hiver)
        AddQuestion("TrainNoise", 
            "How can we reduce train noise during winter?", 
            new string[] { "Kingfisher beak shape", "Wall moss with lichens", "Infrasound like elephants" }, 
            0);

        // 2. Urban Heat (été)
        AddQuestion("UrbanHeat", 
            "How can urban heat issues be solved in summer?", 
            new string[] { "Shark skin micro-grooves", "Termite mound construction", "Sand fox burrows" }, 
            1);

        // 3. Flooding (hiver)
        AddQuestion("Flooding", 
            "What biomimetic solution can prevent winter flooding?", 
            new string[] { "Mole gallery", "Lotus leaf / reptile skin", "Elevated roads" }, 
            1);

        // 4. Glare / Reflection (été)
        AddQuestion("Glare", 
            "How can we prevent glare and reflections in summer?", 
            new string[] { "Moth eye structure", "Chameleon skin", "Fish scales" }, 
            0);

        // 5. Traffic Jams (hiver)
        AddQuestion("TrafficJams", 
            "How can traffic congestion be optimized in winter?", 
            new string[] { "Honeybee dance", "Beaver damming", "Slime mold / ant algorithm" }, 
            2);

        // 6. Heat Loss (hiver)
        AddQuestion("HeatLoss", 
            "What animal mechanism helps limit heat loss in winter?", 
            new string[] { "Peacock feathers", "Polar bear fur", "Hedgehog spines" }, 
            1);

        // 7. Window Bird Strikes (été)
        AddQuestion("WindowStrikes", 
            "How can window facades prevent bird collisions in summer?", 
            new string[] { "Spider web pattern", "Raptor silhouettes", "Peacock tail pattern" }, 
            0);

        // 8. Narrow Streets Ventilation (été)
        AddQuestion("Ventilation", 
            "How can we improve airflow in narrow streets during summer?", 
            new string[] { "Porous marine sponge", "Shell spiral model", "Beehive structure" }, 
            1);

        // 9. Wildfires (été)
        AddQuestion("Wildfires", 
            "How can recurring wildfires be prevented in summer?", 
            new string[] { "Badger underbrush clearing", "Meerkat group vigilance", "Insect-inspired detectors" }, 
            2);

        // 10. Adhesive (hiver)
        AddQuestion("Adhesive", 
            "What biological model inspires strong winter adhesives?", 
            new string[] { "Gecko feet", "Sequoia sap", "Leech suckers" }, 
            0);
    }

    void AddQuestion(string id, string text, string[] options, int correctIndex)
    {
        QuestionData q = new QuestionData
        {
            id = id,
            questionText = text,
            options = options,
            correctAnswerIndex = correctIndex
        };

        if (!questionsDictionary.ContainsKey(id))
        {
            questionsDictionary.Add(id, q);
        }
    }

    /// <summary>
    /// Déclenche le quiz en fonction du nom de l'objet ou de l'ID passé
    /// </summary>
    public void TriggerQuizByName(string questionID, QuizTrigger trigger)
    {
        if (questionsDictionary.TryGetValue(questionID, out QuestionData data))
        {
            currentTrigger = trigger;
            correctAnswerIndex = data.correctAnswerIndex;

            // Update du texte de la question
            questionText.text = data.questionText;

            // Update de l'image (si configurée dans l'Inspector de la liste)
            QuestionData inspectorData = questionsList.Find(q => q.id == questionID);
            if (questionImage != null)
            {
                if (inspectorData != null && inspectorData.questionIllustration != null)
                {
                    questionImage.sprite = inspectorData.questionIllustration;
                    questionImage.gameObject.SetActive(true);
                }
                else
                {
                    questionImage.gameObject.SetActive(false);
                }
            }

            // Update des textes et boutons d'options
            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < data.options.Length)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    optionTexts[i].text = data.options[i];

                    int index = i;
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }

            // Affichage du Canvas
            quizWindow.SetActive(true);
        }
        else
        {
            Debug.LogError($"Aucune question trouvée avec l'ID : {questionID}");
        }
    }

    void OnOptionSelected(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex)
        {
            Debug.Log("Correct answer!");
            quizWindow.SetActive(false);

            if (currentTrigger != null)
            {
                currentTrigger.OnQuestionAnsweredCorrectly();
            }
        }
        else
        {
            Debug.Log("Wrong answer, try again!");
        }
    }
}