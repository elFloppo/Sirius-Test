using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class TextManager : MonoBehaviour
{
    [SerializeField] GameSettings gameSettings;

    /// <summary>
    /// Исходный текст
    /// </summary>
    private string stringText;

    /// <summary>
    /// Преобразованный текст
    /// </summary>
    private string newStringText;

    /// <summary>
    /// Список слов
    /// </summary>
    [HideInInspector] public static List<string> wordList = new List<string>();   

    private void Start()
    {
        stringText = gameSettings.originalText.ToString();
        newStringText = Regex.Replace(stringText, "(?i)[^A-Z  \f\n\r\t\v]", " ");
        FillWordList();
    }

    /// <summary>
    /// Создает список подходящих слов
    /// </summary>
    private void FillWordList()
    {
        string[] words = newStringText.Split(' ', '\n');

        foreach (string word in words)
        {
            if (word != string.Empty && word.Length >= gameSettings.minWordLength && !wordList.Contains(word.ToUpper()))
            {
                Debug.Log(word.ToUpper());
                wordList.Add(word.ToUpper());
            }
        }       
    }
}
