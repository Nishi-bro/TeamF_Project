using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public Image image;  // UIのImageコンポーネント
    public Sprite newSprite;  // 表示する新しいスプライト

    void Start()
    {
        // Imageコンポーネントに新しいスプライトを設定
        if (image != null && newSprite != null)
        {
            image.sprite = newSprite;
        }
    }
}