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
    public Text missionText;
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

    void Start()
    {
        nowScore = 0;
        nowScenNum = SceneManager.GetActiveScene().buildIndex;
    }

   
    void Update()
    {
        scoreText.text = nowScore.ToString();
        if(isMissionOn==true)
        {
           isMissionOn = false;
            MissionOn();
        }
    }
    public void MissionOn()
    {
        //씬구분
        if (nowScenNum == 1)
        {
            Debug.Log("1스테이지 미션");
            nowMission = Random.Range(0,5);
            MissionFigure(nowMission);
        }
        else if (nowScenNum == 2)
        {
            Debug.Log("2스테이지 미션");
            nowMission = Random.Range(0,5);
             MissionFigure(nowMission);
        }
        else if (nowScenNum == 3)
        {
            Debug.Log("3스테이지 미션");
            nowMission = Random.Range(0,5);
             MissionFigure(nowMission);
        }
        else
        {

        }
    }
    /// <summary>
    ///  0~4 사이의 번호를 매개변수로 받고 0번 큐브 1번 실린더 2번 원뿔 3번 정사면체 4번 정팔면체 관련미션이다
    /// </summary>
    /// <param name="num"></param>
    public void MissionFigure(int num)
    {
        if(num==0)
        {
            Debug.Log("큐브관련 미션"); 
            if(nowScenNum==1)
            {
                nowGoal = Random.Range(0, 2);
                if(nowGoal==0)
                {
                    MakeTriangle();
                }
                else
                {
                    MakeSquare();
                }
            }
            else if(nowScenNum==2)
            {
                nowGoal = Random.Range(0, 2);
                if (nowGoal == 0)
                {
                    MakePenta();
                }
                else
                {
                    MakeHexa();
                }
            }
            else if(nowScenNum==3)
            {
                nowGoal = Random.Range(0, 4);
                if(nowGoal ==0)
                {
                    MakeTriangle();
                }
                else if(nowGoal ==1)
                {
                    MakeSquare();
                }
                else if(nowGoal == 2)
                {
                    MakePenta();
                }
                else
                {
                    MakeHexa();
                }
            }
        }
        else if(num==1)
        {
            Debug.Log("실린더관련 미션");
            if (nowScenNum == 1)
            {
                nowGoal = Random.Range(0, 2);
                if(nowGoal ==0)
                {
                    MakeSquare();
                }
                else
                {
                    MakeCircle();
                }
            }
            else if (nowScenNum == 2)
            {
                MakeParabola();
            }
            else if (nowScenNum == 3)
            {
                nowGoal = Random.Range(0, 3);
                if(nowGoal==0)
                {
                    MakeSquare();
                }
                else if(nowGoal ==1)
                {
                    MakeCircle();
                }
                else
                {
                    MakeParabola();
                }
            }
        }
        else if(num==2)
        {
            Debug.Log("원뿔관련 미션");
            if (nowScenNum == 1)
            {
                nowGoal = Random.Range(0, 2);
                if(nowGoal ==0)
                {
                    MakeTriangle();
                }
                else
                {
                    MakeCircle();
                }
            }
            else if (nowScenNum == 2)
            {
                MakeParabola();
            }
            else if (nowScenNum == 3)
            {
                nowGoal = Random.Range(0, 3);
                if(nowGoal ==0)
                {
                    MakeTriangle();
                }
                else if(nowGoal ==1)
                {
                    MakeCircle();
                }
                else
                {
                    MakeParabola();
                }
            }
        }
        else if(num==3)
        {
            Debug.Log("정사면체관련 미션");
            if (nowScenNum == 1)
            {
                MakeTriangle();
            }
            else if (nowScenNum == 2)
            {
                MakeSquare();
            }
            else if (nowScenNum == 3)
            {
                nowGoal = Random.Range(0, 2);
                if(nowGoal ==0)
                {
                    MakeTriangle();
                }
                else
                {
                    MakeSquare();
                }
            }
        }
        else
        {
            Debug.Log("정팔면체관련 미션");
            if (nowScenNum == 1)
            {
                MakeSquare();
            }
            else if (nowScenNum == 2)
            {
                nowGoal = Random.Range(0, 2);
                if (nowGoal == 0)
                {
                    MakePenta();
                }
                else
                {
                    MakeHexa();
                }
            }
            else if (nowScenNum == 3)
            {
                nowGoal = Random.Range(0, 3);
                if(nowGoal ==0)
                {
                    MakeSquare();
                }
                else if(nowGoal ==1)
                {
                    MakePenta();
                }
                else
                {
                    MakeHexa();
                }
            }
        }
    }
    public void MakeTriangle()
    {
        missionText.text = "삼각형을";
        judgment = 0;
    }
    public void MakeSquare()
    {
        missionText.text = "사각형을";
        judgment = 1;
    }
    public void MakePenta()
    {
        missionText.text = "오각형을";
        judgment = 2;
    }
    public void MakeHexa()
    {
        missionText.text = "육각형을";
        judgment = 3;
    }
    public void MakeCircle()
    {
        missionText.text = "원을";
        judgment = 4;
    }
    public void MakeParabola()
    {
        missionText.text = "포물선을";
        judgment = 5;
    }
}
