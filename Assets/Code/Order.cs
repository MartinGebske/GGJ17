using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Order : MonoBehaviour
{

    [Tooltip("Welche Zutaten werden gebraucht?")]
    public GameObject[] ingredients;

    public GameObject[] sauces;

    public float endposition;

    public AudioClip guestHappySound;
    public AudioClip guestAngrySound; 
   

   // [HideInInspector] public int currentLevel;
    [HideInInspector] public int currentIngredients;
  

    private AudioSource audioSource;

    private OrderManager orderManager;

    RectTransform orderTransform;

    void Start ()
    {
        orderTransform = GetComponent<RectTransform>();

        orderManager = FindObjectOfType<OrderManager>();

        int currentLevel = orderManager.levelOnCreation;

        audioSource = GetComponent<AudioSource>();

        currentIngredients = CalculateMaxIngredients(currentLevel);

        FillIngredients();
        SelectSauce();
    }

    void Update()
    {
        orderTransform.Translate(Vector3.right * orderManager.speedOnCreation);

        if (orderTransform.anchoredPosition.x > endposition)
        { GuestIsAngry(); }
    }

    public void GuestIsHappy()
    {
        audioSource.PlayOneShot(guestHappySound);
        GameManager.GetInstance(true).orders++;
    }

    public void GuestIsAngry()
    {
        Debug.Log("Bestellung nicht ausgeführt!!!");
        audioSource.PlayOneShot(guestAngrySound);
        StartCoroutine(WaitForDestroy());
    }

    void FillIngredients()
    {
        List<int> generatedIndices = new List<int>();
        while(generatedIndices.Count < currentIngredients)
        {
           int i = Mathf.FloorToInt (Random.Range(0F, 1F) * (float)(ingredients.Length));

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
        int sauceCount = Random.Range(1, 4);

        List<int> sauceIndices = new List<int>();
        while (sauceIndices.Count < sauceCount)
        {
            int i = Mathf.FloorToInt(Random.Range(0F, 1F) * (float)(sauces.Length));

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
                maxIngredients = 2;
                break;

            case 2:
                maxIngredients = 2;
                break;

            case 3:
                maxIngredients = 4;
                break;

            case 4:
                maxIngredients = 4;
                break;

            default:
            case 5:
                maxIngredients = ingredients.Length;
                break;
        }

        return maxIngredients;
    }
    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
