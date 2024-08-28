using UnityEngine;

public class Managesmapho : MonoBehaviour
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

            // 5秒後にShowCanvasメソッドを実行
            Invoke("ExtraMonitor", 0f);

        }
        
    }
    void viewMonitor()
    {
        // Canvasを表示
        DefaultMonitor.gameObject.SetActive(true);
    }
    void ExtraMonitor()
    {
        CookMonitor.gameObject.SetActive(true);
    }
}
 