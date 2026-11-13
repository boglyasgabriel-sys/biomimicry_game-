using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

// Cette classe n'affiche plus que les images dans l'Inspector
[System.Serializable]
public class QuestionVisuals
{
    public string id; // ex: "TrainNoise", "Flooding"
    
    [Header("Images Dynamiques")]
    public Sprite contextImage;
    public Sprite quizBackgroundImage;
    public Sprite solutionImage;
}

// Cette classe sert uniquement au code interne pour stocker les textes en dur
public class QuestionTextData
{
    public string questionText;
    public string[] options;
    public int correctAnswerIndex;
}

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance { get; private set; }

    [Header("1. Fenêtre CONTEXTE")]
    public GameObject contextWindow;
    public Image contextImageDisplay;
    public Button continueButton;

    [Header("2. Fenêtre QUIZ")]
    public GameObject quizWindow;
    public Image quizBackgroundImageDisplay;
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionTexts;

    [Header("3. Fenêtre SOLUTION")]
    public GameObject solutionWindow;
    public Image solutionImageDisplay;
    public Button closeSolutionButton;

    [Header("Gestionnaires de remplacement 3D")]
    public TrainReplacer trainReplacer; 
    public FloodingReplacer floodingReplacer;

    [Header("Images des Questions (Remplir ici)")]
    // La liste de l'Inspector ne demandera plus que l'ID et les 3 images
    public List<QuestionVisuals> questionsVisualsList = new List<QuestionVisuals>();

    // Le dictionnaire invisible qui stocke les textes en dur
    private Dictionary<string, QuestionTextData> questionsTextDictionary = new Dictionary<string, QuestionTextData>();
    
    private int correctAnswerIndex;
    private QuizTrigger currentTrigger;
    private string currentQuestionID;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // On charge les textes en dur au démarrage
        InitializeQuestions();
    }

    IEnumerator Start()
    {
        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ProceedToQuiz);
        }

        if (closeSolutionButton != null)
        {
            closeSolutionButton.onClick.RemoveAllListeners();
            closeSolutionButton.onClick.AddListener(CloseSolution);
        }

        if (contextWindow != null) contextWindow.SetActive(false);
        if (quizWindow != null) quizWindow.SetActive(false);
        if (solutionWindow != null) solutionWindow.SetActive(false);

        yield return null;
        CheckCompletedTriggersInScene();
    }

    void CheckCompletedTriggersInScene()
    {
        QuizTrigger[] triggers = FindObjectsByType<QuizTrigger>(FindObjectsSortMode.None);
        foreach (QuizTrigger trigger in triggers)
        {
            string id = string.IsNullOrEmpty(trigger.customQuestionID) ? trigger.gameObject.name : trigger.customQuestionID;

            if (GameManager.Instance != null && GameManager.Instance.IsQuizCompleted(id))
            {
                if (id == "TrainNoise" && trainReplacer != null) trainReplacer.ReplaceTrainModel();
                else if (id == "Flooding" && floodingReplacer != null) floodingReplacer.ReplaceFloodingEnvironment();

                trigger.gameObject.SetActive(false);
            }
        }
    }

    // Les textes et réponses sont de nouveau intouchables et stockés ici en dur
    void InitializeQuestions()
    {
        AddQuestionText("TrainNoise", "How can we reduce train noise during winter?", new string[] { "Kingfisher beak shape", "Wall moss with lichens", "Infrasound like elephants" }, 0);
        AddQuestionText("UrbanHeat", "How can urban heat issues be solved in summer?", new string[] { "Shark skin micro-grooves", "Termite mound construction", "Sand fox burrows" }, 1);
        AddQuestionText("Flooding", "What biomimetic solution can prevent winter flooding?", new string[] { "Mole gallery", "Lotus leaf / reptile skin", "Elevated roads" }, 1);
        AddQuestionText("Glare", "How can we prevent glare and reflections in summer?", new string[] { "Moth eye structure", "Chameleon skin", "Fish scales" }, 0);
        AddQuestionText("TrafficJams", "How can traffic congestion be optimized in winter?", new string[] { "Honeybee dance", "Beaver damming", "Slime mold / ant algorithm" }, 2);
        AddQuestionText("HeatLoss", "What animal mechanism helps limit heat loss in winter?", new string[] { "Peacock feathers", "Polar bear fur", "Hedgehog spines" }, 1);
        AddQuestionText("WindowStrikes", "How can window facades prevent bird collisions in summer?", new string[] { "Spider web pattern", "Raptor silhouettes", "Peacock tail pattern" }, 0);
        AddQuestionText("Ventilation", "How can we improve airflow in narrow streets during summer?", new string[] { "Porous marine sponge", "Shell spiral model", "Beehive structure" }, 1);
        AddQuestionText("Wildfires", "How can recurring wildfires be prevented in summer?", new string[] { "Badger underbrush clearing", "Meerkat group vigilance", "Insect-inspired detectors" }, 2);
        AddQuestionText("Adhesive", "What biological model inspires strong winter adhesives?", new string[] { "Gecko feet", "Sequoia sap", "Leech suckers" }, 0);
    }

    void AddQuestionText(string id, string text, string[] options, int correctIndex)
    {
        QuestionTextData q = new QuestionTextData { questionText = text, options = options, correctAnswerIndex = correctIndex };
        if (!questionsTextDictionary.ContainsKey(id)) questionsTextDictionary.Add(id, q);
    }

    public void TriggerQuizByName(string questionID, QuizTrigger trigger)
    {
        // 1. On récupère les textes depuis le dictionnaire codé en dur
        if (questionsTextDictionary.TryGetValue(questionID, out QuestionTextData textData))
        {
            currentTrigger = trigger;
            currentQuestionID = questionID;
            correctAnswerIndex = textData.correctAnswerIndex;

            questionText.text = textData.questionText;

            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < textData.options.Length)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    optionTexts[i].text = textData.options[i];
                    int index = i;
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }

            // 2. On récupère les images depuis l'Inspector
            QuestionVisuals visuals = questionsVisualsList.Find(v => v.id == questionID);
            if (visuals != null)
            {
                if (contextImageDisplay != null) contextImageDisplay.sprite = visuals.contextImage;
                if (quizBackgroundImageDisplay != null) quizBackgroundImageDisplay.sprite = visuals.quizBackgroundImage;
                if (solutionImageDisplay != null) solutionImageDisplay.sprite = visuals.solutionImage;
            }
            else
            {
                Debug.LogWarning($"Aucune image configurée dans l'Inspector pour l'ID : {questionID}");
            }

            // On lance la séquence
            contextWindow.SetActive(true);
            quizWindow.SetActive(false);
            solutionWindow.SetActive(false);
        }
        else
        {
            Debug.LogError($"Aucun texte trouvé pour l'ID : {questionID} dans le code en dur !");
        }
    }

    public void ProceedToQuiz()
    {
        contextWindow.SetActive(false);
        quizWindow.SetActive(true);
    }

    void OnOptionSelected(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex)
        {
            quizWindow.SetActive(false);
            solutionWindow.SetActive(true);

            if (GameManager.Instance != null) GameManager.Instance.MarkQuizCompleted(currentQuestionID);

            if (currentQuestionID == "TrainNoise" && trainReplacer != null) trainReplacer.ReplaceTrainModel();
            else if (currentQuestionID == "Flooding" && floodingReplacer != null) floodingReplacer.ReplaceFloodingEnvironment();

            if (currentTrigger != null) currentTrigger.OnQuestionAnsweredCorrectly();
        }
        else
        {
            Debug.Log("Wrong answer, try again!");
        }
    }

    public void CloseSolution()
    {
        solutionWindow.SetActive(false);
    }
}