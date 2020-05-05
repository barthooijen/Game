using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RechargeFlockBehaviour : MonoBehaviour
{

    float speed = 4.8f; 
    float rotationSpeed = 1.0f;
    bool turning = false;
    public Flock flocking;
    GameObject character;
    public Image time;


    private void Start()
    {
        character = GameObject.Find("Character");
        transform.parent = GameObject.Find("RechargeFlock(Clone)").transform;
        flocking = GameObject.Find("RechargeFlock(Clone)").GetComponent<Flock>();
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
            if (Random.Range(0, 7) < 1)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        Vector3 direction = flocking.goalPos - transform.position;
       transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        time.fillAmount -= 1.0f / 120 * Time.deltaTime;
        if (time.fillAmount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
