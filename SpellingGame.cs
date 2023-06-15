using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SpellingGame : MonoBehaviour
{
    public TextMeshProUGUI wordText;
    public Button button1;
    public Button button2;
    public Button button3;

   private List<string> wordList = new List<string> { "миш_", "лам_", "лег_", "ябъл_", "вра_" };
private Dictionary<string, string> syllableDict = new Dictionary<string, string> { {"миш_", "ка"}, {"лам_", "па"}, {"лег_", "ло"}, {"ябъл_", "ка"}, {"вра_", "та"} };

private string missingSyllable;
private string correctAnswer;

// Start is called before the first frame update
void Start()
{
    ChooseRandomWord();
    AssignButtons(button1, button2, button3);
}

void ChooseRandomWord()
{
    string randomWord = wordList[Random.Range(0, wordList.Count)];
    missingSyllable = syllableDict[randomWord];
    correctAnswer = randomWord.Replace("_", missingSyllable);

    wordText.text = randomWord.Replace("_", "...");
}

void AssignButtons(Button b1, Button b2, Button b3)
{
    List<Button> buttonList = new List<Button> { b1, b2, b3 };
    int correctButtonIndex = Random.Range(0, buttonList.Count);

    foreach (Button button in buttonList)
    {
        button.onClick.RemoveAllListeners();
    }

    for (int i = 0; i < buttonList.Count; i++)
    {
        string randomSyllable = GetRandomSyllable();
        if (i == correctButtonIndex)
        {
            buttonList[i].GetComponentInChildren<TextMeshProUGUI>().text = missingSyllable;
            buttonList[i].onClick.AddListener(() => CheckAnswer(true));
        }
        else
        {
            buttonList[i].GetComponentInChildren<TextMeshProUGUI>().text = randomSyllable;
            buttonList[i].onClick.AddListener(() => CheckAnswer(false));
        }
    }
}

string GetRandomSyllable()
{
    List<string> syllablesList = new List<string> { "миш", "ка", "пи", "вра", "та", "лам", "па", "бъл", "та", "вра", "са", "ма", "ка" };
    string randomSyllable = syllablesList[Random.Range(0, syllablesList.Count)];

    while (randomSyllable == missingSyllable)
    {
        randomSyllable = syllablesList[Random.Range(0, syllablesList.Count)];
    }

    return randomSyllable;
}

void CheckAnswer(bool isCorrectButton)
{
    if (isCorrectButton)
    {
        Debug.Log("Correct Answer! Restarting game...");
        ChooseRandomWord();
        AssignButtons(button1, button2, button3);
    }
    else
    {
        Debug.Log("Wrong Answer! Try again...");
    }
}

private class ButtonState
{
    public Button button;
    public bool isEnabled;

    public ButtonState(Button button, bool isEnabled)
    {
        this.button = button;
        this.isEnabled = isEnabled;
    }
}
}



            // new WordData { word = "миш__", syllableOptions = new string[] { "ка", "па", "ма" }, correctSyllableIndex = 0 },
            // new WordData { word = "лам__", syllableOptions = new string[] { "па", "ка", "са" }, correctSyllableIndex = 0 },
            // new WordData { word = "лег__", syllableOptions = new string[] { "ло", "лу", "зо" }, correctSyllableIndex = 0 },
            // new WordData { word = "лам__", syllableOptions = new string[] { "па", "ка", "са" }, correctSyllableIndex = 0 },
            // new WordData { word = "лам__", syllableOptions = new string[] { "па", "ка", "са" }, correctSyllableIndex = 0 },
            // new WordData { word = "лам__", syllableOptions = new string[] { "па", "ка", "са" }, correctSyllableIndex = 0 },