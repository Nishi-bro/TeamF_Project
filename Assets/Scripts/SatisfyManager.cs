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




    private void OnRightButtonPressed(PanelSetup setup)//E button
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

    private void OnLeftButtonPressed(PanelSetup setup)// Q button
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
        // 満足度の更新があった場合、表示も更新する
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

                satisfaction = Mathf.Clamp(satisfaction, 0, 100); // 満足度の範囲を制限
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
        // 満足度の更新があった場合、表示も更新する
        setup.satisfactionText.text = "彼女満足度:" + satisfaction.ToString() + "%";
        UpdateColor(setup);
    }

    public void IgnoreSatisfaction(PanelSetup setup) //Managesmaphoより呼び出し
    {
        satisfaction -= 10;
        satisfaction = Mathf.Clamp(satisfaction, 0, 100);
        // 満足度の更新があった場合、表示も更新する
        setup.satisfactionText.text = "彼女満足度:" + satisfaction.ToString() + "%";
        UpdateColor(setup);
    }

    void UpdateColor(PanelSetup setup) // 満足度で色を変える
    {
        if (satisfaction >= 70)
        {
            float r = 255f / 255f;
            float g = 0f / 255f;
            float b = 0f / 255f;//RGB(255, 0, 0)
            setup.satisfactionText.color = new Color(r, g, b);
        }
        else if (satisfaction >= 50)
        {
            float r = 200f / 255f;
            float g = 0f / 255f;
            float b = 150f / 255f;//RGB(255, 0, 0)
            setup.satisfactionText.color = new Color(r, g, b);
        }
        else
        {
            float r = 0f;
            float g = 0f;
            float b = 1f;//RGB(255, 0, 0)
            setup.satisfactionText.color = new Color(r, g, b);
        }
    }
}