using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup introCanvasGroup; // Pour gérer la transparence (fade)
    public Image introImage;             // L'image d'intro
    public Sprite introSprite;           // Ton image (intro.jpg)

    [Header("Timing Settings")]
    [Tooltip("Durée d'affichage à 100% d'opacité (en secondes)")]
    public float displayDuration = 1.0f; 

    [Tooltip("Durée de la transition en fondu (en secondes)")]
    public float fadeDuration = 0.5f;   

    void Awake()
    {
        // 1. S'assure que le Canvas est actif dès le lancement
        gameObject.SetActive(true);

        // 2. Force l'image à prendre TOUT l'écran par code
        if (introImage != null)
        {
            if (introSprite != null) introImage.sprite = introSprite;

            RectTransform rt = introImage.rectTransform;
            rt.anchorMin = Vector2.zero;       // (0,0)
            rt.anchorMax = Vector2.one;        // (1,1)
            rt.offsetMin = Vector2.zero;       // Pos Left & Bottom = 0
            rt.offsetMax = Vector2.zero;       // Pos Right & Top = 0
        }

        // 3. Initialise l'opacité au maximum
        if (introCanvasGroup != null)
        {
            introCanvasGroup.alpha = 1f;
        }
    }

    void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        // Temps d'affichage fixe
        yield return new WaitForSeconds(displayDuration);

        // Fondu de sortie (Fade Away)
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (introCanvasGroup != null)
            {
                introCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            }
            yield return null;
        }

        // Désactive complètement l'intro à la fin
        gameObject.SetActive(false);
    }
}