using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterController : MonoBehaviour
{
    [SerializeField] GameObject letterLock;
    [SerializeField] TextMeshProUGUI letterText;
    [HideInInspector] public char letter;

    public void SetLetter(char lttr)
    {
        letterText.text = lttr.ToString();
        letter = lttr;
    }

    public void SetLetterLock(bool locked)
    {
        letterLock.SetActive(locked) ;
    }
}
