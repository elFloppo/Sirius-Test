using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public int minWordLength;
    public int attemptCount;
    public TextAsset originalText;
}
