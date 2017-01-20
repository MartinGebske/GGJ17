using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{

    public GameObject orderSlip;

  //  RectTransform parentTransform;

    [HideInInspector] public float orderSpawnTime;
    [HideInInspector] public int levelOnCreation;
    [HideInInspector] public float speedOnCreation;


    void Start()
    {
        StartOrderingLoop();
    }

    void StartOrderingLoop()
    {
        levelOnCreation = CalculateLevel();
        orderSpawnTime = CalculateSpawnTime(levelOnCreation);
        speedOnCreation = CalculateSpeed(levelOnCreation);
        StartCoroutine("CreateOrder");
    }

    IEnumerator CreateOrder()
    {

        // Erstellt den order Slip
        (Instantiate(orderSlip, transform.position, transform.rotation) as GameObject).transform.parent = this.transform;

         yield return new WaitForSeconds(orderSpawnTime);

        StartOrderingLoop();
    }

     float CalculateSpawnTime(int level)
        {
        float thisLevel = level;
        float min = 5F / (level +1);
        float max = 10F / (level +1);

         float spawnTime = Random.Range(min, max);

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

    float CalculateSpeed(int level)
    {
        float thisLevel = level;

        float groundspeed = 1;

        groundspeed += thisLevel;

        return groundspeed;
    }
}
