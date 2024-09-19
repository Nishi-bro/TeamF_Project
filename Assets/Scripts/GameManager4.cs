using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class GameManager4 : MonoBehaviour
{

    public AudioSource seAudioSource; // SEを再生するためのAudioSource
    public GameObject canvas;
    void Start()
    {
        Screen.SetResolution(1920, 1080, false); // 第三引数 false はウィンドウモード
    }
    public void StartButton()
    {
        //SceneManager.LoadScene("Countdawn");
        //Thread.Sleep(3000);
        //SceneManager.UnloadSceneAsync("Countdawn");
        SceneManager.LoadScene("IntroScene");
    }
    public void TitleButton()
    {
        SceneManager.LoadScene("startButton");
    }
    public void ZukanButton()
    {
        canvas.SetActive(true);
    }
    public void CloseButton()
    {
        canvas.SetActive(false);
    }

}