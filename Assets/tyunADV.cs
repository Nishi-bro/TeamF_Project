using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理のために必要
using UnityEngine.UI;

public class tyunADV : MonoBehaviour
{
    public Canvas[] panels; // 切り替えるCanvas（背景やキャラクターなど）の配列
    public Button loadCountdownButton_A; // "Countdown"シーンをロードするボタン
    private int currentPanelIndex_A = 0; // 現在表示しているCanvasのインデックス

    public AudioSource seAudioSource; // SEを再生するためのAudioSource
    public AudioClip panel4SE; // 4つ目のCanvasが表示された時に鳴らすSE

    void Start()
    {
        // 最初のCanvasのみ表示
        ShowCurrentPanel_A();

        // ボタンが設定されていれば、クリック時にシーンをロードするリスナーを追加
        if (loadCountdownButton_A != null)
        {
            loadCountdownButton_A.onClick.AddListener(LoadCountdownScene_A);

        }
    }

    void Update()
    {
        // Qキーが押されたら次のCanvasを表示
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentPanelIndex_A != 5)
            {
                ShowNextPanel_A();
            } else
            {
                LoadCountdownScene_A();
            }

            
        }
    }

    void ShowCurrentPanel_A()
    {
        // 全てのCanvasを非表示にする
        foreach (Canvas panel in panels)
        {
            panel.gameObject.SetActive(false);
        }

        // 現在のCanvasを表示
        if (currentPanelIndex_A < panels.Length)
        {
            panels[currentPanelIndex_A].gameObject.SetActive(true);
        }
    }

    void ShowNextPanel_A()
    {
        // 現在のCanvasを非表示にする
        if (currentPanelIndex_A < panels.Length)
        {
            panels[currentPanelIndex_A].gameObject.SetActive(false);
        }
        // 4つ目のCanvasが表示された時にSEを再生
        if (currentPanelIndex_A == 3 && seAudioSource != null && panel4SE != null)
        {
            seAudioSource.PlayOneShot(panel4SE);
        }

        // インデックスを進める
        currentPanelIndex_A++;


        // 新しいCanvasを表示
        ShowCurrentPanel_A();
    }
    // "Countdown"シーンをロードするメソッド
    void LoadCountdownScene_A()
    {
        SceneManager.LoadScene("Countdawn"); // シーン名で指定してロード
    }
}
