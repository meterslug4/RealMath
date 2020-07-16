using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Text Text;
    public Text Text2;
    public Text score;
    public GameObject mission;
    public GameObject success;
    public GameObject cubeMap;

    float crrentTime;
    // Start is called before the first frame update
    void Start()
    {
        // missionText = GetComponent<Text>();
        mission.SetActive(false);
        score.text = "0";
        success.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        crrentTime += Time.deltaTime;
        if(crrentTime >= 1)
        {
            
            mission.SetActive(true);
            Text.text = "삼각형을";
            Text2.text = "만드시오";

            if(crrentTime >= 4)
            {
                mission.transform.position= new Vector3(12,15,44);
                score.text = "100";
                if (crrentTime >= 5)
                {
                    score.text = "500";
                    if(crrentTime > 8)
                    {
                        SkinnedMeshRenderer render = cubeMap.GetComponent<SkinnedMeshRenderer>();
                        Material mat = render.material;  
                        Color goRed = Color.Lerp(Color.blue, Color.yellow,2);
                        mat.SetColor("MapColor", goRed);
                    }
                }

            }
        }

        if(score.text == "500")
        {
            success.SetActive(true);
        }
        
    }
}
