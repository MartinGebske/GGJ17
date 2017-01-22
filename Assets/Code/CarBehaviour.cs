using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{

    float speed;
    float lastX;

    private void Start()
    {
        lastX = transform.position.x;

        speed = Random.Range(50, 100);

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
        
        if (lastX < 0f && transform.position.x > 0f)
            AudioManager.Instance.PlayCar();
        else if (lastX > 0f && transform.position.x < 0f)
            AudioManager.Instance.PlayCar();

        lastX = transform.position.x;
    }

    void DestroyCar()
    {
        Destroy(gameObject);
    }

}
