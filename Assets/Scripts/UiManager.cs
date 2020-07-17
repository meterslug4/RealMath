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
            Text2.text = "만드시오"
;
            if(crrentTime >= 4)
            {
                //mission.transform.position= new Vector3(12,15,44);
                iTween.MoveTo(mission, iTween.Hash("x", 12, "y", 15, "z", 44, "easetype", iTween.EaseType.easeInQuad, "time", 0.5f));
                score.text = "100";
                if (crrentTime >= 6)
                {
                    iTween.MoveTo(mission, iTween.Hash("x", 0, "y", 3, "z", 12, "easetype", iTween.EaseType.easeInQuad, "time", 0.5f));
                    Text.text = "사각형을";
                    Text2.text = "만드시오";
                    if(crrentTime > 8)
                    {
                        score.text = "300";
                        iTween.MoveTo(mission, iTween.Hash("x", 12, "y", 15, "z", 44, "easetype", iTween.EaseType.easeInQuad, "time", 0.5f));
                        if (crrentTime > 10)
                        {
                            score.text = "500";
                            
                        }
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
