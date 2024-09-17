using UnityEngine;

public class GifController : MonoBehaviour
{
    void Start()
    {
        // 第一引数にAssets/StreamingAssetsに置いた画像か、URLを指定。
        // 第二引数は自動再生フラグで、デフォルトがtrueなので指定しなくてもOK
        GetComponent<UniGifImage>().SetGifFromUrl("003_run_3.gif", true);
    }
}

