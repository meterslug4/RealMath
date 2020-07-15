using GlowingSwords.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBarControl : MonoBehaviour
{
    public float playTimeCurrent = 10.0f;
    public float playTimeMax = 10.0f;

    public Image TimeBar;
    public GameObject dieEffect;
    public GameObject robot;

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
            Instantiate(dieEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void MissionSuccess()
    {
        if(playTimeCurrent < 7)
        {
           playTimeCurrent += 3;

        }
        else
        {
            playTimeCurrent = playTimeMax;
        }
    }

}
