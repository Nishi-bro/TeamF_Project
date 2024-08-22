using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallMove : MonoBehaviour
{
    private int ManageTransform = 0;
    private bool isMoving = false;
    public float Timerag = 0.2f; // 移動にかかる時間の関数 

    public float RimitTime = 180;
    public Text TimeText;//時間管理に使う関数

    public GameObject Rock; //　岩のプレハブ
    public GameObject Heart; // ハートのプレハブ
    private bool _xBarrier, Barrier, xBarrier;
    private int nextSpawnZ = 30; // 次に生成するZ位置の初期値
    System.Random random_man = new System.Random(); //障害物のランダム生成に使う

    
    [SerializeField]
    private Slider hpSlider;
    private float hpmanage = 100;//HP管理に使う


    [SerializeField]
    private float _runspeed = 10f;  // RUNの速さ（

    private Rigidbody _rigidbody;// 物理判定に使う

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(DecreaseScore());
        // 時間制限処理
        TimeText.text = "Time: " + RimitTime.ToString();
    }

    private void Update()
    {
        if(!isMoving)
        {
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
        }
        MoveForward();
        // RUN処理 横移動処理と分けてます

        if (transform.position.z >= nextSpawnZ )
        {
            SpawnBarrier(); // 障害物を生成する処理
            nextSpawnZ += 20; // 次の生成位置を更新
        }
        if(hpmanage <= 0)
        {
            //hp0でシーン遷移
            SceneManager.LoadScene("GameOverscene");
        }
        if (RimitTime <= 0)
        {
            //Time0でシーン遷移
            SceneManager.LoadScene("GameOverTime");
        }
    }

    private void MoveForward()
    {
        Vector3 forwardMovement = Vector3.forward * _runspeed * Time.deltaTime;
        _rigidbody.MovePosition(_rigidbody.position + forwardMovement);
    }//RUNの処理


    //コルーチン
    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;  // 移動中フラグをオン
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction * 3;//横に3移動

        float consumeTime = 0f;
        //移動中はどのキーも干渉しないように
        while (consumeTime < Timerag)
        {
            consumeTime += Time.deltaTime;
            _rigidbody.MovePosition(Vector3.Lerp(startPosition, targetPosition, Mathf.Clamp01(consumeTime / Timerag)));
            yield return null; // 次のフレームまで待機
        }
        _rigidbody.MovePosition(targetPosition);

        isMoving = false;  // 移動完了
    }



    private IEnumerator DecreaseScore()
    {
        while (RimitTime > 0)  // 時間が0より大きい間
        {
            RimitTime--;  // スコアを1減らす
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

        // 新しい障害物を 生成
        if (_xBarrier != false)
        {
            GameObject _xstreetBarrier = Rock;
            Vector3 spawnPosition = new Vector3(-3, 2, nextSpawnZ + 120); // 生成位置を設定
            Instantiate(_xstreetBarrier, spawnPosition, Quaternion.identity); // プレハブを生成
        }
        if (Barrier != false)
        {
            GameObject streetBarrier = Rock;
            Vector3 spawnPosition = new Vector3(0, 2, nextSpawnZ + 120);
            Instantiate(streetBarrier, spawnPosition, Quaternion.identity); // 座標違うが同上
        }
        if (xBarrier != false)
        {
            GameObject xstreetBarrier = Rock;
            Vector3 spawnPosition = new Vector3(3, 2, nextSpawnZ + 120);
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
            RimitTime = RimitTime - 5;

            // ハート1個減らします
            hpSlider.value -= 20;
            hpmanage -= 20;

            // UI の表示を更新します
            SetCountText();
        }
        // ぶつかったオブジェクトが車だった場合　即ゲームオーバー
        if (other.gameObject.CompareTag("Cars"))
        {
            SceneManager.LoadScene("GameOverCar");
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
            TimeText.text = "Time: " + RimitTime.ToString();//右下の時間テキストの更新
        }
    }
}
