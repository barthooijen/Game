using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAddCirlce : MonoBehaviour
{

    public GameObject boss;




    void Start()
    {

        Invoke("destroy", 10);
    }


    void destroy()
    
    {
        Destroy(gameObject);
    }
}

