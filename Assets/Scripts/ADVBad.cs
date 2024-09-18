using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理のために必要
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    public Canvas[] panels; // 切り替えるCanvas（背景やキャラクターなど）の配列
    public Button loadCountdownButton; // "Countdown"シーンをロードするボタン
    private int currentPanelIndex = 0; // 現在表示しているCanvasのインデックス

    void Start()
    {
        // 最初のCanvasのみ表示
        ShowCurrentPanel();

        // ボタンが設定されていれば、クリック時にシーンをロードするリスナーを追加
        if (loadCountdownButton != null)
        {
            loadCountdownButton.onClick.AddListener(LoadCountdownScene);
        }
    }

    void Update()
    {
        // Qキーが押されたら次のCanvasを表示
        if (Input.GetKeyDown(KeyCode.Q) && currentPanelIndex < panels.Length - 1)
        {
            ShowNextPanel();
        }
    }

    void ShowCurrentPanel()
    {
        // 全てのCanvasを非表示にする
        foreach (Canvas panel in panels)
        {
            panel.gameObject.SetActive(false);
        }

        // 現在のCanvasを表示
        if (currentPanelIndex < panels.Length)
        {
            panels[currentPanelIndex].gameObject.SetActive(true);
        }
    }

    void ShowNextPanel()
    {
        // 現在のCanvasを非表示にする
        if (currentPanelIndex < panels.Length)
        {
            panels[currentPanelIndex].gameObject.SetActive(false);
        }

        // インデックスを進める
        currentPanelIndex++;

        // 新しいCanvasを表示
        ShowCurrentPanel();
    }
    // "Countdown"シーンをロードするメソッド
    void LoadCountdownScene()
    {
        SceneManager.LoadScene("startButton"); // シーン名で指定してロード
    }
}
