using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{

    float speed;

    private void Start()
    {
        speed = Random.Range(20, 100);

        Invoke("DestroyCar", 30);
    }

    void Update()
    {
        if (gameObject.tag == "RedCar")
        {
            gameObject.transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        else {
            gameObject.transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        
    }

    void DestroyCar()
    {
        Destroy(gameObject);
    }

}
