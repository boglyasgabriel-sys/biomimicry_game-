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
    public Sprite questionIllustration;
}

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance { get; private set; }

    [Header("Elements UI du Canvas")]
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionTexts;
    public Image questionImage;

    [Header("Panneau Quizz")]
    public GameObject quizWindow;

    [Header("Gestionnaire de remplacement du Train")]
    public TrainReplacer trainReplacer; 

    
    [Header("Gestionnaire de remplacement Flooding")]
    public FloodingReplacer floodingReplacer;

    [Header("Banque de Questions")]
    public List<QuestionData> questionsList = new List<QuestionData>();

    private Dictionary<string, QuestionData> questionsDictionary = new Dictionary<string, QuestionData>();
    private int correctAnswerIndex;
    private QuizTrigger currentTrigger;
    private string currentQuestionID;

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

    IEnumerator Start()
    {
        if (quizWindow != null)
        {
            quizWindow.SetActive(false);
        }

        // Attend la fin de la frame pour s'assurer que tous les scripts de la scène ont exécuté leur Awake/Start
        yield return null;

        CheckCompletedTriggersInScene();
    }

    /// <summary>
    /// Parcourt la scène au lancement et applique l'état sauvegardé dans le GameManager
    /// </summary>
    void CheckCompletedTriggersInScene()
{
    QuizTrigger[] triggers = FindObjectsByType<QuizTrigger>(FindObjectsSortMode.None);
    foreach (QuizTrigger trigger in triggers)
    {
        string id = string.IsNullOrEmpty(trigger.customQuestionID) ? trigger.gameObject.name : trigger.customQuestionID;

        if (GameManager.Instance != null && GameManager.Instance.IsQuizCompleted(id))
        {
            if (id == "TrainNoise" && trainReplacer != null)
            {
                trainReplacer.ReplaceTrainModel();
            }
            else if (id == "Flooding" && floodingReplacer != null)
            {
                floodingReplacer.ReplaceFloodingEnvironment();
            }

            trigger.gameObject.SetActive(false);
        }
    }
}
    void InitializeQuestions()
    {
        AddQuestion("TrainNoise", 
            "How can we reduce train noise during winter?", 
            new string[] { "Kingfisher beak shape", "Wall moss with lichens", "Infrasound like elephants" }, 
            0);

        AddQuestion("UrbanHeat", 
            "How can urban heat issues be solved in summer?", 
            new string[] { "Shark skin micro-grooves", "Termite mound construction", "Sand fox burrows" }, 
            1);

        AddQuestion("Flooding", 
            "What biomimetic solution can prevent winter flooding?", 
            new string[] { "Mole gallery", "Lotus leaf / reptile skin", "Elevated roads" }, 
            1);

        AddQuestion("Glare", 
            "How can we prevent glare and reflections in summer?", 
            new string[] { "Moth eye structure", "Chameleon skin", "Fish scales" }, 
            0);

        AddQuestion("TrafficJams", 
            "How can traffic congestion be optimized in winter?", 
            new string[] { "Honeybee dance", "Beaver damming", "Slime mold / ant algorithm" }, 
            2);

        AddQuestion("HeatLoss", 
            "What animal mechanism helps limit heat loss in winter?", 
            new string[] { "Peacock feathers", "Polar bear fur", "Hedgehog spines" }, 
            1);

        AddQuestion("WindowStrikes", 
            "How can window facades prevent bird collisions in summer?", 
            new string[] { "Spider web pattern", "Raptor silhouettes", "Peacock tail pattern" }, 
            0);

        AddQuestion("Ventilation", 
            "How can we improve airflow in narrow streets during summer?", 
            new string[] { "Porous marine sponge", "Shell spiral model", "Beehive structure" }, 
            1);

        AddQuestion("Wildfires", 
            "How can recurring wildfires be prevented in summer?", 
            new string[] { "Badger underbrush clearing", "Meerkat group vigilance", "Insect-inspired detectors" }, 
            2);

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

    public void TriggerQuizByName(string questionID, QuizTrigger trigger)
    {
        if (questionsDictionary.TryGetValue(questionID, out QuestionData data))
        {
            currentTrigger = trigger;
            currentQuestionID = questionID;
            correctAnswerIndex = data.correctAnswerIndex;

            questionText.text = data.questionText;

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

            quizWindow.SetActive(true);
        }
        else
        {
            Debug.LogError($"Aucune question trouvée avec l'ID : {questionID}");
        }
    }

// 3. Dans OnOptionSelected(int selectedIndex) :
void OnOptionSelected(int selectedIndex)
{
    if (selectedIndex == correctAnswerIndex)
    {
        Debug.Log("Correct answer!");
        quizWindow.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.MarkQuizCompleted(currentQuestionID);
        }

        if (currentQuestionID == "TrainNoise" && trainReplacer != null)
        {
            trainReplacer.ReplaceTrainModel();
        }
        else if (currentQuestionID == "Flooding" && floodingReplacer != null)
        {
            floodingReplacer.ReplaceFloodingEnvironment();
        }

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