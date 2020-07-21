using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DesEdgeRoot : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject Pivot;
    //public GameObject checkBar;
    //public GameObject lineObj;
    //public List<Vector3> vertexsPoint = new List<Vector3>();
    //float z;
    //public bool isMakeLine = false;
    //public bool isRotate = false;
    void Start()
    {
        Invoke("Desobj", 3.0f);
        //Invoke("RotateCheckBar", 0.5f);
    }
    public void Desobj()
    {
        Destroy(this.gameObject);
    }
   // private void Update()
   // {
        //if (z < 360 && isRotate == true)
       // {
          //  z += 1000 * Time.deltaTime;
         //   Pivot.transform.rotation = Quaternion.Euler(0, 0, z);
       // }
        //if (z >= 360)
        //{
           // checkBar.SetActive(false);
            //isMakeLine = true;
           // z = 0;
            //line.SetVertexCount(vertexsPoint.Count + 1);
            //line.loop = true;
            //line.useWorldSpace = false;//월드 좌표가 아니라 로컬좌표 기준으로 레이저가 나가도록한다
            //line.material = new Material(Shader.Find("Unlit/Color"));//쉐이더 설정 매개변수는 스트링으로 경로를 적어줌
            //line.material.color = Color.blue;
            //line.startWidth = 0.08f;//레이저 시작 두께
            //line.endWidth = 0.08f;
            //for (int i = 0; i < vertexsPoint.Count + 1; i++)
            //{

            //    //line.SetPosition(i + 1, vertexsPoint[i + 1]);
            //    if (i == vertexsPoint.Count)
            //    {
            //        line.SetPosition(i, vertexsPoint[0]);
            //    }
            //    else
            //    {
            //        line.SetPosition(i, vertexsPoint[i]);
            //    }
            //}
        //}
        
        //if(isMakeLine == true)
        //{
            //isMakeLine = false;
            //LineLong();
        //}
    //}
    //    public void RotateCheckBar()
    //    {
    //        isRotate = true;
    //        checkBar.SetActive(true);
    //    }
    //public void LineLong()
    //{
    //    for(int i=0; i<vertexsPoint.Count;i++)
    //    {
    //        if(i==vertexsPoint.Count-1)
    //        {
    //            float dis = (Vector3.Distance(vertexsPoint[0], vertexsPoint[i]));
    //            GameObject obj = Instantiate(lineObj, (vertexsPoint[0] + vertexsPoint[i])*0.5f, Quaternion.identity,transform);
    //            obj.transform.up = vertexsPoint[i]- vertexsPoint[0];
    //            obj.transform.localScale = new Vector3(obj.transform.localScale.x, dis/2, obj.transform.localScale.z);
    //        }
    //        else
    //        {
    //            float dis=(Vector3.Distance(vertexsPoint[i + 1], vertexsPoint[i]));
    //            GameObject obj = Instantiate(lineObj, (vertexsPoint[i + 1] + vertexsPoint[i])*0.5f, Quaternion.identity,transform);
    //            obj.transform.up = vertexsPoint[i]- vertexsPoint[i + 1];
    //            obj.transform.localScale = new Vector3(obj.transform.localScale.x, dis/2, obj.transform.localScale.z);
    //        }
    //    }
    //}

}

