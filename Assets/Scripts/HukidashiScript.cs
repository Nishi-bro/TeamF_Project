//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//public class HukidashiScript : MonoBehaviour
//{
//    public SpeechBubble speechBubble; // 吹き出しを制御するスクリプト
//    public string message = "Rockに当たった！"; // 吹き出しに表示するメッセージ
//    private bool isDisplaying = false; // 吹き出しが表示中かどうかのフラグ

//    void OnTriggerEnter(Collider other)
//    {
//        // 衝突したオブジェクトが"Rock"タグを持っているかチェック
//        if (other.CompareTag("Rock") && !isDisplaying)
//        {
//            isDisplaying = true;
//            // 吹き出しを表示
//            speechBubble.ShowBubble(message);
//            // 5秒後に吹き出しを非表示にする
//            Invoke("HideSpeechBubble", 5.0f);
//        }
//    }

//    void HideSpeechBubble()
//    {
//        // 吹き出しを非表示にして、表示フラグをリセット
//        speechBubble.HideBubble();
//        isDisplaying = false;
//    }
//}