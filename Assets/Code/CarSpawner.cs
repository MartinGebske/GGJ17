using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class CarSpawner : MonoBehaviour
{
    public GameObject car;


    void Start()
    {
        StartCoroutine(WaitForNewCar());
    }

    void SetUpCar()
    {
  
        GameObject t = Instantiate(car, transform.position, Quaternion.identity);
        if (t.tag == "RedCar")
            t.transform.Rotate(0.0f, 180.0f, 0.0f);
        else
            t.transform.Rotate(0, 0, 0);

        StartCoroutine(WaitForNewCar());
    }

    IEnumerator WaitForNewCar()
    {
        float waitingTime = Random.Range(3, 10);
        yield return new WaitForSeconds(waitingTime);
        SetUpCar();
    }

}
