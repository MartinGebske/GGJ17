using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : BitStrap.Singleton<OrderManager>
{

    public GameObject orderSlip;
    [Range(0,10)] public float groundspeed = 1F;
    [Range(1, 50)] public float minimumSpawnSpeed = 1F;
    [Range(1, 100)] public float maximumSpawnSpeed = 1F;
    public float secondsPerLevel = 10.0f;

    [HideInInspector] public float orderSpawnTime;
    [HideInInspector] public int levelOnCreation;
    [HideInInspector] public float speedOnCreation;

    public bool StopSpawningOrders = false;

    void Start()
    {
        StartOrderingLoop();
    }

    public void StartOrderingLoop()
    {
        levelOnCreation = CalculateLevel();
        orderSpawnTime = CalculateSpawnTime(levelOnCreation);
        speedOnCreation = CalculateSpeed(levelOnCreation);
        StartCoroutine("CreateOrder");
    }

    IEnumerator CreateOrder()
    {
        if (!StopSpawningOrders)
        {

            (Instantiate(orderSlip, transform.position, transform.rotation) as GameObject).transform.SetParent(this.transform, false);

            yield return new WaitForSeconds(orderSpawnTime);

            StartOrderingLoop();
        }
    }

     float CalculateSpawnTime(int level)
        {
        float reduction = -4f + level;
        float min = minimumSpawnSpeed + reduction; /// (level +1);
        float max = maximumSpawnSpeed + reduction; /// (level +1);

         float spawnTime = Random.Range(min, max);

         return spawnTime;
        }

    int CalculateLevel()
    {
        float currentTime = Time.timeSinceLevelLoad;

        int level = Mathf.FloorToInt(currentTime / secondsPerLevel);

        level = Mathf.Clamp(level, 0, 10);

        Debug.Log("Aktuelles Level = " + level);

        return level;
    }

    float CalculateSpeed(int level)
    {
        float thisLevel = level;
        
        return groundspeed + (thisLevel) / 10;
    }
}
