using UnityEngine;

public class ManageSmapho : MonoBehaviour
{
    public GameObject DefaultMonitor; // Canvasオブジェクト（彼女からの通信）
    public GameObject CookMonitor; // Canvasオブジェクト

    void Start()
    {
        // 5秒後にviewMonitorメソッドを実行
        Invoke("viewMonitor", 5f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // DefaultMonitorが表示されている場合にのみExtraMonitorメソッドを実行
            if (DefaultMonitor != null && DefaultMonitor.activeSelf)
            {
                // 即時にExtraMonitorメソッドを実行
                Invoke("ExtraMonitor", 0f);
                Invoke("HideDefaultMonitor", 0.1f);
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
        Invoke("viewMonitor", 5f);
    }
}