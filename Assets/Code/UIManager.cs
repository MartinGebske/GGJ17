using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : BitStrap.Singleton<UIManager>
{
    [Header("Config")]
    public Scrollbar AngerMeter;
    public RectTransform Funke;
    public Image AngryAttention;
    public RectTransform AngryTop;
    public RectTransform AngryBurnt;
    public GameObject AngryFaceAnimation;
    public Vector2 BurntXRange = new Vector2(-430f, -110f);
    public RectTransform Splash;
    public RectTransform ImgGameOver;
    public RectTransform ImgPause;
    public Button MobilePauseBtn;
    public HighscoreSubmit HighScoreSubmit;

    [Space(5f)]
    public Text TxtTotalAmount;
    public Text TxtAddedAmount;

    private float m_LastTotalAmount = 0.0f;
    private float m_LastAddedAmount = 0.0f;
    private Vector2 m_TextAddedStartPos;

    private LTBezierPath m_FunkeBezier;
    private float m_CurrentAngerMeterSize = 0.0f;

    private Vector2 m_CanvasWidthHeight;

    private bool m_ShowingGameOverScreen = false;

    IEnumerator Start()
    {
        TxtTotalAmount.text = "0.00$";
        TxtAddedAmount.gameObject.SetActive(false);
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
        HighScoreSubmit.GetComponent<RectTransform>().localScale = Vector3.zero;
        ImgPause.localScale = Vector3.zero;

        AngryFaceAnimation.SetActive(false);

        if (Application.platform != RuntimePlatform.WindowsEditor
            && Application.platform != RuntimePlatform.WindowsPlayer
            && Application.platform != RuntimePlatform.LinuxEditor
            && Application.platform != RuntimePlatform.LinuxPlayer
            && Application.platform != RuntimePlatform.OSXDashboardPlayer
            && Application.platform != RuntimePlatform.OSXEditor
            && Application.platform != RuntimePlatform.OSXPlayer)
        {
            MobilePauseBtn.gameObject.SetActive(true);
        }
        else
        {
            MobilePauseBtn.gameObject.SetActive(false);
        }

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
        LeanTween.value(gameObject, (float v) => { AngryAttention.color = new Color(1f, 1f, 1f, v); }, 0.0f, 1.0f, 1.0f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() =>
            {
                LeanTween.value(gameObject, (float v) => { AngryAttention.color = new Color(1f, 1f, 1f, v); }, 1.0f, 0.0f, 1.0f)
                    .setEase(LeanTweenType.easeInCubic);
            });

        LeanTween.value(gameObject, (float v) => { Funke.localScale = new Vector3(v,v); }, 1f, 10f, 1.0f)
           .setEase(LeanTweenType.easeOutCubic)
           .setOnComplete(() =>
           {
               LeanTween.value(gameObject, (float v) => { Funke.localScale = new Vector3(v, v); }, 10f, 1f, 1f)
                   .setEase(LeanTweenType.easeInCubic);
           });

        AngryFaceAnimation.SetActive(true);

        AudioManager.Instance.PlayLunte();

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
        if (m_ShowingGameOverScreen)
            return;

        AudioManager.Instance.PlayExplode();

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
        m_ShowingGameOverScreen = true;
        OrderManager.Instance.StopSpawningOrders = true;

        LeanTween.scale(ImgGameOver, Vector3.one * 1.5f, 1.0f)
            .setEase(LeanTweenType.easeOutElastic);

        LeanTween.scale(HighScoreSubmit.GetComponent<RectTransform>(), Vector3.one, 1.0f)
                    .setEase(LeanTweenType.easeOutElastic)
                    .setDelay(0.5f);

        HighScoreSubmit.Init(PlayerController.Instance.TotalMoneyAsScore);

        AudioManager.Instance.PlayGameOver();
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
        TxtAddedAmount.gameObject.SetActive(true);
        TxtAddedAmount.rectTransform.anchoredPosition = m_TextAddedStartPos;

        LeanTween.value(gameObject, ChangeTxtAddedAmount, 0f, AddedAmount / 10.0f, 1.5f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() => MoveTxtAddedOut(NewTotalAmount, AddedAmount));
    }
    private void ChangeTxtAddedAmount(float value)
    {
        TxtAddedAmount.text = value.ToString("c2");
    }
    private void MoveTxtAddedOut(float newTotal, float AddedAmount)
    {
        if (AddedAmount > 1f)
            AudioManager.Instance.PlayCash();

        LeanTween.moveY(TxtAddedAmount.rectTransform, -90.0f, 1.0f)
            .setDelay(1.0f)
            .setEaseInElastic()
            .setOnComplete(() => ChangeTxtTotalAmount(newTotal));
    }
    private void ChangeTxtTotalAmount(float value)
    {  
        TxtTotalAmount.text = value.ToString("c2");
        TxtAddedAmount.gameObject.SetActive(false);
    }


    public void OnTryAgainClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("main", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    public void OnBackToMainMenuClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("start", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void OnPauseClicked()
    {
        PlayerController.Instance.OnPauseGame();
    }
    public void OnResumeClicked()
    {
        PlayerController.Instance.OnUnpauseGame();
    }
    public void TogglePauseScreen()
    {
        if (Time.timeScale == 1)
        {
            LeanTween.scale(ImgPause, Vector3.one * 1.5f, 0.8f)
            .setEase(LeanTweenType.easeOutElastic)
            .setOnComplete(() => { Time.timeScale = 0; });
        }
        else
        {
            Time.timeScale = 1;
            LeanTween.scale(ImgPause, Vector3.zero, 0.5f)
            .setEase(LeanTweenType.easeInCubic);
        }
    }
}
