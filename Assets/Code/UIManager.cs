using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : BitStrap.Singleton<UIManager>
{
    [Header("Config")]
    public Scrollbar AngerMeter;
    public RectTransform Funke;
    public RectTransform AngryTop;
    public RectTransform AngryBurnt;
    public Vector2 BurntXRange = new Vector2(-430f, -110f);
    public RectTransform Splash;
    public RectTransform ImgGameOver;

    [Space(5f)]
    public Text TxtTotalAmount;
    public Text TxtAddedAmount;

    private float m_LastTotalAmount = 0.0f;
    private float m_LastAddedAmount = 0.0f;
    private Vector2 m_TextAddedStartPos;

    private LTBezierPath m_FunkeBezier;
    private float m_CurrentAngerMeterSize = 0.0f;

    private Vector2 m_CanvasWidthHeight;

    IEnumerator Start()
    {
        TxtTotalAmount.text = "0.00$";
        TxtAddedAmount.text = "";
        m_TextAddedStartPos = TxtAddedAmount.rectTransform.anchoredPosition;

        m_CanvasWidthHeight = new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.width);

        m_FunkeBezier = new LTBezierPath(new Vector3[] {
            new Vector3(-495f, 45f, 0f), new Vector3(-440f, 63f, 0f),
            new Vector3(-440f, 63f, 0f), new Vector3(-324f, 18f, 0f),

            new Vector3(-324f, 18f, 0f), new Vector3(-241f, 38f, 0f),
            new Vector3(-241f, 38f, 0f), new Vector3(-183f, 49f, 0f),
        });

        // Start Positions of Angry Meter
        Funke.anchoredPosition = m_FunkeBezier.point(0.0f);
        AngryBurnt.anchoredPosition = new Vector3(BurntXRange.x, AngryBurnt.anchoredPosition.y, 0f);

        Splash.offsetMin = new Vector2(m_CanvasWidthHeight.x, 0.0f);
        Splash.offsetMax = new Vector2(0.0f, -m_CanvasWidthHeight.y);

        ImgGameOver.localScale = Vector3.zero;

        yield return new WaitForEndOfFrame();

        // DEBUGGING:
        
        /*
        yield return new WaitForSeconds(1f);
        UpdateAngerMeter(0.5f);

        yield return new WaitForSeconds(2f);
        UpdateMoneyAmount(5.50f, 14.50f);

        yield return new WaitForSeconds(1f);
        UpdateAngerMeter(1.0f);
        */
    }

    public void UpdateAngerMeter(float amount)
    {
        LeanTween.value(gameObject, ChangeAngerMeterSize, m_CurrentAngerMeterSize, amount, 1.5f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() => { if (m_CurrentAngerMeterSize >= 1.0f) BlowUpSplash(); });
    }
    private void ChangeAngerMeterSize(float value)
    {
        m_CurrentAngerMeterSize = value;

        Vector3 pt = m_FunkeBezier.point(value); // retrieve a point along the path
        Funke.anchoredPosition = pt;

        AngryBurnt.anchoredPosition = new Vector3(Mathf.Lerp(BurntXRange.x, BurntXRange.y, value), 
            AngryBurnt.anchoredPosition.y, 0f);
    }

    private void BlowUpSplash()
    {
        AngryTop.gameObject.SetActive(false);
        Funke.gameObject.SetActive(false);

        LeanTween.value(gameObject, (float v) =>
        { Splash.offsetMin = new Vector2(v, 0.0f); }, m_CanvasWidthHeight.x - 100f, 0.0f, 0.5f)
           .setEase(LeanTweenType.easeOutCubic);

        LeanTween.value(gameObject, (float v) =>
        { Splash.offsetMax = new Vector2(0.0f, v); }, -m_CanvasWidthHeight.y + 50f, 0.0f, 0.5f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(ShowGameOverScreen);
    }

    private void ShowGameOverScreen()
    {
        LeanTween.scale(ImgGameOver, Vector3.one * 1.5f, 1.0f)
            .setEase(LeanTweenType.easeOutElastic);
    }

    /* For Scrollbar
    public void UpdateAngerMeter(float amount)
    {
        LeanTween.value(gameObject, ChangeAngerMeterSize, AngerMeter.size, amount, 1.5f).
            setEase(LeanTweenType.easeOutElastic);
    }
    private void ChangeAngerMeterSize(float value)
    {
        AngerMeter.size = value;
    }*/


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


    public void OnTryAgainClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("main", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    public void OnBackToMainMenuClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("start", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
