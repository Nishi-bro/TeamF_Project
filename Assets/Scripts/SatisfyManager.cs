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
        public string monitorName; // モニター名（例：CookMonitor)
    }

    public PanelSetup[] panelSetups;

    void Start()
    {
        foreach (PanelSetup setup in panelSetups)
        {
            if (setup.LeftButton != null)
            {
                setup.LeftButton.onClick.AddListener(OnLeftButtonPressed);
            }
            else
            {
                Debug.LogWarning("LeftButton is not assigned for " + setup.monitorName);
            }

            if (setup.RightButton != null)
            {
                setup.RightButton.onClick.AddListener(OnRightButtonPressed);
            }
            else
            {
                Debug.LogWarning("RightButton is not assigned for " + setup.monitorName);
            }

            setup.panel.SetActive(false); // 初期状態では非表示
        }
    }

    void OnLeftButtonPressed()
    {
        foreach (PanelSetup setup in panelSetups)
        {
            if (setup.panel.activeSelf) // パネルが表示されている時のみ処理
            {
                Debug.Log("Left Button Pressed on Panel: " + setup.monitorName);
                if (setup.monitorName == "CookMonitor")
                {
                    IncreaseSatisfaction(setup);
                }
                setup.panel.SetActive(false);
                break; // 一つのパネルのみ処理する
            }
        }
    }

    void OnRightButtonPressed()
    {
        foreach (PanelSetup setup in panelSetups)
        {
            if (setup.panel.activeSelf) // パネルが表示されている時のみ処理
            {
                Debug.Log("Right Button Pressed on Panel: " + setup.monitorName);
                setup.panel.SetActive(false);
                break; // 一つのパネルのみ処理する
            }
        }
    }

    void IncreaseSatisfaction(PanelSetup setup)
    {
        if (setup.panel.activeSelf) // パネルが表示されている時のみ処理
        {
            // 満足度を15%増加
            setup.satisfaction += 15;
            setup.satisfaction = Mathf.Clamp(setup.satisfaction, 0, 100); // 0から100の範囲に制限
            setup.satisfactionText.text = setup.satisfaction.ToString() + "%";
        }
    }
}
