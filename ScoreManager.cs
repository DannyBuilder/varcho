using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text bestScoreEasy;
    public Text bestScoreMedium;
    public Text bestScoreHard;
    public Text[] lastFiveScoresEasy;
    public Text[] lastFiveScoresMedium;
    public Text[] lastFiveScoresHard;

    void Start()
    {
        LoadScores("Easy", bestScoreEasy, lastFiveScoresEasy);
        LoadScores("Medium", bestScoreMedium, lastFiveScoresMedium);
        LoadScores("Hard", bestScoreHard, lastFiveScoresHard);
    }

    void LoadScores(string difficulty, Text bestScoreText, Text[] lastFiveScoresTexts)
    {
        float bestScore = PlayerPrefs.GetFloat($"BestScore_{difficulty}", float.MaxValue);
        bestScoreText.text = bestScore == float.MaxValue ? "Рекорд: N/A" : $"Рекорд: {bestScore:0.00}";

        List<float> lastFiveScores = new List<float>();
        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey($"LastScore{i}_{difficulty}"))
            {
                lastFiveScores.Add(PlayerPrefs.GetFloat($"LastScore{i}_{difficulty}"));
            }
        }

        for (int i = 0; i < lastFiveScores.Count; i++)
        {
            int gameNumber = lastFiveScores.Count - i;
            lastFiveScoresTexts[i].text = $"Игра {gameNumber}: {lastFiveScores[i]:0.00}";
        }
    }

    public void ResetScores()
    {
        PlayerPrefs.DeleteKey("BestScore_Easy");
        PlayerPrefs.DeleteKey("BestScore_Medium");
        PlayerPrefs.DeleteKey("BestScore_Hard");

        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.DeleteKey($"LastScore{i}_Easy");
            PlayerPrefs.DeleteKey($"LastScore{i}_Medium");
            PlayerPrefs.DeleteKey($"LastScore{i}_Hard");
        }

        PlayerPrefs.Save();

        // Reload scores 
        LoadScores("Easy", bestScoreEasy, lastFiveScoresEasy);
        LoadScores("Medium", bestScoreMedium, lastFiveScoresMedium);
        LoadScores("Hard", bestScoreHard, lastFiveScoresHard);
    }
}
