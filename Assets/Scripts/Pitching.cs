using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitching : MonoBehaviour
{
    public GameObject ball;
    public Transform spawner;
    
    void Start()
    {
        InvokeRepeating("Spawn", 0f, 5f);
    }

    void Spawn()
    {
        Instantiate(ball, spawner);
    }
}
