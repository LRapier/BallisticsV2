using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Swing : MonoBehaviour
{
    public Animator anim;
    bool ready = true;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && ready)
        {
            anim.Play("Swing");
            ready = false;
            Invoke("Ready", 1f);
        }
    }

    void Ready()
    {
        anim.Play("Default");
        ready = true;
    }
}
