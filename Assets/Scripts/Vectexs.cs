using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class Vectexs : MonoBehaviour
{
    
 private static Vectexs m_Instance = null;
     public static Vectexs Get { get { return m_Instance; } set { m_Instance = value; } }
    //public GameObject obj;
    //public GameObject sphere;
    public GameObject[] Figures;
    public Transform[] spawn;
    public GameObject pointRoot;
    public bool ismakeCube = true;
    public bool ismakeCylinder = true;
    public bool ismakeCone = true;
    public bool ismakeTetrahedron = true;
    public bool ismakeOctahedron = true;
    public bool isconePoint = false;
    public bool isconePlat = false;
    public bool isconSideStart = false; //옆면 부터 자르기 시작했다
    public bool isTop = false;
    public bool isBotum = false;
    public bool isStartPoint = false;//정사면체 정팔면체 자를시 시작점으로 꼭지점을 건드린경우
    public bool isEndPoint = false;//정사면체 정팔면체를 자를시 끝점으로 꼭지점을 건드린경우
    public bool edgeFind = false;//중복점 다 제거하고 잘린면의 꼭지점 찾음
    public bool vertexsField = false;//게임씬에 꼭지점을 다 찍었는지 체크
    public Transform objcenter;
    public List<GameObject> throwObj;
    public int currentFigure;
    public string msg;
    public GameObject success;
    public GameObject fail;
    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else if (m_Instance != this)
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        
    }
    // public Mesh ms = new Mesh();
    //public int duple;
    public bool isGet = false;
    //public GameObject point;
    //public GameObject pointRootleft;
    //public GameObject pointRootright;
    
    public List<Vector3> vertexs = new List<Vector3>(); //잘리는 단면의 교차점들을 받아올 리스트
    public List<Vector3> vertexs_RemoveDuple;
    public List<Vector3> tempList; //각 정점별로 생설할수 있는 모든 벡터들을 임시로 저장할곳
    public List<Vector3> tempList2;
    //public List<Vector3> vertorDis;
    public List<int> removeIndex;
    public List<int> removeIndex2;
    int num ;
    //public Vector3[] points;
    
    //public List<float> test = new List<float>();
    public List<Vector3> finalVectors = new List<Vector3>();
    //List<Vector3> fianlVectorsright = new List<Vector3>();
     private void Update()
    {
        if(isGet == true)
        {
            isGet = false;
          RemoveDuple();
        }
       if(ismakeCube == true)
        {
            ismakeCube = false;
            GameObject obj = Instantiate(Figures[0], spawn[0].position, Quaternion.identity);
            throwObj.Add(obj);
        }
        if(ismakeCylinder == true)
        {
            ismakeCylinder = false;
            GameObject obj = Instantiate(Figures[1], spawn[1].position, Quaternion.identity);
            throwObj.Add(obj);
        }
        if(ismakeCone == true)
        {
            ismakeCone = false;
            GameObject obj = Instantiate(Figures[2], spawn[2].position, Quaternion.identity);
            throwObj.Add(obj);
        }
        if(ismakeTetrahedron == true)
        {
            ismakeTetrahedron = false;
            GameObject obj = Instantiate(Figures[3], spawn[3].position, Quaternion.identity);
            throwObj.Add(obj);
        }
        if(ismakeOctahedron == true)
        {
            ismakeOctahedron = false;
            GameObject obj = Instantiate(Figures[4], spawn[4].position, Quaternion.identity);
            throwObj.Add(obj);
        }

        if(throwObj.Count==5)
        {
            ThrowObj();
        }
    }
    Vector3 nextVector;
    //최종 값들만 남김 아직 중간에 끼인 값은 제거 안함
    public void RemoveDuple() //x,y,z 을 비교해서 근사값이면 이전값을 덮어 씌운다.
    {
        for(int i=0; i<vertexs.Count;i++)
        {

            //i가 0이 아닐때만 이전값하고 비교해서 구한다
            if(vertexs_RemoveDuple.Contains(vertexs[i]))
            {

            }
            else //넣는다
            {
               vertexs_RemoveDuple.Add(vertexs[i]);
            }
        }
        //MakeVertex() 실행전에(사이값 제거하기)전에 근사값이 존재하면 전부다 일치 시켜주는 작업을한다.
        MakeVertex();
        //RemoveDuple2();

    }
    public bool FinalList = false;
    public List<Vector3> tempduple = new List<Vector3>();
    public List<Vector3> tempFinal = new List<Vector3>();
    float x;
    float y;
    float z;
    public void RemoveDuple2(List<Vector3>list)//버텍스를 생성하면서 그점을 매개변수로 받아서 생성후 위치 기준으로 처리해볼것
    {
        tempFinal = list;
        //Debug.Log(vertexs_RemoveDuple.Count);
        for(int i=0; i< tempFinal.Count;i++)
        {
            nextVector = Vector3.zero;
            for(int k=0; k< tempFinal.Count; k++)
            {
                nextVector = tempFinal[k];
                if (Mathf.Abs(tempFinal[i].x - nextVector.x) < 0.005f)
                {
                    
                    x = tempFinal[i].x;
                    //Debug.Log(x);
                }
                else
                {
                    x = nextVector.x;
                }
                if (Mathf.Abs(tempFinal[i].y - nextVector.y) < 0.005f)
                {
                   
                        y = tempFinal[i].y;
                }
                else
                {
                    y = nextVector.y;
                }
                if (Mathf.Abs(tempFinal[i].z - nextVector.z) < 0.005f)
                {
                    
                        z = tempFinal[i].z;
                }
                else
                {
                    z = nextVector.z;
                }
                //Debug.Log("바뀌기 전값" + vertexs_RemoveDuple[k]);
                tempFinal[k] = new Vector3(x, y, z);
                //Debug.Log("바꿀값" + nextVector);
            }
           
        }
        finalVectors.Clear();
        //Debug.Log("넣기전" + tempFinal.Count);
        for (int i = 0; i< tempFinal.Count;i++)
        {
            if(finalVectors.Contains(tempFinal[i]))
            {

            }
            else
            {
                //Debug.Log("넣는다");
                finalVectors.Add(tempFinal[i]);
            }
        }
        MakeVertex2();
       
    }
    public void MakeVertex2()
    {
        for (int i = 0; i < finalVectors.Count; i++)
        {
            tempList2.Clear();
            for (int k = 0; k < finalVectors.Count; k++)
            {
                if (finalVectors[i] != finalVectors[k]) //자기 자신하고는 비교할필요 없다
                {
                    tempList2.Add((finalVectors[k] - finalVectors[i]).normalized); //tempList에 해당 점에서 생길수 있는 모든 벡터들을 가져놓는다
                }
            }
            //i번쨰 인덱스와 구할수 있는 모든 벡터를 노멀라이즈 해서 구했다
            //tempList 에서 2개씩 짝지어서 모든 벡터를 cross해서 0이 나오는것을 찾아야한다 0이 나오는 것은 끼인값이다 
            for (int k = 0; k < tempList2.Count; k++)
            {
                for (int j = 0; j < tempList2.Count; j++)
                {
                    if (tempList2[k] != tempList2[j]) //tempList에는 노말라이즈한 방향만 들어가있다 같은 방향인것은 제외하고 비교한다. 찾고자하는것은 -부호만 다른벡터를 찾아서 제거하는것이다. 방향이 완전히 같은것은 예외
                    {
                        if (Vector3.Cross(tempList2[k], tempList2[j]) == Vector3.zero)
                        {
                            if (removeIndex2.Contains(i))
                            {

                            }
                            else
                            {
                                removeIndex2.Add(i);
                            }

                        }
                    }
                }
            }
        }
        //vertexs_RemoveDuple 에서 사이값들도 제거해준다 제거는 removeIndex에 있는 값들이 제거해야할 인덱스번호이다
        int removeCnt = 0;
        for (int k = 0; k < removeIndex2.Count; k++)
        {
            finalVectors.RemoveAt(removeIndex2[k] - removeCnt);
            removeCnt++;
        }
        //edgeFind = true;
        //CheckFigure(); //도형 판정
        CheckJudge();
    }
    public void MakeVertex()
    {
            for(int i=0; i<vertexs_RemoveDuple.Count; i++)
            {
                tempList.Clear();
                for(int k=0; k<vertexs_RemoveDuple.Count; k++)
                {
                    if(vertexs_RemoveDuple[i] != vertexs_RemoveDuple[k]) //자기 자신하고는 비교할필요 없다
                    {
                        tempList.Add((vertexs_RemoveDuple[k]-vertexs_RemoveDuple[i]).normalized); //tempList에 해당 점에서 생길수 있는 모든 벡터들을 가져놓는다
                    }
                }
                //i번쨰 인덱스와 구할수 있는 모든 벡터를 노멀라이즈 해서 구했다
                //tempList 에서 2개씩 짝지어서 모든 벡터를 cross해서 0이 나오는것을 찾아야한다 0이 나오는 것은 끼인값이다 
                for(int k=0; k<tempList.Count;k++)
                {
                    for(int j=0; j<tempList.Count;j++)
                    {
                        if(tempList[k]!=tempList[j]) //tempList에는 노말라이즈한 방향만 들어가있다 같은 방향인것은 제외하고 비교한다. 찾고자하는것은 -부호만 다른벡터를 찾아서 제거하는것이다. 방향이 완전히 같은것은 예외
                        {
                            if(Vector3.Cross(tempList[k],tempList[j])==Vector3.zero)
                            {
                                if(removeIndex.Contains(i))
                                {

                                }
                                else
                                {
                                     removeIndex.Add(i);
                                }
                              
                            }
                        }
                    }
                }
            }
        //vertexs_RemoveDuple 에서 사이값들도 제거해준다 제거는 removeIndex에 있는 값들이 제거해야할 인덱스번호이다
         int removeCnt=0;
        for(int k=0; k<removeIndex.Count;k++)
        {
            vertexs_RemoveDuple.RemoveAt(removeIndex[k]-removeCnt);
            removeCnt++;
        }
        edgeFind = true;
        //CheckFigure(); //도형 판정
    }
    public int throwindex;
    public void ThrowObj()
    {
        for(int i=0;i<throwObj.Count;i++)
        {
            if(throwObj[i].GetComponent<Ball>().objnum == MissionManager.Get.nowMission)
            {
                throwindex = i;
                throwObj[i].GetComponent<Ball>().enabled = true;
            }
        }
        Debug.Log(throwindex + "번쨰 도형");
        throwObj[throwindex].transform.position = Vector3.MoveTowards(throwObj[throwindex].transform.position, objcenter.position, 10 * Time.deltaTime);
    }
    /// <summary>
    /// 0번큐브 1번 실린더 2번 원뿔 3번 정사면체 4번 정팔면체
    /// 도형 모양을 체크하기 위함
    /// </summary>
    public void CheckFigure() 
    {
        switch(currentFigure)
        {
            case 0:
                
                if(finalVectors.Count==3)
                {
                   
                }
                else if (finalVectors.Count == 4)
                {
                   
                }
                else
                {
                    msg = "자른 단면은 " + finalVectors.Count + "각형입니다.";
                }

                    break;
            case 1:
               
                if((isTop == true && isBotum == true) || finalVectors.Count==4)
                {
                    msg = "자른 단면은 직사각형 입니다.";
                }
                else if(isTop == false && isBotum == false && finalVectors.Count >7)
                {
                    //if(finalVectors.Count !=4)
                    msg = "자른 단면은 원 입니다.";
                }
                else if((isTop == true && isBotum == false&& finalVectors.Count != 4) ||
                    (isTop == false && isBotum == true && finalVectors.Count != 4))
                {
                    msg = "자른 단면은 포물선 입니다.";
                }
                else
                {
                    msg = "사각형도 아니고 원도 아니고 포물선도 아니다";
                }
                isTop = false;
                isBotum = false;
                break;
            case 2:
                if(isconePoint == true && isconePlat == true)//원뿔의 꼭지점과 바닥을 건드린경우
                {
                    msg = "자른 단면은 삼각형입니다";
                }
                else if(isconePoint == false && isconePlat == true)//원뿔의 꼭지점을 안건드리고 바닥만 건드린경우
                {
                    msg = "자른 단면은 포물선입니다";//원과 타원 포물선을 구분해야함
                }
                else if(isconePoint == false && isconePlat == false) //꼭지점도 안지나고 바닥도 안지난경우는 원이 나올것이다
                {
                    msg = "자른 단면은 원 입니다.";
                }
                //자르고 난후 단면 판정이 끝나면 bool 값은 원래대로
                isconePoint = false;
                isconePlat = false;
                break;
            case 3:
                msg = "자른 단면은 " + finalVectors.Count + "각형입니다.";
                isStartPoint = false;
                isEndPoint = false;
                break;
            case 4:
                msg = "자른 단면은 " + finalVectors.Count + "각형입니다.";
                isStartPoint = false;
                isEndPoint = false;
                break;
        }
    }
    public void CheckJudge()
    {
        switch(MissionManager.Get.judgment)
        {
            case 0:
                if(finalVectors.Count==3)
                {
                    Debug.Log("성공");
                    MissionManager.Get.nowScore += 100;
                    success.SetActive(true);
                }
                else
                {
                    fail.SetActive(true);
                }
                ResetBool();
                break;
            case 1:
                if (finalVectors.Count == 4)
                {
                    Debug.Log("성공");
                    MissionManager.Get.nowScore += 100;
                    success.SetActive(true);
                }
                else if((isTop == true && isBotum == true) || finalVectors.Count == 4)
                    {
                    Debug.Log("성공");
                    MissionManager.Get.nowScore += 100;
                    success.SetActive(true);
                }
                    else
                {
                    fail.SetActive(true);
                }
                ResetBool();
                break;
            case 2:
                if(finalVectors.Count ==5 )
                {
                    Debug.Log("성공");
                    MissionManager.Get.nowScore += 100;
                    success.SetActive(true);
                }
                else
                {
                    fail.SetActive(true);
                }
                ResetBool();
                break;
            case 3:
                if(finalVectors.Count == 6)
                {
                    Debug.Log("성공");
                    MissionManager.Get.nowScore += 100;
                    success.SetActive(true);
                }
                else
                {
                    fail.SetActive(true);
                }
                ResetBool();
                break;
            case 4:
                if (isTop == false && isBotum == false && finalVectors.Count > 7)
                {
                    Debug.Log("성공");
                    MissionManager.Get.nowScore += 100;
                    success.SetActive(true);
                }
                else if(isconePoint == false && isconePlat == false)
                {
                    Debug.Log("성공");
                    MissionManager.Get.nowScore += 100;
                    success.SetActive(true);
                }
                else
                {
                    fail.SetActive(true);
                }
                ResetBool();
                break;
            case 5:
                if (isconePoint == false && isconePlat == true)
                {
                    Debug.Log("성공");
                    MissionManager.Get.nowScore += 100;
                    success.SetActive(true);
                }
                else if ((isTop == true && isBotum == false && finalVectors.Count != 4) ||
                            (isTop == false && isBotum == true && finalVectors.Count != 4))
                {
                    Debug.Log("성공");
                    MissionManager.Get.nowScore += 100;
                    success.SetActive(true);
                }
                else
                {
                    fail.SetActive(true);
                }
                ResetBool();
                break;
        }
    }
    public void ResetBool()
    {
        isTop = false;
        isBotum = false;
        isconePoint = false;
        isconePlat = false;
        isStartPoint = false;
        isEndPoint = false;
        //MissionManager.Get.isMissionOn = true;
    }
    //public void TriangleCheck()
    //{
    //    //절대값으로 각 점간의 길이 구하기
    //    float a = Mathf.Abs((finalVectors[0] - finalVectors[1]).sqrMagnitude);
    //    float b = Mathf.Abs((finalVectors[0] - finalVectors[2]).sqrMagnitude);
    //    float c = Mathf.Abs((finalVectors[1] - finalVectors[2]).sqrMagnitude);
    //    //각 길이에 10씩 곱하기
    //    a = a * 10;
    //    b = b * 10;
    //    c = c * 10;
    //    Debug.Log(a);
    //    Debug.Log(b);
    //    Debug.Log(c);
    //    if(Mathf.Abs(a-b)<0.5 && Mathf.Abs(a - c) < 0.5 && Mathf.Abs(c - b) < 0.5)
    //    {
    //        msg = "자른 단면은 정삼각형 입니다.";
    //    }
    //    else
    //    {
            
    //        if(Mathf.Abs(a - b) < 0.5 && Mathf.Abs(a - c) < 1 && Mathf.Abs(b - c)<1)
    //        {
    //            msg = "자른 단면은 이등변 삼각형 입니다.";
    //        }
    //        else
    //        {
    //            msg = "자른 단면은 삼각형 입니다.";
    //        }
    //    }
    //}
    //public void Square()
    //{
    //    float a= Mathf.Abs((finalVectors[1] - finalVectors[0]).sqrMagnitude);
    //    float b = Mathf.Abs((finalVectors[2] - finalVectors[0]).sqrMagnitude);
    //    float c = Mathf.Abs((finalVectors[3] - finalVectors[1]).sqrMagnitude);
    //    float d = Mathf.Abs((finalVectors[3] - finalVectors[2]).sqrMagnitude);
    //    //각 길이에 10씩 곱하기 
    //    a = a * 10;
    //    b = b * 10;
    //    c = c * 10;
    //    d = d * 10;
    //    if(Mathf.Abs(a-d)<0.5 && Mathf.Abs(b-c)<0.5 && Mathf.Abs(a-c)<0.5)
    //    {
    //        msg = "자른 단면은 정사각형 입니다.";
    //    }
    //    else if(Mathf.Abs(a - d) > 0.5 && Mathf.Abs(b - c) < 0.5 && Mathf.Abs(a - b) > 0.5)
    //    {
    //        msg = "자른 단면은 평행사변형 입니다.";
    //    }
    //    else if(Mathf.Abs(a - b) > 0.5 && Mathf.Abs(a - c) > 0.5 && Mathf.Abs(a - d) > 0.5&& Mathf.Abs(b - d) > 0.5)
    //    {
    //        msg = "자른 단면은 사다리꼴 입니다.";
    //    }
    //    else
    //    {
    //        msg = "자른 단면은 직각 삼각형 입니다.";
    //    }
    //}
    

}
