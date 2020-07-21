using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public GameObject subblade;
    public GameObject cutEffect;
    public GameObject scanner;
    Vector3 stayPos;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cutable") == true)
        {
            stayPos = other.ClosestPoint(transform.position);
            Instantiate(cutEffect, stayPos, Quaternion.identity);
            OVRInput.SetControllerVibration(0.1f, 0.9f, OVRInput.Controller.RTouch);
        }
    }
    private void OnTriggerEnter(Collider other)
    {

            if (other.CompareTag("Cutable") == true)
            {
                gameObject.GetComponent<AudioSource>().enabled = true;
                other.GetComponent<Ball>().isCutStart = true;//자르는것 시작
                if (other.GetComponent<Ball>().objnum == 0)
                {
                    other.GetComponent<Ball>().startPos = other.ClosestPoint(transform.position);
                }
                else if (other.GetComponent<Ball>().objnum == 1)
                {
                    other.GetComponent<Ball>().startPos = other.ClosestPoint(transform.position);
                }
                else if (other.GetComponent<Ball>().objnum == 2)
                {
                    if (Vectexs.Get.isconePoint == false && Vectexs.Get.isconePlat == false) //원뿔 자르는 경우
                    {
                        Debug.Log("옆면부터 자르기 시작");
                        //Vectexs.Get.msg = "옆면부터 자르기 시작";
                        //other.GetComponent<Ball>().isCutStart = true;
                        Vectexs.Get.isconSideStart = true; //옆면부터 자르기 시작함
                                                           //other.GetComponent<Ball>().startPos = other.ClosestPointOnBounds(transform.position);
                        other.GetComponent<Ball>().startPos = other.ClosestPoint(transform.position);
                    }
                    //else
                    //{
                    //    //그외의 닿은 점들은 그냥  값을 넣어준다.
                    //    other.GetComponent<Ball>().startPos = other.ClosestPoint(transform.position);
                    //}
                }
                //(other.GetComponent<Ball>().objnum==3 || other.GetComponent<Ball>().objnum == 4)
                else
                {
                    //정사면체나 정팔면체를 자르는 경우
                    if (Vectexs.Get.isStartPoint == false)
                    {
                        //도형을 자르기 시작해서 트리거가 발동됬는데 시작점으로 꼭지점이 아니다?
                        //->옆면부터 자르기 시작햇으므로 그값을 시작값으로 넣어준다
                        other.GetComponent<Ball>().startPos = other.ClosestPoint(transform.position);
                        //Vectexs.Get.msg = "꼭지점 말고 사이드의 값이 들어감";
                    }
                }

            }

            if (other.CompareTag("ConePoint") == true)
            {
                gameObject.GetComponent<AudioSource>().enabled = true;
                Debug.Log("꼭지점을 지남");
                //Vectexs.Get.msg = "꼭지점을 지남";
                Vectexs.Get.isconePoint = true; //원뿔의 꼭지점을 지났다는 표시
                                                //칼날이 원뿔의 꼭지점을 건드리면 enter 혹은 exit로
                                                //꼭지점을 건드렸으면 꼭지점의 부모오브젝트의 ball스크립트로 접근해서 시작값이 정해져 있는지 체크하고 그에 따라 값을 너헝준다
                if (other.transform.parent.GetComponent<Ball>().startPos == Vector3.zero) //시작점이 이미 비어있으면 ? 
                {
                    //Vectexs.Get.msg = "startPos가 꼭지점이됨"; 
                    other.transform.parent.GetComponent<Ball>().startPos = other.transform.position;
                }
                else //시작점이 체크되었다면endPos에 넣는다
                {
                    //Vectexs.Get.msg = "endPos가 꼭지점이됨";
                    other.transform.parent.GetComponent<Ball>().endPos = other.transform.position;
                }
            }
            if (other.CompareTag("ConePlat") == true)
            {
                gameObject.GetComponent<AudioSource>().enabled = true;
            Debug.Log("바닥을 지남");

                //원뿔의 바닥을 건드린경우
                //바닥오브젝트의 부모 오브젝트의 ball 스크립트로 가서 시작점이 비었는지 안비었는지 체크해준다
                Vectexs.Get.isconePlat = true;
                if (other.transform.parent.GetComponent<Ball>().startPos == Vector3.zero) //시작점이 비어잇으면 ?
                {
                    //other.GetComponent<Ball>().startPos = other.transform.GetChild(1).transform.position;
                    //바닥을 지나면 바닥판정 오브젝트의 부모오브젝트의 ball에 접근해서 값을 넣어준다
                    other.transform.parent.GetComponent<Ball>().startPos = other.ClosestPoint(transform.position);
                    //Vectexs.Get.msg = "바닥부터 자르기 시작";
                }
                else if (other.transform.parent.GetComponent<Ball>().endPos == Vector3.zero)//시작점이 체크되있다면 바닥점을 endPos로 넣어준다
                {
                    //other.GetComponent<Ball>().endPos = other.transform.GetChild(1).transform.position;
                    other.transform.parent.GetComponent<Ball>().endPos = other.ClosestPoint(transform.position);
                    //Vectexs.Get.msg = "끝점으로 바닥을 나옴";
                }
            }
            if (other.CompareTag("Top") == true)//실린더 윗면을 건드렸을때
            {
            gameObject.GetComponent<AudioSource>().enabled = true;
            Vectexs.Get.isTop = true;
            Debug.Log("실린더 윗면 건드림");
                if (other.transform.parent.GetComponent<Ball>().startPos == Vector3.zero)
                {
                    other.transform.parent.GetComponent<Ball>().startPos = other.transform.position;
                }
                else
                {
                    other.transform.parent.GetComponent<Ball>().endPos = other.transform.position;
                }

            }
            if (other.CompareTag("Botum") == true)//실린더 아랫면을 건드렸을때
            {
            gameObject.GetComponent<AudioSource>().enabled = true;
            Vectexs.Get.isBotum = true;
            Debug.Log("실린더 아래면 건드림");
            if (other.transform.parent.GetComponent<Ball>().startPos == Vector3.zero)
                {
                    other.transform.parent.GetComponent<Ball>().startPos = other.transform.position;
                }
                else
                {
                    other.transform.parent.GetComponent<Ball>().endPos = other.transform.position;
                }

            }
            //정사면체 꼭지점을 지난경우
            if (other.CompareTag("Point") == true && other.GetComponent<Ball>().objnum == 3)
            {
            gameObject.GetComponent<AudioSource>().enabled = true;
                if (other.transform.parent.GetComponent<Ball>().startPos == Vector3.zero)
                {
                    //시작점이 비었는데 꼭지점을 지난경우
                    other.transform.parent.GetComponent<Ball>().startPos = other.transform.position;
                    Vectexs.Get.isStartPoint = true;
                }
                else//시작점이 있는데 꼭지점을 지난경우에는 시작점이 꼭지점이 아닌경우에만 끝값으로 꼭지점을 넣는다
                {
                    if (Vectexs.Get.isStartPoint == false)
                    {
                        other.transform.parent.GetComponent<Ball>().endPos = other.transform.position;
                        Vectexs.Get.isEndPoint = true;
                    }
                }
            }
            // 정팔면체에서 꼭지점을 지났을 경우
            //if (other.CompareTag("Point") == true && other.GetComponent<Ball>().objnum == 4)
            //{
            //    gameObject.GetComponent<AudioSource>().enabled = true;
            //    if (other.transform.parent.GetComponent<Ball>().startPos == Vector3.zero)
            //    {
            //        //시작점이 아직 안정해졌는데 꼭지점을 건드렸다?? ->그 꼭지점을 시작점으로
            //        other.transform.parent.GetComponent<Ball>().startPos = other.transform.position;
            //        Vectexs.Get.isStartPoint = true;
            //        //Vectexs.Get.msg = "시작점으로 꼭지점을 지남";
            //    }
            //    else
            //    {
            //        //시작점은 정해져 있는 상태로 꼭지점을 건드렸다? -> 그 꼭지점을 끝점으로
            //        other.transform.parent.GetComponent<Ball>().endPos = other.transform.position;
            //        Vectexs.Get.isEndPoint = true;
            //        //Vectexs.Get.msg = "끝점으로 꼭지점을 지남";
            //    }

            if (other.GetComponent<Ball>().objnum == 4)
            {
                if (other.CompareTag("Point1") == true && other.transform.parent.GetComponent<Ball>().startPos == Vector3.zero)
                {
                    other.transform.parent.GetComponent<Ball>().startPos = other.transform.position;
                    //포인트 1에 값이 시작값으로 들어가면 엔드값으로 2,3,4,5는 들어가면 안된다 마주보는 6만 엔드값으로 들어갈수 있음
                    Vectexs.Get.point6 = true;
                }
                else if (other.CompareTag("Point2") == true && other.transform.parent.GetComponent<Ball>().startPos == Vector3.zero)
                {
                    other.transform.parent.GetComponent<Ball>().startPos = other.transform.position;
                    Vectexs.Get.point4 = true;
                }
                else if (other.CompareTag("Point3") == true && other.transform.parent.GetComponent<Ball>().startPos == Vector3.zero)
                {
                    other.transform.parent.GetComponent<Ball>().startPos = other.transform.position;
                    Vectexs.Get.point5 = true;
                }
                else if (other.CompareTag("Point4") == true && other.transform.parent.GetComponent<Ball>().startPos == Vector3.zero)
                {
                    other.transform.parent.GetComponent<Ball>().startPos = other.transform.position;
                    Vectexs.Get.point2 = true;
                }
                else if (other.CompareTag("Point5") == true && other.transform.parent.GetComponent<Ball>().startPos == Vector3.zero)
                {
                    other.transform.parent.GetComponent<Ball>().startPos = other.transform.position;
                    Vectexs.Get.point3 = true;
                }
                else if (other.CompareTag("Point6") == true && other.transform.parent.GetComponent<Ball>().startPos == Vector3.zero)
                {
                    other.transform.parent.GetComponent<Ball>().startPos = other.transform.position;
                    Vectexs.Get.point1 = true;
                }
                else
                {

                }


                if (other.CompareTag("Point1") && other.transform.parent.GetComponent<Ball>().startPos != Vector3.zero)
                {
                    if (Vectexs.Get.point1 == true)
                    {
                        other.transform.parent.GetComponent<Ball>().endPos = other.transform.position;
                    }

                }
                else if (other.CompareTag("Point2") && other.transform.parent.GetComponent<Ball>().startPos != Vector3.zero)
                {
                    if (Vectexs.Get.point2 == true)
                        other.transform.parent.GetComponent<Ball>().endPos = other.transform.position;
                }
                else if (other.CompareTag("Point3") && other.transform.parent.GetComponent<Ball>().startPos != Vector3.zero)
                {
                    if (Vectexs.Get.point3 == true)
                        other.transform.parent.GetComponent<Ball>().endPos = other.transform.position;
                }
                else if (other.CompareTag("Point4") && other.transform.parent.GetComponent<Ball>().startPos != Vector3.zero)
                {
                    if (Vectexs.Get.point4 == true)
                        other.transform.parent.GetComponent<Ball>().endPos = other.transform.position;
                }
                else if (other.CompareTag("Point5") && other.transform.parent.GetComponent<Ball>().startPos != Vector3.zero)
                {
                    if (Vectexs.Get.point5 == true)
                        other.transform.parent.GetComponent<Ball>().endPos = other.transform.position;
                }
                else if (other.CompareTag("Point6") && other.transform.parent.GetComponent<Ball>().startPos != Vector3.zero)
                {
                    if (Vectexs.Get.point6 == true)
                        other.transform.parent.GetComponent<Ball>().endPos = other.transform.position;
                }
                else
                {

                }
            }
        
        
    }
    private void OnTriggerExit(Collider other)
    {

            if (other.CompareTag("Cutable") == true)
            {            
            gameObject.GetComponent<AudioSource>().enabled = false;
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);

            //other.GetComponent<Ball>().endPos = other.ClosestPointOnBounds(transform.position);
            //오브젝트를 칼이 빠져나왔을때 endPos가 안정해졌으면 나온곳을 끝점으로 해준다.
            //
                if (other.GetComponent<Ball>().endPos == Vector3.zero)
                {
                    other.GetComponent<Ball>().endPos = other.ClosestPoint(transform.position);
                    //Vectexs.Get.msg = "옆면으로 나옴";
                }
                other.GetComponent<Ball>().isCutend = true;
                other.GetComponent<Ball>().dir2_ = transform.up;
                //other.GetComponent<Ball>().angle = subblade.transform.right;
            }
            if (other.CompareTag("Point") == true)//마지막으로 꼭지점을 빠져나가면 
            {
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
            gameObject.GetComponent<AudioSource>().enabled = false;
            other.transform.parent.GetComponent<Ball>().endPos = other.transform.position;//endPos로 꼭지점넣어주기
            }
        

    }


}
