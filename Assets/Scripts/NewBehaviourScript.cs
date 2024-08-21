using UnityEngine;

public class AutoRun : MonoBehaviour
{
    public Animator animator; // Animatorコンポーネント
    public float runSpeed = 5.0f; // 移動速度

    void Start()
    {
        // アニメーションの再生
        animator.Play("Run"); // "Run"はアニメーションクリップの名前です。
    }

    void Update()
    {
        // キャラクターを前に移動
        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
    }
}