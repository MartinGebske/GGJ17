using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreEntry : MonoBehaviour
{
    [Header("Config")]
    public Image ImgBackground;
    public Text Position;
    public Text Name;
    public Text Money;

    private RectTransform m_RectTrans;

    public void InitEntry(string name, int score)
    {
        Position.text = "#" + (transform.parent.childCount);
        Name.text = name;
        Money.text = ((float)score / 100f).ToString("c2");

        // color it if it was the recent addition of this player
        if (name == PlayerPrefs.GetString("last_name", "") && score == PlayerPrefs.GetInt("last_score", -1))
            ImgBackground.color = new Color(1f, 207f / 255f, 105f / 255f);

        m_RectTrans = GetComponent<RectTransform>();
        m_RectTrans.sizeDelta = new Vector2(0f, m_RectTrans.sizeDelta.y);

        LeanTween.value(gameObject, (float v) => 
        {
            m_RectTrans.sizeDelta = new Vector2(v, m_RectTrans.sizeDelta.y);
        }, 0f, 1000f, 0.8f)
            .setEase(LeanTweenType.easeOutElastic)
            .setDelay(transform.parent.childCount / 8f);
    }
}
