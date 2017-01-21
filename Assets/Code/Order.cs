using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class Order : MonoBehaviour, IPointerClickHandler
{

    [Tooltip("Welche Zutaten werden gebraucht?")]
    public GameObject[] ingredients;

    public GameObject[] sauces;

    public float endposition;

    public AudioClip guestHappySound;
    public AudioClip guestAngrySound;

    public Sprite[] zettelImages;

   // [HideInInspector] public int currentLevel;
    [HideInInspector] public int currentIngredients;
  

    private AudioSource audioSource;

    private OrderManager orderManager;

    private RectTransform orderTransform;

    private SpriteRenderer sprRenderer;

    private bool reachedEndPos = false;

    void Start ()
    {

        sprRenderer = GetComponent<SpriteRenderer>();

        orderTransform = GetComponent<RectTransform>();

        orderManager = FindObjectOfType<OrderManager>();

        int currentLevel = orderManager.levelOnCreation;

        audioSource = GetComponent<AudioSource>();

        currentIngredients = CalculateMaxIngredients(currentLevel);

        SelectBackgroundSprite();

        FillIngredients();

        SelectSauce();
    }

    void Update()
    {
        orderTransform.Translate(Vector3.right * orderManager.speedOnCreation * Time.deltaTime);

        if (!reachedEndPos && orderTransform.anchoredPosition.x > endposition)
        {
            PlayerController.Instance.OnFinishHotDogClicked();
            reachedEndPos = true;
        }

    }

    public void GuestIsHappy()
    {
        audioSource.PlayOneShot(guestHappySound);
        StartCoroutine(WaitForDestroy());
    }

    public void GuestIsAngry()
    {
        audioSource.PlayOneShot(guestAngrySound);
        StartCoroutine(WaitForDestroy());
    }

    void SelectBackgroundSprite()
    {
        int select = UnityEngine.Random.Range(1, 2);
        sprRenderer.sprite = zettelImages[select];
    }

    void FillIngredients()
    {
        List<int> generatedIndices = new List<int>();
        while(generatedIndices.Count < currentIngredients)
        {
           int i = Mathf.FloorToInt (UnityEngine.Random.Range(0F, 1F) * (float)(ingredients.Length));

            if (!generatedIndices.Contains(i))
            { generatedIndices.Add(i); }
        }

        List<GameObject> chosenIng = new List<GameObject>();
        foreach(int i in generatedIndices)
        {
            chosenIng.Add(ingredients[i]);
            ingredients[i].SetActive(true);
        }
    }

    void SelectSauce()
    {
        int sauceCount = UnityEngine.Random.Range(1, 4);

        List<int> sauceIndices = new List<int>();
        while (sauceIndices.Count < sauceCount)
        {
            int i = Mathf.FloorToInt(UnityEngine.Random.Range(0F, 1F) * (float)(sauces.Length));

            if (!sauceIndices.Contains(i))
            { sauceIndices.Add(i); }
        }
        List<GameObject> chosenSauces = new List<GameObject>();
        foreach (int i in sauceIndices)
        {
            chosenSauces.Add(sauces[i]);
            sauces[i].SetActive(true);
        }
    }
   
    int CalculateMaxIngredients(int level)
    {
        int maxIngredients = 0;

        switch (level)
        {
            case 0:
                maxIngredients = 1;
            break;

            case 1:
            case 2:
                maxIngredients = 2;
                break;

            case 3:
            case 4:
                maxIngredients = 3;
                break;

            case 5:
            case 6:
                maxIngredients = 4;
                break;

            case 7:
            case 8:
            case 9:
                maxIngredients = 5;
                break;

            default:
            case 10:
                maxIngredients = ingredients.Length - 1;
                break;
        }

        return maxIngredients;
    }
    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool wasSelected = PlayerController.Instance.OnOrderChosen(this);

        // TODO feedback (highlight OR "no access")
        if (wasSelected)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 86.0f / 255.0f);
        }
        else
        {

        }
    }

    public int GetIngredientCount(IngredientObject.IngredientType OfType)
    {
        int count = 0;

        switch (OfType)
        {
            case IngredientObject.IngredientType.Tomato:
                if (ingredients[3].activeSelf) count++;
                if (ingredients[4].activeSelf) count++;
                if (ingredients[5].activeSelf) count++;
                break;
            case IngredientObject.IngredientType.Cucumber:
                if (ingredients[0].activeSelf) count++;
                if (ingredients[1].activeSelf) count++;
                if (ingredients[2].activeSelf) count++;
                break;
            case IngredientObject.IngredientType.Cheese:
                if (ingredients[6].activeSelf) count++;
                if (ingredients[7].activeSelf) count++;
                if (ingredients[8].activeSelf) count++;
                break;
            case IngredientObject.IngredientType.Onion:
                if (ingredients[9].activeSelf) count++;
                if (ingredients[10].activeSelf) count++;
                if (ingredients[11].activeSelf) count++;
                break;
            case IngredientObject.IngredientType.Banana:
                if (ingredients[12].activeSelf) count++;
                if (ingredients[13].activeSelf) count++;
                if (ingredients[14].activeSelf) count++;
                break;
            default:
                break;
        }

        return count;
    }

    public bool GetSqueezeBottleActive(SqueezeBottle.SqueezeBottleType OfType)
    {
        switch (OfType)
        {
            case SqueezeBottle.SqueezeBottleType.Ketchup:
                return sauces[0].activeSelf;
            case SqueezeBottle.SqueezeBottleType.Mustard:
                return sauces[1].activeSelf;
            case SqueezeBottle.SqueezeBottleType.Chocolate:
                return sauces[2].activeSelf;
            default:
                return false;
        }
    }
}
