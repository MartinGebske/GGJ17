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

        if (orderTransform.anchoredPosition.x > endposition)
        { GuestIsAngry(); }

    }

    public void GuestIsHappy()
    {
        audioSource.PlayOneShot(guestHappySound);
        GameManager.GetInstance(true).orders++;
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

        }
        else
        {

        }
    }
}
