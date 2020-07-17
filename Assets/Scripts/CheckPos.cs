using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPos : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cutable") == true)
        {
            MissionManager.Get.isTimeFlow = true;
        }
    }
}
