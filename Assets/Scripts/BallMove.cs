using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallMove : MonoBehaviour
{
    private int ManageTransform = 0;
    private bool isMoving = false;
    public float Timerag = 0.2f; // 移動にかかる時間 

    public float Timeman = 180;
    public Text TimeText;
    public float span = 3f;

    public GameObject Floor1; // Street1のプレハブ
    public GameObject Floor2; // Street2のプレハブ
    private int nextSpawnZ = 50; // 次に生成するZ位置の初期値


    //[SerializeField]
    //private float _speed = 20f;  // 左右に動くスピード
    [SerializeField]
    private float _runspeed = 10f;  // RUNの速さ

    private Rigidbody _rigidbody;
    private Vector3 _currentVelocity;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(DecreaseScore());
        // 時間制限処理
        TimeText.text = "Time: " + Timeman.ToString();
    }

    private void Update()
    {
        if (isMoving)
        {
            return; // 移動中は新たな入力を無視
        }
        // 左キーを押した時の動き
        if (Input.GetKeyDown(KeyCode.A) && ManageTransform > -1)
        {
            StartCoroutine(MovePlayer(-transform.right)); // 左に動くコルーチンを開始
            ManageTransform -= 1;
        }

        // 右キーを押した時の動き
        if (Input.GetKeyDown(KeyCode.D) && ManageTransform < 1)
        {
            StartCoroutine(MovePlayer(transform.right)); // 左に動くコルーチンを開始
            ManageTransform += 1;
        }
        // RUN処理 横移動処理と分けてます
        Vector3 forwardMovement = transform.forward * _runspeed * Time.deltaTime;

        // 現在の速度を取得
        _currentVelocity = _rigidbody.velocity;
        // 前進の移動処理を行う
        transform.position += forwardMovement;

        // Rigidbodyの速度を更新
        _rigidbody.velocity = new Vector3(_currentVelocity.x, _rigidbody.velocity.y, _runspeed);

        if (transform.position.z >= nextSpawnZ)
        {
            SpawnStreet(); // ストリートを生成
            nextSpawnZ += 100; // 次の生成位置を更新
        }
    }

    void SpawnStreet()
    {
        // ランダムにStreet1またはStreet2を選択
        GameObject streetPrefab = Random.value > 0.5f ? Floor1 : Floor2;

        // 新しいストリートを生成
        Vector3 spawnPosition = new Vector3(0, 0, nextSpawnZ + 100); // 生成位置を設定
        Instantiate(streetPrefab, spawnPosition, Quaternion.identity); // プレハブを生成
    }

    //コルーチン
    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;  // 移動中フラグをオン
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction * 3;

        float elapsedTime = 0;

        while (elapsedTime < Timerag)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / Timerag);
            elapsedTime += Time.deltaTime;
            yield return null; // 次のフレームまで待機
        }
        transform.position = targetPosition;

        isMoving = false;  // 移動完了
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
        //Clear画面の表示
        if (other.gameObject.CompareTag("Finish"))
        {
            SceneManager.LoadScene("ClearScene");
        }
    }
    void SetCountText()
    {
        if (TimeText != null)
        {
            TimeText.text = "Time: " + Timeman.ToString();
        }
    }
}
