using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ButtonController : MonoBehaviour
{
    Button button;
    [SerializeField] GameObject obj;

    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(RetryButton);
    }

    public void RetryButton()
    {
        SceneManager.LoadScene("StartButton");
    }
}
