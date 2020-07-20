using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        iTween.ShakePosition(gameObject, Vector3.one, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
