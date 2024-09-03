using UnityEngine;
using UnityEngine.UI;

public class SatisfyManager : MonoBehaviour
{
    [System.Serializable]
    public class PanelSetup
    {
        public GameObject panel; // APanel または BPanel
        public Button LeftButton; // 左のボタン
        public Button RightButton; // 右のボタン
        public Text satisfactionText; // 満足度を表示するテキスト
        public int satisfaction; // 現在の満足度
        public string monitorName; // モニター名（例：CookMonitor）
    }

    public PanelSetup[] panelSetups;

    void Start()
    {
        foreach (PanelSetup setup in panelSetups)
        {
            if (setup.LeftButton != null)
            {
                // 引数なしで呼び出せるメソッドを設定
                setup.LeftButton.onClick.AddListener(() => OnLeftButtonPressed());
            }
            else
            {
                Debug.LogWarning("LeftButton is not assigned in PanelSetup.");
            }

            if (setup.RightButton != null)
            {
                // 引数なしで呼び出せるメソッドを設定
                setup.RightButton.onClick.AddListener(() => OnRightButtonPressed());
            }
            else
            {
                Debug.LogWarning("RightButton is not assigned in PanelSetup.");
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (PanelSetup setup in panelSetups)
            {
                if (setup.panel.activeSelf) // アクティブなパネルがある場合
                {
                    OnLeftButtonPressed(); // 左ボタンのクリックイベントを手動でトリガー
                    break; // 一つのパネルのみ処理する
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (PanelSetup setup in panelSetups)
            {
                if (setup.panel.activeSelf) // アクティブなパネルがある場合
                {
                    OnRightButtonPressed(); // 右ボタンのクリックイベントを手動でトリガー
                    break; // 一つのパネルのみ処理する
                }
            }
        }
    }

    void OnRightButtonPressed()
    {
        // アクティブなパネルを取得し、処理を実行
        foreach (PanelSetup setup in panelSetups)
        {
            if (setup.panel.activeSelf) // パネルが表示されている時のみ処理
            {
                setup.panel.SetActive(false);
                break; // 処理は1つのパネルに対してのみ実行
            }
        }
    }

    void OnLeftButtonPressed()
    {
        foreach (PanelSetup setup in panelSetups)
        {
            if (setup.monitorName == "CookMonitor")
            {
                IncreaseSatisfaction(setup); // 満足度を上昇させる処理
            }

            if (setup.panel.activeSelf) // パネルが表示されている時のみ消す処理
            {
                setup.panel.SetActive(false); // パネルを消す処理
                break; // 一つのパネルのみ処理する
            }
        }
    }

    void IncreaseSatisfaction(PanelSetup setup)
    {
        if (setup.panel.activeSelf) // パネルが表示されている時のみ処理する
        {
            setup.satisfaction += 15;
            setup.satisfaction = Mathf.Clamp(setup.satisfaction, 0, 100);
            setup.satisfactionText.text = setup.satisfaction.ToString() + "%";        }
    }


}
