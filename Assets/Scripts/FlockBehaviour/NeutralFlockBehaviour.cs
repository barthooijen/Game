using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralFlockBehaviour : MonoBehaviour
{
    float speed = 3f;
    float neighbourDistance = 20.0f;
    float rotationSpeed = 2.0f;
    bool turning = false;
    public Flock flocking;
    Vector3 direction;
    Vector3 averageHeading;
    Vector3 avaragePosition;

    private void Start()
    {
        transform.parent = GameObject.Find("NeutralFlock(Clone)").transform;
        flocking = GameObject.Find("NeutralFlock(Clone)").GetComponent<Flock>();  
    }

    void Update()
    {

            if (Random.Range(0, 30) < 1)
        {
            if (Vector3.Distance(transform.position, flocking.spawn.transform.position) >= flocking.flockSize)
            {
                turning = true;
                speed = Random.Range(3.0f, 5.0f);
            }
            else
                turning = false;
        }

        if (turning)
        {
            Vector3 direction = flocking.spawn.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }
        else
        {
           if (Random.Range(0, 3) < 1 )
            ApplyRules();
        }

        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        List<GameObject> gos;
        gos = GameObject.Find("NeutralFlock(Clone)").GetComponent<Flock>().allUnits;
        Vector3 goalPos = flocking.goalPos;
        speed = 3f;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;

       // float gSpeed = 0.1f;
        float dist;

        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if (dist <= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if (dist < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    NeutralFlockBehaviour anotherFlock = go.GetComponent<NeutralFlockBehaviour>();
                   // gSpeed = gSpeed + anotherFlock.speed;
                }
            }


        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);
           // speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }         
    }

}
