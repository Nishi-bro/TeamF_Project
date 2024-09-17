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
    public AudioClip GoodComunicationClip;  // 再生するSEを指定
    private AudioSource audioSource;
    public int satisfaction = 25; // 現在の満足度
    public PanelSetup[] panelSetups;
    // 満足度上昇時に浮き上がるテキスト管理
    public float moveSpeed = 2f;  // 上昇速度
    public float fadeTime = 1f;   // フェードアウトまでの時間

    //public GameObject floatingTextPrefab;  // フローティングテキスト用のPrefab
    //public Transform canvasTransform;
    //public Text FloatingText;
    public int UpdownScore;




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
    private void OnLeftButtonPressed(PanelSetup setup) // Qボタン
    {
        if (setup.monitorName == "NebouMonitor" || setup.monitorName == "KakuninMonitor" || setup.monitorName == "EigaMonitor" || setup.monitorName == "FasionMonitor")
        {
            IncreaseSatisfaction(setup); // 満足度を上昇させる処理
            if (GoodComunicationClip != null)
            {
                GetComponent<AudioSource>().Play();

            }
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


    private void OnRightButtonPressed(PanelSetup setup) // Eボタン
    {
        if (setup.monitorName == "CookMonitor" || setup.monitorName == "KouteiMonitor" || setup.monitorName == "AmuseMonitor" || setup.monitorName == "QuestionMonitor")
        {
            IncreaseSatisfaction(setup); // 満足度を上昇させる処理
            if (GoodComunicationClip != null)
            {
                GetComponent<AudioSource>().Play();

            }
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
        UpdownScore = 15;
        satisfaction = satisfaction + UpdownScore;
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
        UpdownScore = -5;
        satisfaction = satisfaction + UpdownScore;
        satisfaction = Mathf.Clamp(satisfaction, 0, 100);
        setup.satisfactionText.text = "彼女満足度:" + satisfaction.ToString() + "%";
        UpdateColor(setup);
    }

    public void IgnoreSatisfactionOnDefaultMonitor()
    {
        UpdownScore = -10;
        satisfaction = satisfaction + UpdownScore;
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

    //public IEnumerator FadeIn()
    //{
    //    FloatingText.text = UpdownScore + "%";
    //    while (true)
    //    {
    //        for (int i = 0; i < 255; i++)
    //        {
    //            FloatingText.color = FloatingText.color + new Color32(0, 0, 0, 1);
    //            yield return new WaitForSeconds(0.01f);
    //        }
    //    }

    //}
}
