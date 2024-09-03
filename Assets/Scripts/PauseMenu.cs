using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // PausePanelを割り当てる
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pausePanel.SetActive(true); // Pauseメニューを表示
        Time.timeScale = 0f; // ゲームを一時停止
        isPaused = true;
    }

    void ResumeGame()
    {
        pausePanel.SetActive(false); // Pauseメニューを非表示
        Time.timeScale = 1f; // ゲームを再開
        isPaused = false;
    }
}
