using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharMov : MonoBehaviour
{
    [Header("References")]
    public LibPdInstance pdPatch;
    public Rigidbody rb;
    public Gamemanager gamemanager;
    public GameObject spawn;

    [Header("Parameters")]
    bool healing = false;

    [Header("CharacterMov")]
    float speed = 5f;
    public float rotD;
    public Camera cam;
    Vector3 MovD;
    public LayerMask ground;

    [Header("Buffs")]
    public Image rechargeImage;
    public bool rechargeBuff;
    public Image shieldImage;
    public bool shieldBuff;
    public Image energyImage;
    public bool energyBuff;

    [Header("Buffs&drops")]
    public int dropRate = 1;
    public GameObject points;
    public GameObject[] buffs;

    void Start()
    {
        rechargeImage.fillAmount= 0f;
        rechargeBuff = false;
        shieldImage.fillAmount = 0f;
        shieldBuff = false;
        energyImage.fillAmount = 0f;
        energyBuff = false;

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // character speed regulation
        if (Input.GetMouseButtonDown(0))
        {
            speed = 8.2f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            speed = 5.0f;
        }

        if (Input.GetMouseButtonDown(1))
        {
            speed = 2.8f;
        }
        if (Input.GetMouseButtonUp(1))
        {
            speed = 5.0f;
        }

        // constant forward drive
        transform.Translate(0, 0, Time.deltaTime * speed);

        // steering character using the mouse
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, ground))
        {
            MovD = hit.point;
        }
        Vector3 dir = MovD - transform.position;
        Quaternion LookRot = Quaternion.LookRotation(dir);
        Vector3 rot = Quaternion.Lerp(transform.rotation, LookRot, Time.deltaTime * rotD).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rot.y, 0f);

        // loose points over time when not near recharge unit
        if (healing == false && gamemanager.statistic >= 0)
        {
            gamemanager.statistic = gamemanager.statistic - 0.1f * Time.deltaTime;
        }
        healing = false;

        if (rechargeBuff == true)
        {
            rechargeImage.fillAmount -= 1.0f / 15 * Time.deltaTime;
            if (rechargeImage.fillAmount == 0)
            {
                rechargeBuff = false;
            }
        }
        if (shieldBuff == true)
        {
            shieldImage.fillAmount -= 1.0f / 15 * Time.deltaTime;
            if (shieldImage.fillAmount == 0)
            {
                shieldBuff = false;
            }
        }
        if (energyBuff == true)
        {
            energyImage.fillAmount -= 1.0f / 15 * Time.deltaTime;
            if (energyImage.fillAmount == 0)
            {
                energyBuff = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            Gethitbyflock();
            gamemanager.statistic = gamemanager.statistic - 2;
            StartCoroutine(cam.GetComponent<CameraMov>().CameraShake(.15f, 4f));
        }
        if (other.tag == "enemyboss")
        {
            gamemanager.statistic = gamemanager.statistic - 5;
            Gethitbyboss();
            StartCoroutine(cam.GetComponent<CameraMov>().CameraShake(.15f, 4f));
        }
        if (other.tag == "enemyneutral")
        {
            if (Random.Range(0, dropRate) < 1)
            {
                Buffdrop();
            }
            Hitflock();
        }
        if (other.tag == "rechargebuff")
        {
            rechargeBuff = true;
            rechargeImage.fillAmount = 15;
            Destroy(other.gameObject);
        }
        if (other.tag == "shield")
        {
            shieldBuff = true;
            shieldImage.fillAmount = 15;
            Destroy(other.gameObject);
        }
        if (other.tag == "energybuff")
        {
            energyBuff = true;
            energyImage.fillAmount = 15;
            Destroy(other.gameObject);
        }
        if (other.tag == "+1")
        {
            gamemanager.statistic = gamemanager.statistic + 1;
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "healing")
        {
            healing = true;
            // if (rechargeBuff.enabled == true)
            if (rechargeBuff == true)
            {
                gamemanager.statistic = gamemanager.statistic + 0.8f * Time.deltaTime;
            }
            else
                gamemanager.statistic = gamemanager.statistic + 0.2f * Time.deltaTime;
        }
        if (other.tag == "slowdebuff")
        {
            speed = 3f;
        }
        if (other.tag == "damagedebuff")
        {
            gamemanager.statistic = gamemanager.statistic - 0.8f * Time.deltaTime;
        }
    }

    void Buffdrop()
    {
        Vector3 randomPos;
        randomPos = new Vector3(0, 2, 0);

        int random;
        random = Random.Range(0, 100);
        if (random <= 1)
        {
            for (var i = 0; i < Random.Range(7, 9); i++)
            {
                Instantiate(points, this.transform.position + randomPos, Quaternion.identity);
            }
            Instantiate(buffs[Random.Range(0, buffs.Length)], this.transform.position + randomPos, Quaternion.identity);
            return;
        }
        if (random <= 10)
        {
            for (var i = 0; i < Random.Range(5, 7); i++)
            {
                Instantiate(points, this.transform.position + randomPos, Quaternion.identity);
            }
            Instantiate(buffs[Random.Range(0, buffs.Length)], this.transform.position + randomPos, Quaternion.identity);
            return;
        }
        else if (random <= 25)
        {
            for (var i = 0; i < Random.Range(3, 5); i++)
            {
                Instantiate(points, this.transform.position + randomPos, Quaternion.identity);
            }
            Instantiate(buffs[Random.Range(0, buffs.Length)], this.transform.position + randomPos, Quaternion.identity);
            return;
        }
        else if (random <= 100)
        {
            for (var i = 0; i < Random.Range(1, 3); i++)
            {
                Instantiate(points, this.transform.position + randomPos, Quaternion.identity);
            }
        }
    }

    /////////////////////////////////////////////////////////////////// AUDIO

    public void Gethitbyflock()
    {
      // pdPatch.SendBang("VolumeUp");
    } 
    public void Gethitbyboss()
    {
        // pdPatch.SendBang("Gethitbyboss");
    }
    public void Hitflock()
    {
        //pdPatch.SendBang("Hitflock");
        
    }
}
