using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Softball : MonoBehaviour
{
    public GameObject myTarget;
    public GameObject otherTarget;
    public Score gameScore;
    public float myForce = 15f;
    public bool useMin = true;
    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myTarget = GameObject.FindWithTag("MyTarget");
        otherTarget = GameObject.FindWithTag("OtherTarget");
        gameScore = GameObject.FindWithTag("GameManager").GetComponent<Score>();

        Nullable<Vector3> force = Calculate(transform.position, myTarget.transform.position, myForce, Physics.gravity);
        if (force.HasValue)
            rb.AddForce(force.Value.normalized * myForce, ForceMode.VelocityChange);

        Invoke("DestroySelf", 10f);
    }

    public void GetHit()
    {
        rb.velocity = Vector3.zero;

        gameScore.AddScore();

        SetUseMin(false);
        Nullable<Vector3> force = Calculate(transform.position, otherTarget.transform.position, myForce * 1.5f, Physics.gravity);
        if (force.HasValue)
            rb.AddForce(force.Value.normalized * myForce, ForceMode.VelocityChange);
        SetUseMin(true);
    }

    public void SetUseMin(bool useMin)
    {
        this.useMin = useMin;
    }

    public Nullable<Vector3> Calculate(Vector3 start, Vector3 end, float muzzleV, Vector3 gravity)
    {
        Nullable<float> ttt = GetTimeToTarget(start, end, muzzleV, gravity);
        if (!ttt.HasValue)
        {
            return null;
        }

        Vector3 delta = end - start;


        Vector3 n1 = delta * 2;
        Vector3 n2 = gravity * (ttt.Value * ttt.Value);
        float d = 2 * muzzleV * ttt.Value;
        Vector3 solution = (n1 - n2) / d;

        return solution;
    }

    public Nullable<float> GetTimeToTarget(Vector3 start, Vector3 end, float muzzleV, Vector3 gravity)
    {
        Vector3 delta = start - end;

        float a = gravity.magnitude * gravity.magnitude;
        float b = -4 * (Vector3.Dot(gravity, delta) + muzzleV * muzzleV);
        float c = 4 * (delta.magnitude * delta.magnitude);

        float b2minus4ac = (b * b) - (4 * a * c);
        if (b2minus4ac < 0)
        {
            return null;
        }

        float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b2minus4ac)) / (2 * a));
        float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b2minus4ac)) / (2 * a));

        Nullable<float> ttt;
        if (time0 < 0)
        {
            if (time1 < 0)
            {
                return null;
            }

            else
            {
                ttt = time1;
            }
        }

        else if (time1 < 0)
        {
            ttt = time0;
        }

        else
        {
            if (useMin == true)
                ttt = Mathf.Min(time0, time1);
            else
                ttt = Mathf.Max(time0, time1);


        }
        return ttt;
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bat")
        {
            GetHit();
        }
    }
}