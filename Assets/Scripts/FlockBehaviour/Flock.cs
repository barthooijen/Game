using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{

    [Header("flock size")]
    public GameObject spawn;
    public GameObject unit;
    public int randomMin;
    public int randomMax;
    int numUnit = 1;
    public List<GameObject> allUnits;
    public int flockSize;
    public int flockHeight;

    [Header("flock movement")]
    public GameObject character;
    public Vector3 randomPos;
    public Vector3 goalPos;

    private void Start()
    {
        character = GameObject.Find("Character");
        transform.parent = GameObject.Find("All flocks").transform;
        spawn = GameObject.Find("Spawn");
        transform.position = GameObject.Find("Spawn").transform.position + randomPos;

        int randomNum;
        randomNum = Random.Range(randomMin, randomMax);
        numUnit = numUnit * randomNum;
        StartCoroutine(spawnUnits());
    }

    void Update()
    {
        if (Random.Range(0, 10000) < 50)
        {
            randomPos = new Vector3(Random.Range(-flockSize, flockSize),
                                    Random.Range(0, flockHeight),
                                    Random.Range(-flockSize, flockSize));
            goalPos = this.transform.position + randomPos;
        }
    }

    private IEnumerator spawnUnits()
    {
        float Countdown = Random.Range(0.1f, 3.0f);
        for (int i = 0; i < numUnit; i++)
        {
            Vector3 randomSpawn;
            randomSpawn = new Vector3(Random.insideUnitSphere.x + 30, transform.position.y, Random.insideUnitSphere.z + 30);
            yield return new WaitForSeconds(Countdown);
            allUnits.Add((GameObject)Instantiate(unit, Quaternion.Euler(0, Random.Range(0, 360), 0) * randomSpawn + spawn.transform.position, Quaternion.identity));
        }
    }
}
