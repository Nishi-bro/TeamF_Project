using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BallMove : MonoBehaviour
{
    private float Timeman = 180;
    public Text TimeText;
    public float span = 3f;
    [SerializeField]
    private float _speed = 20f;  // 左右に動くスピード
    [SerializeField]
    private float _runspeed = 10f;  // RUNの速さ

    private Rigidbody _rigidbody;
    private Vector3 _currentVelocity;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(DecreaseScore());
        // 時間制限処理
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
        // RUN処理 横移動処理と分けてます
        Vector3 forwardMovement = transform.forward * _runspeed * Time.deltaTime;

        // 現在の速度を取得
        _currentVelocity = _rigidbody.velocity;
        // 前進の移動処理を行う
        transform.position += forwardMovement;

        // Rigidbodyの速度を更新
        _rigidbody.velocity = new Vector3(_currentVelocity.x, _rigidbody.velocity.y, _runspeed);

    }
    private IEnumerator DecreaseScore()
    {
        while (Timeman > 0)  // 時間が0より大きい間
        {
            Timeman--;  // スコアを1減らす
            SetCountText();  
            yield return new WaitForSeconds(1f);  // 1秒待つ
            //＝1秒ごとにスコアを1減らす
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
        // 衝突したかどうかのチェック、記述不要
    }
    void OnTriggerEnter(Collider other)
    {
        // ぶつかったオブジェクトが岩だった場合
        if (other.gameObject.CompareTag("Rock"))
        {
            // 岩を非表示にします（しなくてもいいかも）
            other.gameObject.SetActive(false);

            // 時間減らします
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
