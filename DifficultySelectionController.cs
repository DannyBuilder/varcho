using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelectionController : MonoBehaviour
{
    public void StartEasy()
    {
        PlayerPrefs.SetString("SelectedDifficulty", "Easy");
        SceneManager.LoadScene("Spelling Game");
    }

    public void StartMedium()
    {
        PlayerPrefs.SetString("SelectedDifficulty", "Medium");
        SceneManager.LoadScene("Spelling Game");
    }

    public void StartHard()
    {
        PlayerPrefs.SetString("SelectedDifficulty", "Hard");
        SceneManager.LoadScene("Spelling Game");
    }
}
