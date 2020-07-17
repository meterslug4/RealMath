using GlowingSwords.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeBarControl : MonoBehaviour
{
    public float playTimeCurrent = 10.0f;
    public float playTimeMax = 10.0f;

    public Image TimeBar;
    public GameObject dieEffect;
    public GameObject robot;
    public GameObject success;
    public GameObject fail;

    public float currenttime;

    public GameObject startUI;
    public GameObject wall;
    public GameObject vertex;
    public GameObject timeRobot;

    float time;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        SkinnedMeshRenderer render = robot.GetComponent<SkinnedMeshRenderer>();
        Material mat = render.material;
        time += Time.deltaTime * 0.2f;
        Color goRed = Color.Lerp(Color.blue, Color.red, time);
        mat.SetColor("_Color", goRed);

        playTimeCurrent -= 1 * Time.deltaTime;

        TimeBar.fillAmount = playTimeCurrent / playTimeMax;

        if (playTimeCurrent < 5 && playTimeCurrent > 0)
        {
            gameObject.GetComponent<Animator>().SetBool("TakeDamage", true);
        }
        else if (playTimeCurrent < 0)
        {

            playTimeCurrent = 0;
            dieEffect.SetActive(true);
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            fail.SetActive(true);
            Invoke("ChangeStartScene", 3.0f);
        }
    }

    public void ChangeStartScene()
    {
        SceneManager.LoadScene("01.StartScene");
    }

    /*public void MissionSuccess()
    {
        currenttime += Time.deltaTime;
        success.SetActive(true);
        if(playTimeCurrent < 7)
        {
           playTimeCurrent += 3;

        }
        else
        {
            playTimeCurrent = playTimeMax;
        }

        if (currenttime > 1.0f)
        {
            success.SetActive(false);
            currenttime = 0;
        }
    }

    public void MissionFail()
    {
        currenttime += Time.deltaTime;
        fail.SetActive(true);
        if (currenttime > 3.0f)
        {
            fail.SetActive(false);
            startUI.SetActive(true);
            wall.SetActive(false);
            vertex.SetActive(false);
            timeRobot.SetActive(false);
        }

    }*/

}
