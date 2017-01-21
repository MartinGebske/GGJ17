using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{

    public GameObject orderSlip;
    [Range(0,10)] public float groundspeed = 1F;
    [Range(5, 50)] public float minimumSpawnSpeed = 1F;
    [Range(15, 100)] public float maximumSpawnSpeed = 1F;
    public float secondsPerLevel = 10.0f;

    [HideInInspector] public float orderSpawnTime;
    [HideInInspector] public int levelOnCreation;
    [HideInInspector] public float speedOnCreation;


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

       (Instantiate(orderSlip, transform.position, transform.rotation) as GameObject).transform.SetParent(this.transform, false);
 
        yield return new WaitForSeconds(orderSpawnTime);

        StartOrderingLoop();
    }

     float CalculateSpawnTime(int level)
        {
        float thisLevel = level;
        float min = minimumSpawnSpeed; /// (level +1);
        float max = maximumSpawnSpeed; /// (level +1);

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
        
        return groundspeed + (thisLevel + 1) / 10;
    }
}
