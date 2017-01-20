using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Order : MonoBehaviour
{

    [Tooltip("Welche Zutaten werden gebraucht?")]
    public GameObject[] ingredients;

    public AudioClip guestHappySound;
    public AudioClip guestAngrySound; 
   

   // [HideInInspector] public int currentLevel;
    [HideInInspector] public int currentIngredients;

    private AudioSource audioSource;

    private OrderManager orderManager;
    

    void Start ()
    {
        orderManager = FindObjectOfType<OrderManager>();

        int currentLevel = orderManager.levelOnCreation;

        audioSource = GetComponent<AudioSource>();

        currentIngredients = CalculateMaxIngredients(currentLevel);

        FillIngredients();

        StartCoroutine("OrderIsActive");
    }

    public void GuestIsHappy()
    {
        StopCoroutine("OrderIsActive");
        audioSource.PlayOneShot(guestHappySound);
        GameManager.GetInstance(true).orders++;
    }

    void GuestIsAngry()
    {
        audioSource.PlayOneShot(guestAngrySound);
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
            foreach (GameObject activeIngredient in ingredients)
            {
                activeIngredient.SetActive(true);
            }
        }
    }
   
    int CalculateMaxIngredients(int level)
    {
        int maxIngredients = 0;

        switch (level)
        {
            case 0:
                maxIngredients = 2;
            break;

            case 1:
                maxIngredients = 3;
                break;

            case 2:
                maxIngredients = 4;
                break;

            case 3:
                maxIngredients = 5;
                break;

            case 4:
                maxIngredients = 6;
                break;

            case 5:
                maxIngredients = 7;
                break;
        }

        return maxIngredients;
    }
}
