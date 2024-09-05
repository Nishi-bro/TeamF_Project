using UnityEngine;

public class ManageSmapho : MonoBehaviour
{
    public GameObject DefaultMonitor; // Canvasオブジェクト（彼女からの通信）
    public GameObject CookMonitor; // Canvasオブジェクト
    public SatisfyManager satisfyManager; // SatisfyManager の参照
    private float monitorDisplayTime = 0f; // Monitorが表示された時刻を記録
    private bool monitorShown = false; // Monitorが表示されたかどうか

    void Start()
    {
        // satisfyManager を自動的にシーンから取得
        if (satisfyManager == null)
        {
            satisfyManager = FindObjectOfType<SatisfyManager>();
            if (satisfyManager == null)
            {
                Debug.LogError("SatisfyManager がシーン内に存在しません。");
            }
        }

        // 5秒後にviewMonitorメソッドを実行
        Invoke("viewMonitor", 5f);
    }

    void Update()
    {
        if (DefaultMonitor != null && DefaultMonitor.activeSelf)
        {
            if (!monitorShown) // Monitorが新たに表示された場合
            {
                monitorDisplayTime = Time.time; // 現在の時間を記録
                monitorShown = true; // Monitorが表示されたとマーク
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // DefaultMonitorが表示されている場合にのみExtraMonitorメソッドを実行
            if (DefaultMonitor != null && DefaultMonitor.activeSelf)
            {
                // 即時にExtraMonitorメソッドを実行
                ExtraMonitor();
                HideDefaultMonitor();
                monitorShown = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (DefaultMonitor != null && DefaultMonitor.activeSelf)
            {
                // 満足度を減少させる
                satisfyManager.IgnoreSatisfactionOnDefaultMonitor();

                // DefaultMonitorを非表示にする
                HideDefaultMonitor();
                Invoke("viewMonitor", 5f);
                monitorShown = false;
            }
        }

        bool hasSecondsPassed = monitorShown && (Time.time - monitorDisplayTime >= 3f);

        if (hasSecondsPassed)
        {
            if (DefaultMonitor != null && DefaultMonitor.activeSelf)
            {
                // 3秒経過後に満足度を減少させ、モニターを隠す
                satisfyManager.IgnoreSatisfactionOnDefaultMonitor();

                // DefaultMonitorを非表示にする
                HideDefaultMonitor();
                monitorShown = false; // Monitorが非表示になるのでリセット
                Invoke("viewMonitor", 5f);
            }

        }
        

    }

    void viewMonitor()
    {
        // DefaultMonitor Canvasを表示
        if (DefaultMonitor != null)
        {
            DefaultMonitor.SetActive(true);
        }
    }

    void HideDefaultMonitor()
    {
        // DefaultMonitorを非表示にする
        if (DefaultMonitor != null)
        {
            DefaultMonitor.SetActive(false);
        }
    }

    void ExtraMonitor()
    {
        // CookMonitor Canvasを表示
        if (CookMonitor != null)
        {
            CookMonitor.SetActive(true);
        }

        // 5秒後に再びDefaultMonitorを表示
        Invoke("viewMonitor", 5f);
    }
}
