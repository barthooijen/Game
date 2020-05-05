using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatFlockBehaviour : MonoBehaviour
{
    float speed = 8f;
    float neighbourDistance = 10.0f;
    float rotationSpeed = 3.0f;
    bool turning = false;
    public Flock flocking;
    Vector3 direction;
    Vector3 averageHeading;
    Vector3 avaragePosition;
    public Material blue;

    private void Start()
    {
        transform.parent = GameObject.Find("CombatFlock(Clone)").transform;
        flocking = GameObject.Find("CombatFlock(Clone)").GetComponent<Flock>();
    }

    void Update()
    {
        if (Random.Range(0, 30) < 1)
        {
            if (Vector3.Distance(transform.position, flocking.spawn.transform.position) >= flocking.flockSize)
            {
                turning = true;
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
            if (Random.Range(0, 6) < 1)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);   
}


    void ApplyRules()
    {
        List<GameObject> gos;
        gos = GameObject.Find("CombatFlock(Clone)").GetComponent<Flock>().allUnits;
        Vector3 goalPos = flocking.goalPos;
        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
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
                    CombatFlockBehaviour anotherFlock = go.GetComponent<CombatFlockBehaviour>();
                }
            }
        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }
    }

    public void destroy()
    {
        {         
            this.GetComponent<Renderer>().material = blue;
            this.GetComponent<MeshCollider>().enabled = false;
            speed = 4.5f;
            rotationSpeed = 2f;
        }
    }
}
