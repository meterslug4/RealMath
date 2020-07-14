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
    public GameObject text3d;
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
    public Transform objcenter;
    public List<GameObject> throwObj;
    public int currentFigure;
    public string msg;
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

        DontDestroyOnLoad(gameObject);
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
    //public List<Vector3> vertorDis;
    public List<int> removeIndex;
    int num ;
    //public Vector3[] points;
    
    //public List<float> test = new List<float>();
    List<Vector3> fianlVectors = new List<Vector3>();
    List<Vector3> fianlVectorsright = new List<Vector3>();
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

    //최종 값들만 남김 아직 중간에 끼인 값은 제거 안함
    public void RemoveDuple()
    {
        for(int i=0; i<vertexs.Count;i++)
        {
            if(vertexs_RemoveDuple.Contains(vertexs[i]))
            {

            }
            else
            {
                vertexs_RemoveDuple.Add(vertexs[i]);
            }
        }
        MakeVertex();

    }

    public void MakeVertex()
    {
     for(int i=0; i<vertexs_RemoveDuple.Count; i++)
            {
                tempList.Clear();

                //Debug.Log(i+ "번쨰 루프");
                for(int k=0; k<vertexs_RemoveDuple.Count; k++)
                {
                    if(vertexs_RemoveDuple[i] != vertexs_RemoveDuple[k]) //자기 자신하고는 비교할필요 없다
                    {
                        tempList.Add((vertexs_RemoveDuple[k]-vertexs_RemoveDuple[i]).normalized); //tempList에 해당 점에서 생길수 있는 모든 벡터들을 가져놓는다
                    }
                }
                //Debug.Log(tempList.Count);

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
        //vertexs_RemoveDuple.RemoveAt(4);
         //vertexs_RemoveDuple.RemoveAt(5);
          //for(int i=0; i<vertexs_RemoveDuple.Count;i++)
          //  {
          //      GameObject obj = Instantiate(text3d,vertexs_RemoveDuple[i],Quaternion.identity);
          //      obj.GetComponent<TextMesh>().text = i.ToString();
          //  }
        //Debug.Log("자른 단면은 "+ vertexs_RemoveDuple.Count +"각형입니다.");
        CheckFigure(); //도형 판정
    }
    public void ThrowObj()
    {
        //Debug.Log("이동");
        throwObj[0].transform.position = Vector3.MoveTowards(throwObj[0].transform.position, objcenter.position, 10 * Time.deltaTime);
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
                
                if(vertexs_RemoveDuple.Count==3)
                {
                    TriangleCheck();
                }
                else if (vertexs_RemoveDuple.Count == 4)
                {
                    Square();
                }
                else
                {
                    msg = "자른 단면은 " + vertexs_RemoveDuple.Count + "각형입니다.";
                }

                    break;
            case 1:
               
                if(isTop == true && isBotum == true)
                {
                    msg = "자른 단면은 직사각형 입니다.";
                }
                else if(isTop == false && isBotum == false)
                {
                    msg = "자른 단면은 원 입니다.";
                }
                else
                {
                    msg = "자른 단면은 포물선 입니다.";
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
                msg = "자른 단면은 " + vertexs_RemoveDuple.Count + "각형입니다.";
                break;
            case 4:
                msg = "자른 단면은 " + vertexs_RemoveDuple.Count + "각형입니다.";
                break;
        }
    }
    public void TriangleCheck()
    {
        //절대값으로 각 점간의 길이 구하기
        float a = Mathf.Abs((vertexs_RemoveDuple[0] - vertexs_RemoveDuple[1]).sqrMagnitude);
        float b = Mathf.Abs((vertexs_RemoveDuple[0] - vertexs_RemoveDuple[2]).sqrMagnitude);
        float c = Mathf.Abs((vertexs_RemoveDuple[1] - vertexs_RemoveDuple[2]).sqrMagnitude);
        //각 길이에 10씩 곱하기
        a = a * 10;
        b = b * 10;
        c = c * 10;
        Debug.Log(a);
        Debug.Log(b);
        Debug.Log(c);
        if(Mathf.Abs(a-b)<0.5 && Mathf.Abs(a - c) < 0.5 && Mathf.Abs(c - b) < 0.5)
        {
            msg = "자른 단면은 정삼각형 입니다.";
        }
        else
        {
            
            if(Mathf.Abs(a - b) < 0.5 && Mathf.Abs(a - c) < 1 && Mathf.Abs(b - c)<1)
            {
                msg = "자른 단면은 이등변 삼각형 입니다.";
            }
            else
            {
                msg = "자른 단면은 삼각형 입니다.";
            }
        }
    }
    public void Square()
    {
        float a= Mathf.Abs((vertexs_RemoveDuple[1] - vertexs_RemoveDuple[0]).sqrMagnitude);
        float b = Mathf.Abs((vertexs_RemoveDuple[2] - vertexs_RemoveDuple[0]).sqrMagnitude);
        float c = Mathf.Abs((vertexs_RemoveDuple[3] - vertexs_RemoveDuple[1]).sqrMagnitude);
        float d = Mathf.Abs((vertexs_RemoveDuple[3] - vertexs_RemoveDuple[2]).sqrMagnitude);
        //각 길이에 10씩 곱하기 
        a = a * 10;
        b = b * 10;
        c = c * 10;
        d = d * 10;
        if(Mathf.Abs(a-d)<0.5 && Mathf.Abs(b-c)<0.5 && Mathf.Abs(a-c)<0.5)
        {
            msg = "자른 단면은 정사각형 입니다.";
        }
        else if(Mathf.Abs(a - d) > 0.5 && Mathf.Abs(b - c) < 0.5 && Mathf.Abs(a - b) > 0.5)
        {
            msg = "자른 단면은 평행사변형 입니다.";
        }
        else if(Mathf.Abs(a - b) > 0.5 && Mathf.Abs(a - c) > 0.5 && Mathf.Abs(a - d) > 0.5&& Mathf.Abs(b - d) > 0.5)
        {
            msg = "자른 단면은 사다리꼴 입니다.";
        }
        else
        {
            msg = "자른 단면은 직각 삼각형 입니다.";
        }
    }
}
