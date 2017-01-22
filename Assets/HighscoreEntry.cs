using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreEntry : MonoBehaviour
{
    [Header("Config")]
    public Text Position;
    public Text Name;
    public Text Money;
	
    public void InitEntry(string name, int score)
    {
        Position.text = "#" + (transform.parent.childCount);
        Name.text = name;
        Money.text = ((float)score / 100f).ToString("c2");
    }
}
