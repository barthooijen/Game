using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1 : MonoBehaviour
{
    float speed;
    float minSpeed = 5f;
    float maxSpeed = 22f;

    enum Gamestate { state00, state01, state02, state03, state04 }
    Gamestate currentState;
    float lastStateChange = 0.0f;

    GameObject character;
    public GameObject addSpawn;
    public GameObject addSpawnCircle;
    bool isAttacking;
    Vector3 randomPos;
    Vector3 goalPos;

    bool charging;

    void Start()
    {
        SetCurrentState(Gamestate.state00);
        character = GameObject.Find("Character");
        Invoke("spawnadds", 2f);
        isAttacking = false;
    }

    void Update()
    {   
        switch (currentState)
        {
            case Gamestate.state00:
                isAttacking = false;
                Vector3 direction = character.transform.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 2f * Time.deltaTime);

                speed = Mathf.Clamp(speed, 0f, minSpeed);
                speed -= 1f * Time.deltaTime;
                transform.Translate(0, 0, Time.deltaTime * minSpeed);

                if (Random.Range(0, 500) < 1 && GetStateElapsed() > 5f)
                {
                    Gamestate pickone = GetRandomEnum<Gamestate>();
                    SetCurrentState(pickone);
                }              
                break;

            case Gamestate.state01:
                if (isAttacking == false)
                {
                    StartCoroutine(ChargeAttack());
                }
                if (isAttacking && !charging)
                {
                    Vector3 chargedirection = character.transform.position - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(chargedirection), 2f * Time.deltaTime);
                }
                if (isAttacking && charging)
                {
                    Vector3 chargedirection = character.transform.position - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(chargedirection), 2f * Time.deltaTime);


                    speed = Mathf.Clamp(speed, 0f, maxSpeed);
                    speed += 100f * Time.deltaTime;
                    transform.position += transform.forward * Time.deltaTime * speed;
                }

                break;

            case Gamestate.state02:
               transform.Translate(0, 0, Time.deltaTime * 2f);
                if (isAttacking == false)
                {
                   StartCoroutine(AOEAttack());
                }               
                break;

            case Gamestate.state03:
                if (isAttacking == false)
                {
                    StartCoroutine(ChargeAttack());
                }
                if (isAttacking && !charging)
                {
                    Vector3 chargedirection = character.transform.position - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(chargedirection), 2f * Time.deltaTime);
                }
                if (isAttacking && charging)
                {
                    Vector3 chargedirection = character.transform.position - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(chargedirection), 2f * Time.deltaTime);


                    speed = Mathf.Clamp(speed, 0f, maxSpeed);
                    speed += 100f * Time.deltaTime;
                    transform.position += transform.forward * Time.deltaTime * speed;
                }
                break;

            case Gamestate.state04:
                break;
        }
    }

    void spawnadds()
    {
        Vector3 randomSpawn;
        randomSpawn = new Vector3(Random.insideUnitSphere.x + 30, transform.position.y, Random.insideUnitSphere.z + 30);
        Instantiate(addSpawn, Quaternion.Euler(0, Random.Range(0, 360), 0) * randomSpawn + transform.position, Quaternion.identity);
    }

    void spawnaddCirle()
    {
        Vector3 randomSpawn;
        randomSpawn = new Vector3(Random.Range(-15, 15), transform.position.y, Random.Range(-15, 15)) + transform.position;
        Instantiate(addSpawnCircle, randomSpawn, Quaternion.identity);
    }

    private IEnumerator ChargeAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        charging = true;
        yield return new WaitForSeconds(1.5f);
        charging = false;
        SetCurrentState(Gamestate.state00);
    }

    private IEnumerator AOEAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.5f);
        spawnaddCirle();
        yield return new WaitForSeconds(0.5f);
        spawnaddCirle();
        yield return new WaitForSeconds(0.5f);
        spawnaddCirle();
        yield return new WaitForSeconds(0.5f);
        spawnaddCirle();
        SetCurrentState(Gamestate.state00);
    }

    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
       // T V = (T)A.GetValue(UnityEngine.Random.Range(1, A.Length));
        T V = (T)A.GetValue(UnityEngine.Random.Range(1, 3));
        return V;
    }

    void SetCurrentState(Gamestate state)
    {
        currentState = state;
        lastStateChange = Time.time;
    }
    float GetStateElapsed()
    {
        return Time.time - lastStateChange;
    }


}

