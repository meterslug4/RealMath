using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailManager : MonoBehaviour
{
    private void OnEnable()
    {
        
            Vector3 amount = new Vector3(0.1f, 0.1f, 0.1f);
            iTween.ShakePosition(gameObject, amount, 1.0f);
    }
}

