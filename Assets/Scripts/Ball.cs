using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //public GameObject checkCenter;
    //public GameObject checkStart;
    //public GameObject checkEnd;

    GameObject[] gameObjects;
    //public GameObject Obj;
    //public GameObject end;
    //GameObject go;
    //GameObject go_;
    public int objnum; //0번큐브 1번 실린더 2번 원뿔 3번 정사면체 4번 정팔면체
    public GameObject leftSideRoot; //생성할 프리팹
    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 angle;
    public Vector3 conePoint;
    //Vector3 dir1;
    //Vector3 dir2;
    Vector3 totaldir;
    Vector3 dir1;
    Vector3 dir2;
    public Vector3 dir2_;
    public bool isCutStart = false;
    public bool isCutend = false;
    public Vector3 center;
    public float MoveSpeed = 0.3f;
    public float MoveTime = 0.5f;
    public bool isCutting = true;
    bool isrotate = false;
    float currMoveTime;
    bool cutFinish = false;
    GameObject obj;
    GameObject cameraPos;
    Transform centerEye;
    //percent= -0.5 < percent <0.5 의 값들중하나로 지정하면될듯함.
    //public float percent=0.0f;
    //dir값을 노말 벡터 형식으로 1,0,0 넣어주면 x축 기준으로 좌우로 나뉘어짐
    //dir값을 노말 벡터 형식으로 0,1,0 넣어주면 y축 기준으로 위아래로 나뉘어짐
    //dir값을 노말 벡터 형식으로 0,0,1 넣어주면 z축 기준으로 앞뒤로 나뉘어짐
    //public Vector3 dir; //월드 벡터 기준임

    void Start()
    {
        startPos = Vector3.zero;
        cameraPos = GameObject.Find("OVRCameraRig");
        centerEye = cameraPos.transform.GetChild(0).GetChild(1).transform;
        //centerEye = cameraPos.transform;
        if (objnum == 2)
        {
            transform.Rotate(new Vector3(-90, 0, 0), Space.World);
        }
        MoveTime = 3.0f;
    }

    void Update()
    {
       // centerEye = cameraPos.transform.GetChild(0).GetChild(1).transform;
        
        if (isCutStart == true && isCutend == true && isCutting == true)
        {
            isCutStart = false;
            isCutend = false;
            //Instantiate(Obj, startPos, Quaternion.identity);
            //Instantiate(end, endPos, Quaternion.identity);
            //startPos = transform.TransformDirection(startPos);
            center = (startPos + endPos) * 0.5f;
            //Instantiate(Obj, center, Quaternion.identity);
            //totaldir = endPos - startPos;
            //Resources.Load("CenterPos")
            //go = Instantiate(Resources.Load("CenterPos") as GameObject, totaldir, Quaternion.identity);
            //go_ = Instantiate(Resources.Load("CenterPos") as GameObject, totaldir, Quaternion.identity);
            //go.transform.forward = go.transform.TransformDirection(center - endPos);
            //.transform.Rotate(0, 0.01f, 0);

            MakeAgle();
            //Instantiate(checkStart,startPos,Quaternion.identity);
            //Instantiate(checkEnd,endPos,Quaternion.identity);
            //Instantiate(checkCenter,center,Quaternion.identity);
            MeshCollider ms = GetComponent<MeshCollider>();
            Destroy(ms);
            Cut();
        }
        if (cutFinish == true)
        {

            MoveObject();
        }
    }

    public void Cut()
    {
        //자를때 지금 잘리는 도형이 무슨 도형인지  Vectexs 스크립트에 알려줌
        Vectexs.Get.currentFigure = objnum;
        //잘리는 순간 원뿔이면 꼭지점과 바닥은 더이상 충돌 검출을 할필요가 없으니 비활성화한다.

        gameObjects = MeshCut.Cut(gameObject, center, angle,
         new Material(Shader.Find("Unlit/Color")));
        gameObjects[0].GetComponent<Ball>().isCutting = false;
        gameObjects[0].AddComponent<MeshCollider>().convex = true;
        //gameObjects[0].AddComponent<Ball>();
        //gameObjects[0].transform.position += -angle*1;
        //태그를 추가해준다 자르고나서
        gameObjects[0].tag = "Cutable";
        //gameObjects[0].GetComponent<MeshCollider>().convex = true;
        //gameObjects[0].AddComponent<Rigidbody>();
        gameObjects[1].AddComponent<MeshCollider>().convex = true;
        //gameObjects[1].AddComponent<Ball>();
        gameObjects[1].AddComponent<Rigidbody>();
        gameObjects[1].AddComponent<RemoveObj>();
        gameObjects[1].tag = "Cutable";
        //gameObjects[1].transform.position += angle*1;
        gameObjects[1].transform.localScale = gameObjects[0].transform.localScale;
        //gameObjects[1].GetComponent<MeshCollider>().convex = true;
        //gameObjects[1].AddComponent<Rigidbody>();
        cutFinish = true;
        isrotate = true;
    }
    public void MoveObject()
    {
        //잘린 오브젝트가 순간이동이 아니라 서서히 이동하도록
        //gameObjects[0].transform.position += -angle * MoveTime * Time.deltaTime * MoveSpeed;
        if (isrotate == true)
        {
            isrotate = false;
            obj = Instantiate(leftSideRoot, gameObjects[0].transform.position, Quaternion.identity);//오브젝트 생성
            obj.transform.rotation = Quaternion.LookRotation(angle);
            Debug.Log("빈게임 오브젝트를 법선벡터방향으로 회전");
        }
        RotateLeftSide(obj);
        gameObjects[1].transform.position += angle * MoveTime * Time.deltaTime * MoveSpeed;
        //iTween.MoveTo(gameObjects[1],iTween.Hash("y",10.0f,"time",3.0f)); //절대좌표 이동
        //iTween.MoveBy(gameObjects[1],iTween.Hash("y",10.0f,"time",2.0f,"delay",3.0f)); //시ㅏ대 좌표 이동
        //iTween.MoveFrom(gameObjects[1],iTween.Hash("y",200.0f,"time",2.0f,"dealay",5.0f)); //y축의 200위치에서 내위치로
        currMoveTime += Time.deltaTime; //시간의 흐흠을 체크한다.
        if (currMoveTime > MoveTime)
        {
            gameObjects[0].SetActive(false);
            Vectexs.Get.throwObj.RemoveAt(0);//첫번째꺼를 지운다

            //1초이상이 되면?(1초 정도만 오브젝트가 움직이게 해서 서로 떨어 뜨려 놓는다)
            cutFinish = false;//다시 원래 상태로 돌린다 
            if (cutFinish == false)
            {
                Invoke("ResetObj", 1.0f);
            }
        }
    }

    public void MakeAgle()
    {
        // dir1 = center-startPos;
        // dir2_ = -(center-startPos/Mathf.Tan(45.0f)-center);
        // Debug.Log(dir2_);
        // dir2 = dir2_-center;
        // angle = Vector3.Cross(dir1,dir2);
        // Debug.Log(angle);
        dir1 = endPos - startPos;
        //dir2_ = go_.transform.forward - center;
        dir2 = dir2_; //+ go.transform.right * 0.1f;
        angle = Vector3.Cross(dir1, dir2);
        Debug.Log(angle);
    }

    public void ResetObj()
    {
        if (objnum == 0)
        {
            Vectexs.Get.ismakeCube = true;
        }
        else if (objnum == 1)
        {
            Vectexs.Get.ismakeCylinder = true;
        }
        else if (objnum == 2)
        {
            Vectexs.Get.ismakeCone = true;
        }
        else if (objnum == 3)
        {
            Vectexs.Get.ismakeTetrahedron = true;
        }
        else if (objnum == 4)
        {
            Vectexs.Get.ismakeOctahedron = true;
        }
    }
    //gameObjects[0] 잘린 오브젝트의 원본을 회전시키기
    public void RotateLeftSide(GameObject obj)
    {
        gameObjects[0].transform.SetParent(obj.transform);//새로만든 오브젝트에 자식으로 넣기
                                                          //centerEye

        obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, Quaternion.LookRotation(-centerEye.transform.forward), Time.deltaTime * 4.0f);
    }
}