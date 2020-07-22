using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoder : MonoBehaviour
{
    public static int gameLevel;

    public Animator transition;
    public float transitionTime = 1f;
    public GameObject levelUI;
    public GameObject startUI;
    // Update is called once per frame

    private void Awake()
    {
        
    }
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    LoadNextLevel();
        //}
                //LoadNextLevel();
    }

    public void SelectLv()
    {
        gameObject.GetComponent<AudioSource>().Play();
        levelUI.SetActive(true);
        startUI.SetActive(false);
    }

    public void LoadHighLv()
    {
        gameLevel = 3;
        Debug.Log("클릭");
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadMidLv()
    {
        gameLevel = 2;
        Debug.Log("클릭");
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadLowLv()
    {
        gameLevel = 1;
        Debug.Log("클릭");
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadStart()
    {
        StartCoroutine(LoadLevel(0));
    }
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
