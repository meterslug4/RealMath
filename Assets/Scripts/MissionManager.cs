using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    private static MissionManager m_Instance = null;
     public static MissionManager Get { get { return m_Instance; } set { m_Instance = value; } }

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
    public Image missionUi;
    public Text missionText;
    public Text missionName;
    public Text scoreText;
    //현재 씬번호
    [Header("현재씬번호")]
    public int nowScenNum;
    [Header("어떤 도형에관한문제인가")]
    public int nowMission;
    [Header("미션목표단면")]
    public int nowGoal;
    public int judgment;
    //미션을 내려주는 기준이 되는 불 변수
    public bool isMissionOn = true;
    public bool isTimeFlow = false;
    public int nowScore;
    public GameObject success;
    public GameObject fail;
    public GameObject missionEffect;

    void Start()
    {
        nowScenNum = SceneManager.GetActiveScene().buildIndex;
        nowScore = 0;
        if (nowScenNum == 2)
        {
            nowScore = 500;
        }
        if( nowScenNum ==3)
        {
            nowScore = 1000;
        }

    }

   
    void Update()
    {
        scoreText.text = nowScore.ToString();
        if(isMissionOn==true && nowScore<3000)
        {
            missionEffect.GetComponent<ParticleSystem>().Play();
            isMissionOn = false;
            success.SetActive(false);
            fail.SetActive(false);

            MissionOn();
        }
        if(nowScore >=500 && nowScenNum==1)
        {
            Invoke("LoadScene2", 2.5f);
        }
        if (nowScore >= 1000 && nowScenNum == 2)
        {
            Invoke("LoadScene3", 2.5f);
        }

    }


    void LoadScene2()
    {
        SceneManager.LoadScene(2);
    }
    void LoadScene3()
    {
        SceneManager.LoadScene(3);
    }

    public void MissionOn()
    {
        //씬구분
        if (LevelLoder.gameLevel == 1)
        {
            Debug.Log("난이도 하");
            nowMission = Random.Range(0,5);
            LowLvMission(nowMission);
        }
        else if (LevelLoder.gameLevel == 2)
        {
            Debug.Log("난이도 중");
            nowMission = Random.Range(0,5);
            MidLvMission(nowMission);
        }
        else if (LevelLoder.gameLevel == 3)
        {
            Debug.Log("난이도 상");
            nowMission = Random.Range(0,5);
            Debug.Log(nowMission);
            HighLvMission(nowMission);
        }
        else
        {

        }
    }
    /// <summary>
    ///  0~4 사이의 번호를 매개변수로 받고 0번 큐브 1번 실린더 2번 원뿔 3번 정사면체 4번 정팔면체 관련미션이다
    /// </summary>
    /// <param name="num"></param>
    public void HighLvMission(int num)
    {
        if(num==0)
        {
            Debug.Log("큐브관련 미션"); 

                nowGoal = Random.Range(0, 7);
                if (nowGoal == 0)
                {
                    MakeTriangle();//삼각형
                }
                else if (nowGoal == 1)
                {
                    MakeSquare();//사각형
                }
                else if (nowGoal == 2)
                {
                    MakePenta();//오각형
                }
                else if(nowGoal ==3)
                {
                    MakeHexa();//육각형
                }
                else if (nowGoal == 4)
                {
                    MakeRect();//직사각형
                }
                else if (nowGoal == 5)
                {
                    MakeTrapezoid();//사다리꼴
                }
                else
                {
                    MakeSquare2();//정사각형
                }
        }//큐브미션
        else if(num==1)
        {
                Debug.Log("실린더관련 미션"); //사각형 포물선 원 타원
                nowGoal = Random.Range(0, 3);
                if (nowGoal == 0)
                {
                    MakeSquare();
                }
                else if (nowGoal == 1)
                {
                    MakeCircle();
                }
                //else if (nowGoal == 2)
                //{
                //    MakeParabola();
                //}
                else
                {
                    MakeEllipse();
                }
        }//실린더미션
        else if(num==2)
        {
            Debug.Log("원뿔관련 미션"); //삼각형, 타원, 원, 포물선, 쌍곡선

                nowGoal = Random.Range(0, 5);
                if (nowGoal == 0)
                {
                    MakeTriangle();//삼각형
                }
                else if (nowGoal == 1)
                {
                    MakeCircle();//원
                }
                else if (nowGoal == 2)
                {
                    MakeParabola();//포물선
                }
                else if (nowGoal == 3)
                {
                    MakeEllipse();//타원
                }
                else
                {
                    MakeHyperbola();//쌍곡선
                }
        }//원뿔미션
        else if(num==3)
        {
            Debug.Log("정사면체관련 미션");
                nowGoal = Random.Range(0, 2);
                if (nowGoal == 0)
                {
                    MakeTriangle();
                }
                else
                {
                    MakeSquare();
                }
            
        }//정사면체미션
        else
        {
            Debug.Log("정팔면체관련 미션");
 
                nowGoal = Random.Range(0, 2);
                if (nowGoal == 0)
                {
                    MakeSquare();
                }
                //else if (nowGoal == 1)
                //{
                //    MakePenta();
                //}
                else
                {
                    MakeHexa();
                }
        }//정팔면체미션
    }
    public void MidLvMission(int num)
    {
        if (num == 0)
        {
            Debug.Log("큐브관련 미션");

            nowGoal = Random.Range(0, 7);
            if (nowGoal == 0)
            {
                MakeTriangle();//삼각형
            }
            else if (nowGoal == 1)
            {
                MakeSquare();//사각형
            }
            else if (nowGoal == 2)
            {
                MakePenta();//오각형
            }
            else if (nowGoal == 3)
            {
                MakeHexa();//육각형
            }
            else if (nowGoal == 4)
            {
                MakeRect();//직사각형
            }
            else if (nowGoal == 5)
            {
                MakeTrapezoid();//사다리꼴
            }
            else
            {
                MakeSquare2();//정사각형
            }


        }//큐브미션
        else if (num == 1)
        {
            Debug.Log("실린더관련 미션"); //사각형 포물선 원 타원

                nowGoal = Random.Range(0, 3);
                if (nowGoal == 0)
                {
                    MakeSquare();
                }
                else if (nowGoal == 1)
                {
                    MakeCircle();
                }
                //else if (nowGoal == 2)
                //{
                //    MakeParabola();
                //}
                else
                {
                    MakeEllipse();
                }
        }//실린더미션
        else if (num == 2)
        {
            Debug.Log("원뿔관련 미션"); //삼각형, 타원, 원, 포물선, 쌍곡선
                nowGoal = Random.Range(0, 5);
                if (nowGoal == 0)
                {
                    MakeTriangle();//삼각형
                }
                else if (nowGoal == 1)
                {
                    MakeCircle();//원
                }
                else if (nowGoal == 2)
                {
                    MakeParabola();//포물선
                }
                else if (nowGoal == 3)
                {
                    MakeEllipse();//타원
                }
                else
                {
                    MakeHyperbola();//쌍곡선
                }
        }//원뿔미션
        else if (num == 3)
        {
            Debug.Log("정사면체관련 미션");

                nowGoal = Random.Range(0, 2);
                if (nowGoal == 0)
                {
                    MakeTriangle();
                }
                else
                {
                    MakeSquare();
                }
            
           
        }//정사면체미션
        else
        {
            Debug.Log("정팔면체관련 미션");

                nowGoal = Random.Range(0, 2);
                if (nowGoal == 0)
                {
                    MakeSquare();
                }
                //else if (nowGoal == 1)
                //{
                //    MakePenta();
                //}
                else
                {
                    MakeHexa();
                }
        }//정팔면체미션
    }
    public void LowLvMission(int num)
    {
        if (num == 0)
        {
            Debug.Log("큐브관련 미션");
 
                nowGoal = Random.Range(0, 7);
                if (nowGoal == 0)
                {
                    MakeTriangle();//삼각형
                }
                else if (nowGoal == 1)
                {
                    MakeSquare();//사각형
                }
                else if (nowGoal == 2)
                {
                    MakePenta();//오각형
                }
                else if (nowGoal == 3)
                {
                    MakeHexa();//육각형
                }
                else if (nowGoal == 4)
                {
                    MakeRect();//직사각형
                }
                else if (nowGoal == 5)
                {
                    MakeTrapezoid();//사다리꼴
                }
                else
                {
                    MakeSquare2();//정사각형
                }
        }//큐브미션
        else if (num == 1)
        {
            Debug.Log("실린더관련 미션"); //사각형 포물선 원 타원
 
                nowGoal = Random.Range(0, 3);
                if (nowGoal == 0)
                {
                    MakeSquare();
                }
                else if (nowGoal == 1)
                {
                    MakeCircle();
                }
                //else if (nowGoal == 2)
                //{
                //    MakeParabola();
                //}
                else
                {
                    MakeEllipse();
                }
        }//실린더미션
        else if (num == 2)
        {
            Debug.Log("원뿔관련 미션"); //삼각형, 타원, 원, 포물선, 쌍곡선

                nowGoal = Random.Range(0, 5);
                if (nowGoal == 0)
                {
                    MakeTriangle();//삼각형
                }
                else if (nowGoal == 1)
                {
                    MakeCircle();//원
                }
                else if (nowGoal == 2)
                {
                    MakeParabola();//포물선
                }
                else if (nowGoal == 3)
                {
                    MakeEllipse();//타원
                }
                else
                {
                    MakeHyperbola();//쌍곡선
                }
        }//원뿔미션
        else if (num == 3)
        {
            Debug.Log("정사면체관련 미션");

                nowGoal = Random.Range(0, 2);
                if (nowGoal == 0)
                {
                    MakeTriangle();
                }
                else
                {
                    MakeSquare();
                }
        }//정사면체미션
        else
        {
            Debug.Log("정팔면체관련 미션");

                nowGoal = Random.Range(0, 2);
                if (nowGoal == 0)
                {
                    MakeSquare();
                }
                //else if (nowGoal == 1)
                //{
                //    MakePenta();
                //}
                else
                {
                    MakeHexa();
                }
        }//정팔면체미션
    }
    /// <summary>
    /// 삼각형은 큐브, 정사면체, 원뿔 에서만 발생해야함
    /// </summary>
    public void MakeTriangle()
    {
        missionText.text = "삼각형을";
        missionName.text = "Triangle";
        missionUi.sprite = Resources.Load<Sprite>("Triangle");
        judgment = 0;
    }
    /// <summary>
    /// 사각형은 큐브, 실린더, 정사면,체 정팔면체에서 발생함
    /// </summary>
    public void MakeSquare()
    {
        missionText.text = "사각형을";
        missionName.text = "Quadrate";
        missionUi.sprite = Resources.Load<Sprite>("Quadrate");
        judgment = 1;
    }
    /// <summary>
    /// 오각형은 큐브, 정팔면체 에서 발생함
    /// </summary>
    public void MakePenta()
    {
        missionText.text = "오각형을";
        missionName.text = "Pentagon";
        missionUi.sprite = Resources.Load<Sprite>("Pentagon");
        judgment = 2;
    }
    /// <summary>
    /// 육각형은 정팔면체, 큐브 에서만 발생함
    /// </summary>
    public void MakeHexa()
    {
        missionText.text = "육각형을";
        missionName.text = "Hexagon";
        missionUi.sprite = Resources.Load<Sprite>("Hexagon");
        judgment = 3;
    }
    /// <summary>
    /// 원은 원뿔,실린더에서만 생김
    /// </summary>
    public void MakeCircle()
    {
        missionText.text = "원을";
        missionName.text = "Circle";
        missionUi.sprite = Resources.Load<Sprite>("Circle");
        judgment = 4;
    }
    /// <summary>
    /// 포물선은 실린더, 원뿔에서만 생김
    /// </summary>
    public void MakeParabola()
    {
        missionText.text = "포물선을";
        missionName.text = "Parabola";
        missionUi.sprite = Resources.Load<Sprite>("Parabola");
        judgment = 5;
    }
    /// <summary>
    /// 쌍곡선은 원뿔에서만 나옴
    /// </summary>
    public void MakeHyperbola()
    {
        missionText.text = "쌍곡선을";
        missionName.text = "Hyperbola";
        missionUi.sprite = Resources.Load<Sprite>("Hyperbola");
        judgment = 6;
    }
    /// <summary>
    /// 타원은 원뿔, 실린더에서 발생
    /// </summary>
    public void MakeEllipse()
    {
        missionText.text = "타원을";
        missionName.text = "Ellipse";
        missionUi.sprite = Resources.Load<Sprite>("Ellipse");
        judgment = 7;
    }
    public void MakeRect()
    {
        missionText.text = "직사각형을";
        missionName.text = "Rectangle";
        missionUi.sprite = Resources.Load<Sprite>("Rectangle");
        judgment = 8;
    }
    public void MakeTrapezoid()
    {
        missionText.text = "사다리꼴을";
        missionName.text = "Trapezoid";
        missionUi.sprite = Resources.Load<Sprite>("Trapezoid");
        judgment = 9;
    }
    public void MakeSquare2()
    {
        missionText.text = "정사각형을";
        missionName.text = "Quadrate";
        missionUi.sprite = Resources.Load<Sprite>("Quadrate");
        judgment = 10;
    }
}
