using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : BitStrap.Singleton<UIManager>
{
    [Header("Config")]
    public Scrollbar AngerMeter;
    public Text TxtTotalAmount;
    public Text TxtAddedAmount;

    private float m_LastTotalAmount = 0.0f;
    private float m_LastAddedAmount = 0.0f;
    private Vector2 m_TextAddedStartPos;

    IEnumerator Start()
    {
        TxtTotalAmount.text = "0.00$";
        TxtAddedAmount.text = "";
        m_TextAddedStartPos = TxtAddedAmount.rectTransform.anchoredPosition;

        yield return new WaitForEndOfFrame();

        // DEBUGGING:
        /*
        yield return new WaitForSeconds(1f);
        UpdateAngerMeter(0.4f);

        yield return new WaitForSeconds(2f);
        UpdateMoneyAmount(5.50f, 14.50f);
        */
    }

    public void UpdateAngerMeter(float amount)
    {
        LeanTween.value(gameObject, ChangeAngerMeterSize, AngerMeter.size, amount, 1.5f).
            setEase(LeanTweenType.easeOutElastic);
    }
    private void ChangeAngerMeterSize(float value)
    {
        AngerMeter.size = value;
    }

    
    public void UpdateMoneyAmount(float AddedAmount, float NewTotalAmount)
    {
        TxtAddedAmount.rectTransform.anchoredPosition = m_TextAddedStartPos;

        LeanTween.value(gameObject, ChangeTxtAddedAmount, 0f, AddedAmount / 10.0f, 1.5f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() => MoveTxtAddedOut(NewTotalAmount));
    }
    private void ChangeTxtAddedAmount(float value)
    {
        TxtAddedAmount.text = "+ " + value.ToString("c2");
    }
    private void MoveTxtAddedOut(float newTotal)
    {
        LeanTween.moveY(TxtAddedAmount.rectTransform, -90.0f, 1.0f)
            .setDelay(1.0f)
            .setEaseInElastic()
            .setOnComplete(() => ChangeTxtTotalAmount(newTotal));
    }
    private void ChangeTxtTotalAmount(float value)
    {  
        TxtTotalAmount.text = value.ToString("c2");
        TxtAddedAmount.text = "";
    }
}
