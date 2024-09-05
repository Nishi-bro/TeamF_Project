using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SatisfyManager : MonoBehaviour
{
    [System.Serializable]
    public class PanelSetup
    {
        public GameObject panel; // APanel または BPanel
        public Button LeftButton; // 左のボタン
        public Button RightButton; // 右のボタン
        public Text satisfactionText; // 満足度を表示するテキスト
        public string monitorName; // モニター名（例：CookMonitor）
    }

    public int satisfaction = 25; // 現在の満足度
    public PanelSetup[] panelSetups;

    private void Start()
    {
        foreach (PanelSetup setup in panelSetups)
        {
            if (setup.LeftButton != null)
            {
                setup.LeftButton.onClick.AddListener(() => OnLeftButtonPressed(setup));
            }
            else
            {
                Debug.LogWarning("LeftButton is not assigned in PanelSetup.");
            }

            if (setup.RightButton != null)
            {
                setup.RightButton.onClick.AddListener(() => OnRightButtonPressed(setup));
            }
            else
            {
                Debug.LogWarning("RightButton is not assigned in PanelSetup.");
            }
        }

        // 3秒ごとに満足度を更新するコルーチンを開始
        StartCoroutine(UpdateSatisfaction());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (PanelSetup setup in panelSetups)
            {
                if (setup.panel.activeSelf) // アクティブなパネルがある場合
                {
                    OnLeftButtonPressed(setup); // 左ボタンのクリックイベントを手動でトリガー
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
                    OnRightButtonPressed(setup); // 右ボタンのクリックイベントを手動でトリガー
                    break; // 一つのパネルのみ処理する
                }
            }
        }
    }

    private void OnRightButtonPressed(PanelSetup setup) // Eボタン
    {
        if (setup.monitorName == "CookMonitor" || setup.monitorName == "GohanMonitor")
        {
            IncreaseSatisfaction(setup); // 満足度を上昇させる処理
        }
        else
        {
            DecreaseSatisfaction(setup);
        }

        if (setup.panel.activeSelf) // パネルが表示されている時のみ処理
        {
            setup.panel.SetActive(false);
        }
    }

    private void OnLeftButtonPressed(PanelSetup setup) // Qボタン
    {
        if (setup.monitorName == "NebouMonitor")
        {
            IncreaseSatisfaction(setup); // 満足度を上昇させる処理
        }
        else
        {
            DecreaseSatisfaction(setup);
        }

        if (setup.panel.activeSelf) // パネルが表示されている時のみ処理
        {
            setup.panel.SetActive(false);
        }
    }

    private void IncreaseSatisfaction(PanelSetup setup)
    {
        satisfaction += 15;
        satisfaction = Mathf.Clamp(satisfaction, 0, 100);
        setup.satisfactionText.text = "彼女満足度:" + satisfaction.ToString() + "%";
        UpdateColor(setup);
    }

    private IEnumerator UpdateSatisfaction()
    {
        while (true)
        {
            satisfaction -= 1;
            foreach (PanelSetup setup in panelSetups)
            {
                satisfaction = Mathf.Clamp(satisfaction, 0, 100);
                setup.satisfactionText.text = "彼女満足度:" + satisfaction.ToString() + "%";
                UpdateColor(setup);
            }
            yield return new WaitForSeconds(3f);
        }
    }

    private void DecreaseSatisfaction(PanelSetup setup)
    {
        satisfaction -= 5;
        satisfaction = Mathf.Clamp(satisfaction, 0, 100);
        setup.satisfactionText.text = "彼女満足度:" + satisfaction.ToString() + "%";
        UpdateColor(setup);
    }

    public void IgnoreSatisfactionOnDefaultMonitor()
    {
        satisfaction -= 10;
        satisfaction = Mathf.Clamp(satisfaction, 0, 100);

        // すべてのパネルセットアップのテキストを更新
        foreach (PanelSetup setup in panelSetups)
        {
            setup.satisfactionText.text = "彼女満足度:" + satisfaction.ToString() + "%";
            UpdateColor(setup);
        }
    }

    void UpdateColor(PanelSetup setup)
    {
        if (satisfaction >= 70)
        {
            setup.satisfactionText.color = new Color(1f, 0f, 0f); // 赤色
        }
        else if (satisfaction >= 50)
        {
            setup.satisfactionText.color = new Color(0.78f, 0f, 0.59f); // ピンク色
        }
        else
        {
            setup.satisfactionText.color = new Color(0f, 0f, 1f); // 青色
        }
    }
}
