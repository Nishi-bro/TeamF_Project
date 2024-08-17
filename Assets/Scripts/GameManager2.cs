using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("GameScene");
    }
}