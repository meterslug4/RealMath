using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LastScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Text>().text = MissionManager.Get.nowScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
