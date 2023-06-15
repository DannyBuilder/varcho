//Eric Adam

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DyslexiaGameController : MonoBehaviour
{
    public Text wordDisplay;
    public Text resultDisplay;
    public Text feedbackDisplay;
    public Button[] syllableButtons;

    private List<WordData> wordsData;
    private List<WordData> remainingWords;
    private List<WordData> easyWords;
    private List<WordData> mediumWords;
    private List<WordData> hardWords;
    private WordData currentWordData;
    private bool firstGuess;
    private float startTime;
    private string currentDifficulty;

    [System.Serializable]
    public class WordData
    {
        public string word;
        public string[] syllableOptions;
        public int correctSyllableIndex;
    }

   void Start()
    {

        // Define words 
        easyWords = new List<WordData>
        {
            // Add words here
            new WordData { word = "зи__", syllableOptions = new string[] { "ва", "ра", "ма" }, correctSyllableIndex = 2 },
            new WordData { word = "ди__", syllableOptions = new string[] { "ня", "за", "ка" }, correctSyllableIndex = 0 },
            new WordData { word = "дъ__", syllableOptions = new string[] { "ло", "ро", "жд" }, correctSyllableIndex = 2 },
            new WordData { word = "ду__", syllableOptions = new string[] { "га", "ма", "да" }, correctSyllableIndex = 1 },
            new WordData { word = "те__", syllableOptions = new string[] { "ма", "га", "ра" }, correctSyllableIndex = 0 },
            new WordData { word = "де__", syllableOptions = new string[] { "ха", "на", "те" }, correctSyllableIndex = 2},
            new WordData { word = "ри__", syllableOptions = new string[] { "ва", "ба", "те" }, correctSyllableIndex = 1},
            new WordData { word = "во__", syllableOptions = new string[] { "ма", "да", "ра" }, correctSyllableIndex = 1},


        };

        // Define words
        mediumWords = new List<WordData>
        {
            // Add words here
            new WordData { word = "ли__", syllableOptions = new string[] { "дон", "мон", "сон" }, correctSyllableIndex = 1 },
            new WordData { word = "чо__", syllableOptions = new string[] { "зан", "пан", "рап" }, correctSyllableIndex = 2},
            new WordData { word = "кар__", syllableOptions = new string[] { "вон", "тон", "фон" }, correctSyllableIndex = 1},
            new WordData { word = "лам__", syllableOptions = new string[] { "па", "ра", "да" }, correctSyllableIndex = 0},
        };

        // Define words
        hardWords = new List<WordData>
        {
            // Add words here
            new WordData { word = "цве__", syllableOptions = new string[] { "те", "се", "ре" }, correctSyllableIndex = 0 },
            new WordData { word = "алар__", syllableOptions = new string[] { "за", "ма", "да" }, correctSyllableIndex = 1 },
            new WordData { word = "ли__", syllableOptions = new string[] { "сто", "гов", "нов" }, correctSyllableIndex = 0 },
            new WordData { word = "водо__", syllableOptions = new string[] { "рам", "пад", "дав" }, correctSyllableIndex = 1 },
            new WordData { word = "лап__", syllableOptions = new string[] { "топ", "ром", "сто" }, correctSyllableIndex = 0 },
         
        };

        
            currentDifficulty = PlayerPrefs.GetString("SelectedDifficulty", "Easy");

        switch (currentDifficulty)
    {
        case "Easy":
            StartGame(0);
            break;
        case "Medium":
            StartGame(1);
            break;
        case "Hard":
            StartGame(2);
            break;
        default:
            StartGame(0);
            break;
    }

    }


    public void StartGame(int difficulty)
    {
        switch (difficulty)
        {
            case 0: // Easy
                wordsData = new List<WordData>(easyWords);
                currentDifficulty = "Easy";
                break;
            case 1: // Medium
                wordsData = new List<WordData>(mediumWords);
                currentDifficulty = "Medium";
                break;
            case 2: // Hard
                wordsData = new List<WordData>(hardWords);
                currentDifficulty = "Hard";
                break;
        }

        firstGuess = true;
        remainingWords = new List<WordData>(wordsData);
        resultDisplay.enabled = false; // Disable resultDisplay
        feedbackDisplay.gameObject.SetActive(false);
        DisplayRandomWord();
    }

    void DisplayRandomWord()
    {
        if (remainingWords.Count == 0)
        {
            FinishGame();
            return;
        }

        int randomIndex = Random.Range(0, remainingWords.Count);
        currentWordData = remainingWords[randomIndex];
        remainingWords.RemoveAt(randomIndex);
        wordDisplay.text = currentWordData.word;

        List<int> indexList = new List<int> { 0, 1, 2 };
        for (int i = 0; i < syllableButtons.Length; i++)
        {
            int randomButtonIndex = indexList[Random.Range(0, indexList.Count)];
            indexList.Remove(randomButtonIndex);
            syllableButtons[i].GetComponentInChildren<Text>().text = currentWordData.syllableOptions[randomButtonIndex];
            int buttonIndex = randomButtonIndex;

            syllableButtons[i].onClick.RemoveAllListeners();
            syllableButtons[i].onClick.AddListener(() => CheckAnswer(buttonIndex));
            syllableButtons[i].image.color = Color.white;
        }
    }

    void CheckAnswer(int buttonIndex)
    {
        if (firstGuess)
        {
            startTime = Time.time;
            firstGuess = false;
        }

        if (buttonIndex == currentWordData.correctSyllableIndex)
        {
            wordDisplay.text = currentWordData.word.Replace("__", currentWordData.syllableOptions[buttonIndex]);

            
            int indexOfCorrectButton = System.Array.FindIndex(syllableButtons, button => button.GetComponentInChildren<Text>().text == currentWordData.syllableOptions[buttonIndex]);

            // Change to green
            StartCoroutine(CorrectAnswer(indexOfCorrectButton));

            // Delay
            Invoke(nameof(DisplayRandomWord), 2f);
        }
        else
        {
           
            int indexOfIncorrectButton = System.Array.FindIndex(syllableButtons, button => button.GetComponentInChildren<Text>().text == currentWordData.syllableOptions[buttonIndex]);

            // Display message / change color
            StartCoroutine(IncorrectAnswer(indexOfIncorrectButton));
        }
    }

    System.Collections.IEnumerator CorrectAnswer(int indexOfCorrectButton)
    {
        Button correctButton = syllableButtons[indexOfCorrectButton];
        Color originalColor = correctButton.image.color;
        correctButton.image.color = Color.green;

        yield return new WaitForSeconds(1f);

        correctButton.image.color = originalColor;
    }

    System.Collections.IEnumerator IncorrectAnswer(int indexOfIncorrectButton)
    {
        feedbackDisplay.text = "Опитай пак!";
        feedbackDisplay.gameObject.SetActive(true);

        Button incorrectButton = syllableButtons[indexOfIncorrectButton];
        Color originalColor = incorrectButton.image.color;
        incorrectButton.image.color = Color.red;

        yield return new WaitForSeconds(1f);

        feedbackDisplay.gameObject.SetActive(false);
        incorrectButton.image.color = originalColor;
    }

    void FinishGame()
    {
        float totalTime = Time.time - startTime;
        int minutes = Mathf.FloorToInt(totalTime / 60);
        int seconds = Mathf.FloorToInt(totalTime % 60);

        resultDisplay.text = $"Браво! Твоето време е {minutes:00}:{seconds:00}";
        resultDisplay.enabled = true;

        wordDisplay.gameObject.SetActive(false);

        foreach (Button button in syllableButtons)
        {
            button.gameObject.SetActive(false);
        }

        SaveScore(totalTime, currentDifficulty);
    }

    void SaveScore(float totalTime, string difficulty)
{
    // Save scores
    float bestScore = PlayerPrefs.GetFloat($"BestScore_{difficulty}", float.MaxValue);
    if (totalTime < bestScore)
    {
        PlayerPrefs.SetFloat($"BestScore_{difficulty}", totalTime);
    }

   
    List<float> lastFiveScores = new List<float>();
    for (int i = 0; i < 5; i++)
    {
        if (PlayerPrefs.HasKey($"LastScore{i}_{difficulty}"))
        {
            lastFiveScores.Add(PlayerPrefs.GetFloat($"LastScore{i}_{difficulty}"));
        }
    }

    lastFiveScores.Add(totalTime);
    
    if (lastFiveScores.Count > 5)
    {
        lastFiveScores.Sort();
        lastFiveScores.RemoveAt(0);
    }

    for (int i = 0; i < lastFiveScores.Count; i++)
    {
        PlayerPrefs.SetFloat($"LastScore{i}_{difficulty}", lastFiveScores[i]);
    }

    PlayerPrefs.Save();
}

}