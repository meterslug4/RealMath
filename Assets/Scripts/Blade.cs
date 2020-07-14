using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public GameObject subblade;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cutable") == true)
        {
            OVRInput.SetControllerVibration(0.1f, 0.9f, OVRInput.Controller.RTouch);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Cutable")==true)
        {
            other.GetComponent<Ball>().isCutStart = true;//자르는것 시작
            if (Vectexs.Get.isconePoint == false&& Vectexs.Get.isconePlat == false && other.GetComponent<Ball>().objnum==2) //원뿔 자르는 경우
            {
                Debug.Log("옆면부터 자르기 시작");
                //other.GetComponent<Ball>().isCutStart = true;
                Vectexs.Get.isconSideStart = true; //옆면부터 자르기 시작함
                //other.GetComponent<Ball>().startPos = other.ClosestPointOnBounds(transform.position);
                other.GetComponent<Ball>().startPos = other.ClosestPoint(transform.position);
            }
            else
            {
                //그외의 닿은 점들은 그냥  값을 넣어준다.
                other.GetComponent<Ball>().startPos = other.ClosestPoint(transform.position);
            }
        }
        
        if(other.CompareTag("ConePoint")==true)
        {
            Debug.Log("꼭지점을 지남");
            Vectexs.Get.isconePoint = true; //원뿔의 꼭지점을 지났다는 표시
            //칼날이 원뿔의 꼭지점을 건드리면 enter 혹은 exit로
            if(other.GetComponent<Ball>().startPos == Vector3.zero) //시작점이 이미 체크되잇으면 ? endpos로
            {
                other.GetComponent<Ball>().startPos = other.transform.GetChild(0).transform.position;
            }
            else //시작점이 체크안되있다면 startPos에 넣는다
            {
                other.GetComponent<Ball>().endPos = other.transform.GetChild(0).transform.position;
            }
        }
        if(other.CompareTag("ConePlat")==true)
        {
            Debug.Log("바닥을 지남");
            //원뿔의 바닥을 건드린경우
            Vectexs.Get.isconePlat = true;
            if (other.GetComponent<Ball>().startPos == Vector3.zero) //시작점이 이미 체크되잇으면 ? endpos로
            {
                other.GetComponent<Ball>().startPos = other.transform.GetChild(1).transform.position;
            }
            else //시작점이 체크안되있다면 startPos에 넣는다
            {
                other.GetComponent<Ball>().endPos = other.transform.GetChild(1).transform.position;
            }
        }
        if(other.CompareTag("Top")==true)//실린더 윗면을 건드렸을때
        {
            Vectexs.Get.isTop = true;
        }
        if(other.CompareTag("Botum")==true)//실린더 아랫면을 건드렸을때
        {
            Vectexs.Get.isBotum = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Cutable")==true)
        {
            other.GetComponent<Ball>().isCutend = true;
            //other.GetComponent<Ball>().endPos = other.ClosestPointOnBounds(transform.position);
            if(Vectexs.Get.isconePoint==false&&Vectexs.Get.isconePlat==false) //꼭지점과 바닥을 안통과하고 나갔을때 옆면으로 나갓을때
            {
                other.GetComponent<Ball>().endPos = other.ClosestPoint(transform.position);
            }
            else 
            { 
            }
            other.GetComponent<Ball>().dir2_ = transform.up;
            //other.GetComponent<Ball>().angle = subblade.transform.right;
        }
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
    }


}
