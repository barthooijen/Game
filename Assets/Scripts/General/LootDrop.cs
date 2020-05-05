using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    private Vector3 velocity = Vector3.up;
    private Rigidbody rb;
    private Vector3 startPosition;
    private bool stop = false;


    void Start()
    {
        startPosition = this.transform.position;
        rb = this.GetComponent<Rigidbody>();
        velocity *= Random.Range(4f, 6f);
        velocity += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }

    void Update()
    {
        if (stop == false)
        {
            rb.position += velocity * Time.deltaTime;
        }
        else
            rb.velocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {   
        if (other.tag == "terrain")
        {
            rb.useGravity = false;
            stop = true;
        }
    }
}
