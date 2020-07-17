using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObj : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Map"))
        {
            MissionManager.Get.isTimeFlow = false;
            this.gameObject.SetActive(false);
        }
    }
}
