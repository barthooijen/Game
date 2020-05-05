using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    [Header("point display")]
    public float statistic= 0;
    public Text statisticDisplay;
    float maxStatistics = 100;
    public Image progressBar;

    [Header("gameobjects")]
    public GameObject spawn;
    public GameObject character;
    public GameObject allFlocks;
    public GameObject particles;

    [Header("randomization")]
    int randomNum;
    Vector3 randomPos;

    [Header("flocks")]
    public GameObject RechargeFlock;
    public GameObject NeutralFlock;
    public GameObject CombatFlock;
    public GameObject Boss1;

    [Header("flockcheck")]
    bool rechargeFlock = false;
    bool neutralFlock = false;
    bool combatFlock = false;
    bool boss1 = false;

    enum GameState { state00, state01, state02, state03, state04 }
    GameState currentState;
    float lastStateChange = 0.0f;

    void Start()
    {
        SetCurrentState(GameState.state00);
        progressBar.fillAmount = 0f;
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.state00:
                particles.SetActive(true);
                rechargeFlock = false;
                neutralFlock = false;
                combatFlock = false;

                if (statistic >= 90 && GetStateElapsed() > 5f && boss1 == false)
                {
                    SetCurrentState(GameState.state04);
                }
                else
                if ( GetStateElapsed()  > 5f && boss1 == false)

                    SetCurrentState (GameState.state01);
                break;

            case GameState.state01:
                if (rechargeFlock == false)
                {
                    Instantiate (RechargeFlock, spawn.transform.position, Quaternion.identity);
                    rechargeFlock = true;                                        
                }
                if (statistic >= 2 && GetStateElapsed() > 10f) SetCurrentState(GameState.state02);
                break;

            case GameState.state02:
                if (neutralFlock == false)
                {
                    Instantiate(NeutralFlock, spawn.transform.position, Quaternion.identity);
                    neutralFlock = true;                                
                }
                if (statistic >= 5 && GetStateElapsed() > 10f) SetCurrentState(GameState.state03);
                if (statistic <= 2 && GetStateElapsed() > 5f) SetCurrentState(GameState.state01);
                break;

            case GameState.state03:
                if (combatFlock == false)
                {
                    Instantiate(CombatFlock, spawn.transform.position, Quaternion.identity);
                    combatFlock = true;
                }
                if (statistic <= 5 && GetStateElapsed() > 5f) SetCurrentState(GameState.state02);
                break;

            case GameState.state04:
                if (boss1 == false)
                {
                    Vector3 randomSpawn;
                    randomSpawn = new Vector3(Random.insideUnitSphere.x + 30, spawn.transform.position.y, Random.insideUnitSphere.z + 30);
                    Instantiate(Boss1, Quaternion.Euler(0, Random.Range(0, 360), 0) * randomSpawn + spawn.transform.position, Quaternion.identity);
                    boss1 = true;
                }
               
                break;

        }

        if (Vector3.Distance(character.transform.position, spawn.transform.position) >= 80)
        {
            particles.SetActive(false);
            randomPos = new Vector3(Random.Range(-10, 10),
                                    Random.Range(0, 0),
                                    Random.Range(-10, 10));

            spawn.transform.position = character.transform.position + randomPos;

            SetCurrentState(GameState.state00);

            foreach (Transform child in allFlocks.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }



        /////////////////////////////////////////////////////////////GENERAL
        if (statistic <= 0)
        {
            statistic = 0;
        }
        statisticDisplay.text = statistic.ToString("F3");
        progressBar.fillAmount = statistic / maxStatistics;

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void SetCurrentState(GameState state)
    {
        currentState = state;
        lastStateChange = Time.time;
    }
    float GetStateElapsed()
    {
        return Time.time - lastStateChange;
    }
}
