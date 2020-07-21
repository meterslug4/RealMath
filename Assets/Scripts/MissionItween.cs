using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionItween : MonoBehaviour
{

    public GameObject mission;
    float currntTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("MissionMove", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MissionMove()
    {
        Hashtable ht = new Hashtable();
        ht.Add("x", -7.410468f);
        ht.Add("y", 3.5f);
        ht.Add("z", 15.74393f);
        ht.Add("time", 1.0f);
        ht.Add("easetype", iTween.EaseType.easeInOutCubic);
        iTween.MoveTo(mission, ht);
        iTween.RotateTo(mission, iTween.Hash("rotation", new Vector3(0, -45, 0), "time", 1.0f, "easetype", iTween.EaseType.easeInOutCubic));
    }
}
