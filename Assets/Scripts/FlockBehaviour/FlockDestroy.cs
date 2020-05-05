using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockDestroy : MonoBehaviour
{


    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Player")
        {
            CombatFlockBehaviour kill = gameObject.GetComponentInParent<CombatFlockBehaviour>();
            kill.destroy();
            this.gameObject.SetActive(false);
        }
    }
}
