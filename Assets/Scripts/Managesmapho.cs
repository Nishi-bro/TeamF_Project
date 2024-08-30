using UnityEngine;

public class ManageSmapho : MonoBehaviour
{
    public GameObject DefaultMonitor; // Canvasオブジェクトを割り当てる
    public GameObject CookMonitor; // Canvasオブジェクトを割り当てる

    void Start()
    {
        // 5秒後にShowCanvasメソッドを実行
        Invoke("viewMonitor", 5f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 即時にExtraMonitorメソッドを実行
            Invoke("ExtraMonitor", 0f);
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

    void ExtraMonitor()
    {
        // CookMonitor Canvasを表示
        if (CookMonitor != null)
        {
            CookMonitor.SetActive(true);
        }
    }
}
