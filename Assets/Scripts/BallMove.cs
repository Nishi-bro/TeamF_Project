using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BallMove : MonoBehaviour
{
    private float Timeman = 180;
    public Text TimeText;
    public float span = 3f;
    [SerializeField]
    private float _speed = 20f;  // 左右のスピード
    [SerializeField]
    private float _runspeed = 10f;  // 直進の速さ

    private Rigidbody _rigidbody;
    private Vector3 _currentVelocity;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(DecreaseScore());
    }

    private void FixedUpdate()
    {
        // 横移動処理
        Vector3 horizontalVelocity = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalVelocity.x = -_speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontalVelocity.x = _speed;
        }

        // Rigidbodyの物理演算による横移動
        _rigidbody.velocity = new Vector3(horizontalVelocity.x, _rigidbody.velocity.y, _runspeed);
    }

    private void Update()
    {
        // 前進処理を手動で行う（左右の動作と切り離し）
        Vector3 forwardMovement = transform.forward * _runspeed * Time.deltaTime;

        // 現在の速度を取得
        _currentVelocity = _rigidbody.velocity;
        // 前進の移動処理を行う
        transform.position += forwardMovement;

        // Rigidbodyの速度を更新
        _rigidbody.velocity = new Vector3(_currentVelocity.x, _rigidbody.velocity.y, _runspeed);

        // 右下タイムの管理

    }
    private IEnumerator DecreaseScore()
    {
        while (Timeman > 0)  // スコアが0以上の間繰り返す
        {
            Timeman--;  // スコアを1減らす
            SetCountText();  // テキストを更新する
            yield return new WaitForSeconds(1f);  // 1秒待つ
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
    }
    void OnTriggerEnter(Collider other)
    {
        // ぶつかったオブジェクトが岩だった場合
        if (other.gameObject.CompareTag("Rock"))
        {
            // その収集アイテムを非表示にします
            other.gameObject.SetActive(false);

            // 時間消す
            Timeman = Timeman - 5;

            // UI の表示を更新します
            SetCountText();
        }
    }
    void SetCountText()
    {
        // スコアの表示を更新
        TimeText.text = "Time: " + Timeman.ToString();
        
    }
}
