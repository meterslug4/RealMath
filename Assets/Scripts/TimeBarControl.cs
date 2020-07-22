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

    public float currenttime;



    float time;
    public float timeSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        //SkinnedMeshRenderer render = robot.GetComponent<SkinnedMeshRenderer>();
        //Material mat = render.material;
        //time += Time.deltaTime * 0.2f;
        //Color goRed = Color.Lerp(Color.blue, Color.red, time);
        //mat.SetColor("_Color", goRed);
        if(MissionManager.Get.isTimeFlow == true)
        {
            TimeCount(timeSpeed);
        }

    }

    public void TimeCount(float num)
    {
        playTimeCurrent -= num * Time.deltaTime;

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
            Invoke("ChangeEndScene", 2.0f);
        }
    }
    public void ChangeEndScene()
    {
        SceneManager.LoadScene(4);
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
