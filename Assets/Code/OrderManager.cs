using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{

    public GameObject orderSlip;


    [HideInInspector] public float orderSpawnTime;
    [HideInInspector] public int levelOnCreation;


    void Start()
    {
        StartOrderingLoop();
    }

    void StartOrderingLoop()
    {
        orderSpawnTime = CalculateSpawnTime();
        levelOnCreation = CalculateLevel();
        StartCoroutine("CreateOrder");
    }

    IEnumerator CreateOrder()
    {
        Instantiate(orderSlip);

        yield return new WaitForSeconds(orderSpawnTime);

        StartOrderingLoop();
    }

     float CalculateSpawnTime()
        {
         float spawnTime = Random.Range(5F, 10F);

         return spawnTime;
        }

    int CalculateLevel()
    {
        float currentTime = Time.timeSinceLevelLoad;

        int roundedTime = Mathf.RoundToInt(currentTime);

        int level = Mathf.FloorToInt(roundedTime / 10);

        Mathf.Clamp(level, 0, 5);

        return level;
    }
}
