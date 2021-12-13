using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Настройки игры
    /// </summary>
    [SerializeField] private GameSettings gameSettings;

    /// <summary>
    /// Основной интерфейс
    /// </summary>
    [SerializeField] private GameObject mainUI;

    /// <summary>
    /// Интерфейс меню
    /// </summary>
    [SerializeField] private GameObject menuUI;

    /// <summary>
    /// Счет игрока (текст)
    /// </summary>
    [SerializeField] private TextMeshProUGUI scoresText;

    /// <summary>
    /// Счет игрока (текст в меню)
    /// </summary>
    [SerializeField] private TextMeshProUGUI menuScoresText;

    /// <summary>
    /// Количество попыток (текст)
    /// </summary>
    [SerializeField] private TextMeshProUGUI attemptCountText;

    /// <summary>
    /// Открываемая буква (префаб)
    /// </summary>
    [SerializeField] private LetterController letter;

    /// <summary>
    /// Счет игрока
    /// </summary>
    private int scores = 0;
    
    /// <summary>
    /// Текущее количество попыток
    /// </summary>
    private int currentAttemptCount;

    /// <summary>
    /// Угадываемое слово
    /// </summary>
    private string currentWord;

    /// <summary>
    /// Буквы угадываемого слова
    /// </summary>
    private char[] currentWordLetters;

    /// <summary>
    /// Количество открытых букв
    /// </summary>
    private int unlockedLetters = 0;

    /// <summary>
    /// Открываемые буквы
    /// </summary>
    private List<LetterController> letters = new List<LetterController>();

    /// <summary>
    /// Отключенные кнопки
    /// </summary>
    private List<Button> deactivatedButtons = new List<Button>();

    /// <summary>
    /// Конец игры
    /// </summary>
    private bool gameEnd = false;

    private void Start()
    {
        currentAttemptCount = gameSettings.attemptCount;
        UpdateUI();
        SetCurrentWord();       
    }

    private void Update()
    {
        if (!gameEnd && unlockedLetters == currentWordLetters.Length)
        {
            TextManager.wordList.Remove(currentWord);
            scores += currentAttemptCount;
            currentAttemptCount = gameSettings.attemptCount;
            CheckGameEnd();

            if (!gameEnd)
            {
                SetCurrentWord();

                foreach (Button bttn in deactivatedButtons)
                {
                    bttn.interactable = true;
                }

                deactivatedButtons = new List<Button>();
                unlockedLetters = 0;
            }
        }
    }

    /// <summary>
    /// Выбирает случайное слова из списка
    /// </summary>
    private void SetCurrentWord()
    {
        if (TextManager.wordList.Count > 0)
        {
            currentWord = TextManager.wordList[Random.Range(0, TextManager.wordList.Count)];
            currentWordLetters = currentWord.ToCharArray();
            SetLetters();
        }
    }

    /// <summary>
    /// Настраивает открываемые буквы
    /// </summary>
    private void SetLetters()
    {
        foreach (LetterController lttr in letters)
        {
            lttr.SetLetterLock(true);
            lttr.gameObject.SetActive(false);
        }

        for (int i = 0; i < currentWordLetters.Length; i++)
        {
            if (currentWordLetters.Length > letters.Count)
            {
                LetterController currentLetter = Instantiate(letter, mainUI.transform);
                letters.Add(currentLetter);
            }

            Vector3 letterPos = new Vector3(150 * Mathf.Floor(i - currentWordLetters.Length / 2) + 75 - (75 * (currentWordLetters.Length % 2)), 300);

            letters[i].transform.localPosition = letterPos;
            letters[i].SetLetter(currentWordLetters[i]);
            letters[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Проверяет наличие буквы в слове
    /// </summary>
    /// <param name="letter"></param>
    public void CheckLetterInWord(string letter)
    {
        bool letterFound = false;

        for (int i = 0; i < currentWordLetters.Length; i++)
        {
            if (letter == letters[i].letter.ToString())
            {
                letters[i].SetLetterLock(false);
                unlockedLetters++;
                letterFound = true;
            }
        }

        if (!letterFound)
        {
            currentAttemptCount--;
            CheckGameEnd();
        }
    }

    /// <summary>
    /// Обновляет интерфейс
    /// </summary>
    private void UpdateUI()
    {
        scoresText.text = "Scores: " + scores.ToString();     
        attemptCountText.text = "Attempts: " + currentAttemptCount.ToString();
    }

    /// <summary>
    /// Проверяет, закончилась ли игра
    /// </summary>
    private void CheckGameEnd()
    {
        if (currentAttemptCount <= 0 || TextManager.wordList.Count <= 0)
        {
            menuScoresText.text = "Game end!\nYour score: " + scores.ToString();
            mainUI.SetActive(false);
            menuUI.SetActive(true);
            gameEnd = true;
        }
        else
        {
            UpdateUI();
        }
    }

    /// <summary>
    /// Отключает кнопку
    /// </summary>
    /// <param name="bttn"></param>
    public void DeactivateButton(Button bttn)
    {
        bttn.interactable = false;
        deactivatedButtons.Add(bttn);
    }

    /// <summary>
    /// Перезапускает игру
    /// </summary>
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}