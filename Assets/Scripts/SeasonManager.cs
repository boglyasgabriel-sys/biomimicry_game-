using UnityEngine;
using UnityEngine.SceneManagement;

public class SeasonManager : MonoBehaviour
{
    // Charge la scène d'Hiver
    public void LoadWinterScene()
    {
        SceneManager.LoadScene("City_Winter");
    }

    // Charge la scène d'Été
    public void LoadSummerScene()
    {
        SceneManager.LoadScene("City_Summer");
    }
}