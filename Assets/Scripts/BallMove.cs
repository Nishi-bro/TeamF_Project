using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallMove : MonoBehaviour
{
    private float WaitingTime = 1f; // キー入力を無視する時間（秒）
    private float FirstTime = 0f; // 最初に操作を受け付けない時間
    private bool canReceiveInput = false; // 入力を受け付けるかどうか

    private int ManageTransform = 0;
    private bool isMoving = false;
    public float Timerag = 0.2f; // 移動にかかる時間 

    public float Timeman = 180;
    public Text TimeText;
    public float span = 3f;
    System.Random random_man = new System.Random();


    [SerializeField]
    private Slider hpSlider;

    public GameObject Rock; //　岩のプレハブ
    public GameObject Heart; // ハートのプレハブ
    private bool _xBarrier, Barrier, xBarrier;
    private int nextSpawnZ = 30; // 次に生成するZ位置の初期値

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
        if (Input.GetKeyDown(KeyCode.A) && ManageTransform > -1 && canReceiveInput)
        {
            StartCoroutine(MovePlayer(-transform.right)); // 左に動くコルーチンを開始
            ManageTransform -= 1;
        }

        // 右キーを押した時の動き
        if (Input.GetKeyDown(KeyCode.D) && ManageTransform < 1 && canReceiveInput)
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

        FirstTime += Time.deltaTime;
        // 1秒経過したら入力を受け付けるようにする
        if (FirstTime >= WaitingTime)
        {
            canReceiveInput = true;
        }


        if (transform.position.z >= nextSpawnZ )
        {
            SpawnBarrier(); // 障害物を生成
            nextSpawnZ += 20; // 次の生成位置を更新
        }
    }

    //コルーチン
    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;  // 移動中フラグをオン
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction * 3;//横に3移動

        float consumeTime = 0;
        //移動中はどのキーも干渉しないように 
        while (consumeTime < Timerag)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, consumeTime / Timerag);
            consumeTime += Time.deltaTime;
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

    // 以下西田担当、障害物の自動生成,判定部
    void SpawnBarrier()
    {
        // ランダム障害物生成のコード、60％の確率で各レーンに障害物生成 
        _xBarrier = Random.value < 0.6f ? true : false;// x=-3に障害物生成
        Barrier = Random.value < 0.6f ? true : false;  // x=0 に障害物生成
        xBarrier = Random.value < 0.6f ? true : false; // x=3 に障害物生成
        
        // 全てのレーンに障害物が生成された場合
        if (_xBarrier == true && Barrier == true && xBarrier == true)
        {
            int? except;
            except = random_man.Next(1, 3);//1-3 までを抽選する
            if (except == 1)
                _xBarrier = false;
            else if (except == 2)
                Barrier = false;
            else
                xBarrier = false;
            // 出た数字によってレーンの障害物生成を中止
        }
        //streetBarrier xstreetBarrier

        // 新しい障害物を 生成
        if (_xBarrier != false)
        {
            GameObject _xstreetBarrier = Rock;
            Vector3 spawnPosition = new Vector3(-3, 2, nextSpawnZ + 60); // 生成位置を設定
            Instantiate(_xstreetBarrier, spawnPosition, Quaternion.identity); // プレハブを生成
        }
        if (Barrier != false)
        {
            GameObject streetBarrier = Rock;
            Vector3 spawnPosition = new Vector3(0, 2, nextSpawnZ + 60);
            Instantiate(streetBarrier, spawnPosition, Quaternion.identity); // 座標違うが同上
        }
        if (xBarrier != false)
        {
            GameObject xstreetBarrier = Rock;
            Vector3 spawnPosition = new Vector3(3, 2, nextSpawnZ + 60);
            Instantiate(xstreetBarrier, spawnPosition, Quaternion.identity);  // 座標違うが同上
        }
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

            hpSlider.value -= 20; 

            // UI の表示を更新します
            SetCountText();
        }
        // ぶつかったオブジェクトが車だった場合　即ゲームオーバー
        if (other.gameObject.CompareTag("Cars"))
        {
            SceneManager.LoadScene("GameOverscene");
        }
        //ぶつかったオブジェクトがゴールだった場合　Clear画面の表示
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
