using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesEdgeRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Desobj", 3.0f);
    }
 public void Desobj()
    {
        Destroy(this.gameObject);
    }

}
