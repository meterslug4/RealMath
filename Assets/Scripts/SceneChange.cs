using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void ChangeStartScene()
    {
        SceneManager.LoadScene("01.StartScene");
    }

    public void ChangeGameScene()
    {
        Debug.Log("aaa");
        SceneManager.LoadScene("02.GameScene");
    }

}
