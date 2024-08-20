using UnityEngine;
using UnityEngine.UI;

public class CheckClickEvent : MonoBehaviour
{
    //[SerializeField]
    //private Slider hpSlider;

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトがプレイヤーであるかを確認
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Rock"))//岩なら
            {
                Debug.Log("ダメージ");
            }
        }
    }

}