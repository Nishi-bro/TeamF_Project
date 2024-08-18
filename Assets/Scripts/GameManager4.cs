using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class GameManager4 : MonoBehaviour
{
    public void StartButton()
    {
        //SceneManager.LoadScene("Countdawn");
        //Thread.Sleep(3000);
        //SceneManager.UnloadSceneAsync("Countdawn");
        SceneManager.LoadScene("Countdawn");
    }
}