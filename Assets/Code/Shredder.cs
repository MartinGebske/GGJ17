using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    public delegate void OnOutOfGame();
    public static event OnOutOfGame OnOutOfGameEvent;

    void OnTriggerEnter2D(Collider2D collision)
    {
        OnOutOfGameEvent();
    }
}
