using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    void Select();
    void Unselect();
}

public interface IValidatable
{
    float GetScore(); // 100.0 is perfect, 0.0 is bad.
    void SetValidation(int Val); // set the validation for this hot dog. 0 is inactive
    bool IsValidationActive();
}

public class PlayerController : BitStrap.Singleton<PlayerController>
{
    [Header("Config")]
    public GameObject BtnFinishHotDog;
    public Transform HotDogSpawn;
    public Transform HotDogMoveTo;
    public GameObject HotDogPrefab;
    public float[] GuestHappyThresholdPerSauceCount;
    public int CountAngryUntilLost = 6;

    [Space(5f)]
    public IngredientObject IngrCucumber;
    public IngredientObject IngrTomato;
    public IngredientObject IngrCheese;
    public IngredientObject IngrOnion;
    public IngredientObject IngrBanana;

    [Space(5f)]
    public SqueezeBottle BottleKetchup;
    public SqueezeBottle BottleMustard;
    public SqueezeBottle BottleChocolate;


    private ISelectable m_SelectedObject;
    private Order m_SelectedOrder;
    private GameObject m_HotDogObject;

    private List<float> m_Scores = new List<float>();
    private int m_CountAngryCustomers = 0;

    private float m_TotalMoney = 0.0f;
    public float TotalMoney { get { return m_TotalMoney; } }
    public int TotalMoneyAsScore { get { return Mathf.RoundToInt(m_TotalMoney * 100f); } }

    private void Start()
    {
        BtnFinishHotDog.SetActive(false);

        UnityEngine.SceneManagement.SceneManager.LoadScene("main_ui", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
                OnUnpauseGame();
            else
                OnPauseGame();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.GetComponent<MonoBehaviour>() is ISelectable)
            {
                if (m_SelectedObject != null)
                    m_SelectedObject.Unselect();

                m_SelectedObject = hit.transform.GetComponent<MonoBehaviour>() as ISelectable;
                m_SelectedObject.Select();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (m_SelectedObject != null)
            {
                m_SelectedObject.Unselect();
                m_SelectedObject = null;
            }
        }
    }


    public bool OnOrderChosen(Order ChosenOrder)
    {
        if (m_SelectedOrder == null)
        {
            m_SelectedOrder = ChosenOrder;
            BtnFinishHotDog.SetActive(true);

            // Instantiate HotDog
            m_HotDogObject = Instantiate<GameObject>(HotDogPrefab, HotDogSpawn);
            m_HotDogObject.transform.localPosition = Vector3.zero;
            m_HotDogObject.transform.parent = null;

            LeanTween.move(m_HotDogObject, HotDogMoveTo, 0.4f)
                .setEase(LeanTweenType.easeOutCubic)
                .setOnComplete(SetBottleValidity);

            // Ingredients: set the validation counts
            IngrCheese.SetValidation(m_SelectedOrder.GetIngredientCount(IngredientObject.IngredientType.Cheese));
            IngrCucumber.SetValidation(m_SelectedOrder.GetIngredientCount(IngredientObject.IngredientType.Cucumber));
            IngrOnion.SetValidation(m_SelectedOrder.GetIngredientCount(IngredientObject.IngredientType.Onion));
            IngrTomato.SetValidation(m_SelectedOrder.GetIngredientCount(IngredientObject.IngredientType.Tomato));
            IngrBanana.SetValidation(m_SelectedOrder.GetIngredientCount(IngredientObject.IngredientType.Banana));

            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetBottleValidity()
    {
        if (m_SelectedOrder == null) return;

        // Bottles: set whether it is active and a random state
        if (m_SelectedOrder.GetSqueezeBottleActive(SqueezeBottle.SqueezeBottleType.Ketchup))
        {
            BottleKetchup.SetValidation(Random.Range(1, 6));
        }
        else
            BottleKetchup.SetValidation(0);

        if (m_SelectedOrder.GetSqueezeBottleActive(SqueezeBottle.SqueezeBottleType.Mustard))
        {
            BottleMustard.SetValidation(Random.Range(1, 6));
        }
        else
            BottleMustard.SetValidation(0);

        if (m_SelectedOrder.GetSqueezeBottleActive(SqueezeBottle.SqueezeBottleType.Chocolate))
        {
            BottleChocolate.SetValidation(Random.Range(1, 6));
        }
        else
            BottleChocolate.SetValidation(0);
    }

    public bool OrderReachedEnd(Order order)
    {
        if (m_SelectedOrder == order)
        {
            OnFinishHotDogClicked();
            return true;
        }
        else
        {
            if (m_CountAngryCustomers < CountAngryUntilLost)
            {
                m_CountAngryCustomers++;
                UIManager.Instance.UpdateAngerMeter((float)m_CountAngryCustomers / (float)CountAngryUntilLost);
            }
            return false;
        }
    }

    public void OnFinishHotDogClicked()
    {
        // validate the m_SelectedOrder with our hot dog
        if (m_SelectedOrder != null)
        {
            m_Scores.Add(GetFinalScore());

            m_TotalMoney += m_Scores[m_Scores.Count - 1] / 10.0f;
            UIManager.Instance.UpdateMoneyAmount(m_Scores[m_Scores.Count - 1], m_TotalMoney);

            int saucesActive = 0;
            if (BottleChocolate.IsValidationActive()) saucesActive++;
            if (BottleKetchup.IsValidationActive()) saucesActive++;
            if (BottleMustard.IsValidationActive()) saucesActive++;

            if (m_Scores[m_Scores.Count - 1] < GuestHappyThresholdPerSauceCount[saucesActive])
            {
                m_CountAngryCustomers++;
                m_SelectedOrder.GuestIsAngry();

                UIManager.Instance.UpdateAngerMeter((float)m_CountAngryCustomers / (float)CountAngryUntilLost);
            }
            else
            {
                m_SelectedOrder.GuestIsHappy();
            }
        }

        // Back to initial state
        ResetAll();

        LeanTween.move(m_HotDogObject, HotDogSpawn, 0.3f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(()=> { Destroy(m_HotDogObject); });
        
        m_SelectedOrder = null;
        BtnFinishHotDog.SetActive(false);
    }

    private void ResetAll()
    {
        IngrCheese.Reset();
        IngrCucumber.Reset();
        IngrOnion.Reset();
        IngrTomato.Reset();
        IngrBanana.Reset();

        BottleChocolate.Reset();
        BottleKetchup.Reset();
        BottleMustard.Reset();
    }

    private float GetFinalScore()
    {
        float scoreSum = 0.0f; float scoreCounts = 0f;

        /* Ingredients */
        if (IngrCheese.IsValidationActive())
        {
            scoreSum += IngrCheese.GetScore();
            scoreCounts++;
        }
        if (IngrCucumber.IsValidationActive())
        {
            scoreSum += IngrCucumber.GetScore();
            scoreCounts++;
        }
        if (IngrOnion.IsValidationActive())
        {
            scoreSum += IngrOnion.GetScore();
            scoreCounts++;
        }
        if (IngrTomato.IsValidationActive())
        {
            scoreSum += IngrTomato.GetScore();
            scoreCounts++;
        }
        if (IngrBanana.IsValidationActive())
        {
            scoreSum += IngrBanana.GetScore();
            scoreCounts++;
        }

        /* Bottles */
        float bottleMod = 0f;
        if (BottleChocolate.IsValidationActive())
        {
            scoreSum += BottleChocolate.GetScore();
            scoreCounts++;
            bottleMod += 0.2f;
        }
        if (BottleKetchup.IsValidationActive())
        {
            scoreSum += BottleKetchup.GetScore();
            scoreCounts++;
            bottleMod += 0.2f;
        }
        if (BottleMustard.IsValidationActive())
        {
            scoreSum += BottleMustard.GetScore();
            scoreCounts++;
            bottleMod += 0.2f;
        }

        if (scoreCounts <= 0)
            return 0.0f;
        else
            return scoreSum / (scoreCounts - bottleMod);
    }

    public void OnPauseGame()
    {
        UIManager.Instance.TogglePauseScreen();
    }
    public void OnUnpauseGame()
    {
        UIManager.Instance.TogglePauseScreen();
    }
}
