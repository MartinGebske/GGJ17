using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour {

    [Header("Config")]
    public Sprite[] AnimationSprites;
    public float AnimationTime = 0.05f;
    [Tooltip("-1 means infinite animation")]
    public int CountShots = -1;

    private Image m_Image;
    private float m_TimeSinceLastChange = 0f;
    private int m_CurrImage = 0;
    private int m_CurrShot = 0;

    private void Start()
    {
        m_Image = GetComponent<Image>();
        m_Image.sprite = AnimationSprites[m_CurrImage];
    }

    void Update ()
    {
        m_TimeSinceLastChange += Time.deltaTime;

        if (m_TimeSinceLastChange > AnimationTime)
        {
            m_TimeSinceLastChange -= AnimationTime;
            m_CurrImage = (m_CurrImage + 1) % AnimationSprites.Length;

            if (CountShots > -1 && m_CurrImage == 0)
            {
                m_CurrShot++;
                if (m_CurrShot >= CountShots)
                {
                    m_TimeSinceLastChange = 0f;
                    m_CurrShot = 0;
                    m_Image.sprite = AnimationSprites[m_CurrImage];
                    gameObject.SetActive(false);
                    return;
                }
            }
            m_Image.sprite = AnimationSprites[m_CurrImage];
        }
	}
}
