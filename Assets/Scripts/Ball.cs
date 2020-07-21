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
    public GameObject edgeRoot;
    public GameObject edge;
    public GameObject edgeobj;//생성한 꼭지점의 부모 오브젝트가 될 게임 오브젝트
    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 angle;
    public Vector3 conePoint;
    //Vector3 dir1;
    //Vector3 dir2;
    //Vector3 totaldir;//칼의 전체 진행방향
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
    GameObject obj;//leftSideRoot 을 담을 변수
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
        gameObject.GetComponent<Ball>().enabled = false;
        startPos = Vector3.zero;
        endPos = Vector3.zero;
        cameraPos = GameObject.Find("OVRCameraRig");
        centerEye = cameraPos.transform.GetChild(0).GetChild(1).transform;
        //centerEye = cameraPos.transform;
        //if (objnum == 2)
        //{
        //    transform.Rotate(new Vector3(-90, 0, 0), Space.World);
        //}
        MoveTime = 3.0f;
    }

    void Update()
    {
        //꼭지점 그리기
        if (Vectexs.Get.edgeFind == true )
        {
            Vectexs.Get.edgeFind = false;
            
            FindEdge();
        }
        if (isCutStart == true && isCutend == true && isCutting == true)
        {
            
            isCutStart = false;
            isCutend = false;
            Vectexs.Get._startPos = startPos;//도형 판별을 위해 시작점을 넘겨준다
            Vectexs.Get._endPos = endPos;//도형 판정을 위해 끝점을 넘겨준다
            Vectexs.Get._totaldir = endPos - startPos;//칼이 자르고 지나간 전체 전인 진행 방향 포물선 판정 시에 필요하다
            center = (startPos + endPos) * 0.5f;
            MakeAgle();

            MeshCollider ms = GetComponent<MeshCollider>();
            Destroy(ms);
            Cut();
        }
        if (cutFinish == true)
        {
            MoveObject();
        }
        if(templistFull == true)
        {
            templistFull = false;
            //Debug.Log("꼭지점 위치시킴");
            EdgePosLocate();//꼭지점 배치시킴
        }
        if(templistLocate == true)
        {
            //Debug.Log("꼭지점 부모설정및 재배치");
            EdgePosLocate2();
           
        }
        #region 칼을 1.5초이상 대고 있으면 오브젝트 사라진다 관련 설정이나 변수 함수는 280번 라인부터
        if (dieflow == true)
        {
            dieTime += Time.deltaTime;
            if (dieTime > 1.5f)
            {
                dieflow = false;
                objdes = true;
            }
        }
        if(objdes == true)
        {
            objdes = false;
            
            for(int i=0; i< Vectexs.Get.throwObj.Count;i++)
            {
                if(Vectexs.Get.throwObj[i].GetComponent<Ball>().objnum == desindex)
                {
                    Vectexs.Get.throwObj.RemoveAt(i);
                }
            }
            gameObject.GetComponent<MeshCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            Invoke("ResetObj", 2.0f);
            Invoke("ObjActivefalse", 3.5f);
        }
        #endregion
    }

    public void Cut()
    {
        //자를때 지금 잘리는 도형이 무슨 도형인지  Vectexs 스크립트에 알려줌
        Vectexs.Get.currentFigure = objnum;
        //잘리는 순간 원뿔이면 꼭지점과 바닥은 더이상 충돌 검출을 할필요가 없으니 비활성화한다.

        gameObjects = MeshCut.Cut(gameObject, center, angle,
         new Material(Shader.Find("Unlit/Color")));
        gameObjects[0].GetComponent<Ball>().isCutting = false;
        //gameObjects[0].AddComponent<MeshCollider>().convex = true;
        
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

            //Debug.Log("빈게임 오브젝트를 법선벡터방향으로 회전");
        }
        RotateLeftSide(obj);
        GameObject scanner = GameObject.Find("Scanner");
        scanner.GetComponent<MeshRenderer>().enabled = true;
        gameObjects[1].transform.position += angle * MoveTime * Time.deltaTime * MoveSpeed;
        currMoveTime += Time.deltaTime; //시간의 흐흠을 체크한다.
        if (currMoveTime > MoveTime)
        {
            gameObjects[0].SetActive(false);
            Vectexs.Get.throwObj.RemoveAt(Vectexs.Get.throwindex);//첫번째꺼를 지운다

            //1초이상이 되면?(1초 정도만 오브젝트가 움직이게 해서 서로 떨어 뜨려 놓는다)
            cutFinish = false;//다시 원래 상태로 돌린다 
            if (cutFinish == false)
            {
                Invoke("ResetObj", 2.0f);
                scanner.GetComponent<MeshRenderer>().enabled = false;

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
        //Debug.Log(angle);
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
        MissionManager.Get.isMissionOn = true;
    }
    //gameObjects[0] 잘린 오브젝트의 원본을 회전시키기
    public void RotateLeftSide(GameObject obj)
    {
        gameObjects[0].transform.SetParent(obj.transform);//새로만든 오브젝트에 자식으로 넣기
        obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, Quaternion.LookRotation(-centerEye.transform.forward), Time.deltaTime * 4.0f);
    }
    List<GameObject> tempedge = new List<GameObject>();//생성된 꼭지점을 담아둘 임시 리스트
    List<Vector3> temppos = new List<Vector3>();
    bool templistFull = false; //임시리스트가 다 찼음을 알림
    bool templistLocate = false;
    //bool exceptrotate = false;
    public void FindEdge()
    {
        for (int i = 0; i < Vectexs.Get.vertexs_RemoveDuple.Count; i++)
        {
            tempedge.Add(Instantiate(edge, Vectexs.Get.vertexs_RemoveDuple[i], Quaternion.identity));
            temppos.Add(tempedge[i].transform.position);
        }
        templistFull = true;
        Vectexs.Get.RemoveDuple2(temppos);
    }
    public void EdgePosLocate()
    {

            //Debug.Log("edgeobj 생성함");
            //점찍고 나서 스케일과 위치를 다시 잡기위해서 점을 다 찍은후에 실행되야한다
            //Vectexs.Get.throwObj[Vectexs.Get.throwindex].GetComponent<Ball>().edgeobj
            edgeobj = Instantiate(edgeRoot, Vector3.zero, Quaternion.identity);
            edgeobj.transform.rotation = Quaternion.LookRotation(angle);
            for (int i = 0; i < tempedge.Count; i++)
            {
                tempedge[i].transform.SetParent(edgeobj.transform);
                //Debug.Log(edgeobj);
            }
            templistLocate = true;
            //Debug.Log(templistLocate);
        
        //edgeobj.transform.rotation = obj.transform.rotation;//leftSideRoot의 회전값과 일치 시켜준다?
    }
    public void EdgePosLocate2()
    {
            //Debug.Log(edgeobj);
            edgeobj.transform.position = obj.transform.position;
            edgeobj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, Quaternion.LookRotation(-centerEye.transform.forward), Time.deltaTime * 4.0f);
            edgeobj.transform.localScale = gameObjects[0].transform.localScale;
    }
    #region 오브젝트가 일정시간동안 칼에 닿으면 사라지게 처리하기위해서 조건검사
    ///온트리거 스태리로 시간을 체크해서 일정시간이 지나면 잘리지 않게 콜라이더 비활성화 오브젝안보이게 처리후 
    ///오브젝트비활성화 및 새로 생성부분으로 바로 넘어가게한다
    public float dieTime;
    public bool dieflow = false;
    bool objdes = false;
    int desindex;
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Blade")==true)
        {
            dieflow = true;
            desindex = objnum;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Blade") == true)
        {
            dieflow = false;
        }
    }
    public void ObjActivefalse()
    {
        gameObject.SetActive(false);
    }
    #endregion
}