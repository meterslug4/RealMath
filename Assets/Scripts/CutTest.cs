using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTest : MonoBehaviour
{
    public GameObject Reset;
    public Transform ResetPos;
    GameObject testObj;
    bool iscut;
    private void Update()
    {
        if(iscut == true)
        {
            iscut = false;
            testObj = Instantiate(Reset, ResetPos.transform.position, Quaternion.identity);
        }
    }

}
