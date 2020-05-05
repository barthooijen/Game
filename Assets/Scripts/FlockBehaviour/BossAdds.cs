using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAdds : MonoBehaviour
{
    float speed = 5f;
    float rotationSpeed = 2.0f;
    float distChar;

    bool turning = false;
    bool following = false;

    GameObject character;
    public GameObject boss;

    Vector3 randomPos;
    Vector3 goalPos;

    private void Start()
    {
        character = GameObject.Find("Character");
        boss = GameObject.Find("Boss1(Clone)");
    }

    void Update()
    {
        if (Vector3.Distance(character.transform.position, transform.position) <= 8)
        {

            following = true;
            Vector3 direction = character.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            speed = 6f;
            rotationSpeed = 5f;
        }
        else
        {
            following = false;
            speed = 4f;
            rotationSpeed = 2.0f;

          //  if (Random.Range(0, 30) < 1)
            {

                if (Random.Range(0, 10000) < 50)
                {

                    randomPos = new Vector3(Random.Range(-15, 15),
                                            Random.Range(0, 0),
                                            Random.Range(-15, 15));
                    goalPos = this.transform.position + randomPos;
                }
                if (Vector3.Distance(transform.position, boss.transform.position) >= 15)
                {
                    turning = true;
                }
                else
                    turning = false;
            }

            if (turning)
            {
                speed = 2f;
                Vector3 direction = boss.transform.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
            else
            {
                if
                    (Random.Range(0, 6) < 1 && following == false)
                    ApplyRules();
            }
        }

        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        Vector3 direction = goalPos - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
    }
}
