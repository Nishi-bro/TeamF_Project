using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BallMove : MonoBehaviour
{
    private int ManageTransform = 0;
    private bool isMoving = false;
    public float Timerag = 0.2f; // 移動にかかる時間の関数
    private bool slipTriger = false;

    public float ChangeRotation1 = 420f; //420fでx軸方向に回転
    private bool hasChangedDirection = false; // Z座標が420に達したかどうかを記録するフラグ
    public float ChangeRotation2 = 1094f; //1096fでx軸方向に回転
    private bool hasChangedDirection2nd = false; // Z座標が420に達したかどうかを記録するフラグ
    public float ChangeRotation3 = 1482f; //1282fでx軸方向に回転
    private bool hasChangedDirection3rd = false; // Z座標が420に達したかどうかを記録するフラグ

    public float RimitTime = 180;
    public Text TimeText; // 時間管理に使う関数

    public Text DecTimeText;
    public GameObject DecTimePanel;
    private Coroutine scoreCoroutine; // コルーチンの参照を保持する（時間管理に使う）

    public GameObject Rock; // 岩のプレハブ
    public GameObject Rock90; // 90度回転した岩のプレハブ
    public GameObject Trash; // ゴミ袋のプレハブ
    public GameObject Trash90; // 90度回転したゴミ袋のプレハブ
    public GameObject Bicyclecle; // 自転車のプレハブ
    public GameObject Bicyclecle90; // 90度回転した自転車のプレハブ

    public GameObject Heart; // ハートのプレハブ
    private bool LeftBarrier, CenterBarrier, RightBarrier;
    private int nextSpawnZ = 30; // 次に生成するZ位置の初期値
    private int nextSpawnX = -20; // 次に生成するX位置の初期値
    System.Random random_man = new System.Random(); // 障害物のランダム生成に使う
    private int readyBarrier = 0;

    public Image speechBubble; // 吹き出しのImageコンポーネント
    public Sprite damageSprite; // 吹き出しで表示するスプライト
    public Image speechBubble2; // 吹き出しのImageコンポーネント
    public Sprite damageSprite2; // 吹き出しで表示するスプライト
    private bool isDisplaying = false; // 吹き出しが表示中かどうかのフラグ

    public Animator animator; // アニメーションのAnimatorコンポーネント

    public string fallAnimation = "Fall"; // 転倒するアニメーションの名前
    public string getUpAnimation = "GetUp"; // 起き上がるアニメーションの名前

    public Transform playerTransform; // キャラクターの位置を参照するためのTransform
    public Text positionText;         // 表示するText UI


    private SatisfyManager SaManager;

    [SerializeField]
    private Slider hpSlider;
    private float hpmanage = 10; // HP管理に使う
    private float damageAmount;
    public GameObject HpPanel; // HPパネル oya
    public GameObject Heartman; //kodomo
    public Vector2 largeSize = new Vector2(700, 120); // 一時的に大きくするサイズ
    public Vector2 originalSize = new Vector2(352, 70); // 元のパネルサイズ

    private int decreaseTime;


    public AudioClip seClip;  // 再生するSEを指定
    
    private AudioSource audioSource;  // AudioSourceを参照

    [SerializeField]
    private float _runspeed = 10f; // RUNの速さ

    private Rigidbody _rigidbody; // 物理判定に使う

    private Vector3 moveDirection = Vector3.forward; // デフォルトの進行方向

    private void Start()
    {

        animator.Play("FastRun1", 0); // アニメーションクリップの名前

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        TimeText.text = "Time: " + RimitTime.ToString();

        // 吹き出しを非表示にする
        speechBubble.enabled = false;
        speechBubble2.enabled = false;

        audioSource = GetComponent<AudioSource>();

        // もしaudioSourceがない場合、エラーを防ぐためにコンポーネントを自動で追加
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // AudioSourceの基本設定
        audioSource.playOnAwake = false;  // 再生を自動では行わないようにする

    }

    private void Update()
    {
        if (transform.position.z >= ChangeRotation1 && !hasChangedDirection)
        {
            hasChangedDirection = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            // 進行方向をX軸に変更
            moveDirection = Vector3.right;
            // 右方向に回転させる
            transform.rotation = Quaternion.Euler(0, 90, 0); // Y軸方向に90度回転
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            ManageTransform = 0;
        }
        if (transform.position.x >= ChangeRotation2 && !hasChangedDirection2nd)
        {
            hasChangedDirection2nd = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            // 進行方向をZ軸に変更
            moveDirection = Vector3.forward;
            // 左方向に回転させる
            transform.rotation = Quaternion.Euler(0, 0, 0); // Y軸方向に-90度回転
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            ManageTransform = 0;
        }
        if (transform.position.z >= ChangeRotation3 && !hasChangedDirection3rd)
        {
            hasChangedDirection3rd = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            // 進行方向をZ軸に変更
            moveDirection = Vector3.right;
            // 左方向に回転させる
            transform.rotation = Quaternion.Euler(0, 90, 0); // Y軸方向に90度回転
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            ManageTransform = 0;
        }

        if (!isMoving && !slipTriger)
        {
            if (Input.GetKeyDown(KeyCode.A) && ManageTransform > -1)
            {
                StartCoroutine(MovePlayer(-transform.right)); // 左に動くコルーチンを開始
                ManageTransform -= 1;
            }

            if (Input.GetKeyDown(KeyCode.K) && ManageTransform < 1)
            {
                StartCoroutine(MovePlayer(transform.right)); // 右に動くコルーチンを開始
                ManageTransform += 1;
            }
        }

        if (!slipTriger)
        {
            MoveForward();
        }

        if (transform.position.z >= nextSpawnZ && transform.position.x <= 30)
        {
            SpawnBarrier1st(); // 障害物を生成する処理
            nextSpawnZ += 35; // 次の生成位置を更新
            Debug.Log("1");
        }
        if (transform.position.x >= nextSpawnX)
        {
            SpawnBarrier2nd(); // 障害物を生成する処理
            nextSpawnX += 30;
            Debug.Log("2");

        }
        if (transform.position.z >= nextSpawnZ)
        {
            SpawnBarrier3rd(); // 障害物を生成する処理
            nextSpawnZ += 25; // 次の生成位置を更新
            Debug.Log("3");
        }


        if (hpmanage <= 0)
        {
            SceneManager.LoadScene("GameOverscene");
        }
        if (RimitTime <= 0)
        {
            SceneManager.LoadScene("GameOverTime");
        }

        // slipTrigerがfalseで、コルーチンが動いていない場合、コルーチンを開始
        if (!slipTriger && scoreCoroutine == null)
        {
            scoreCoroutine = StartCoroutine(DecreaseScore());
        }

        // slipTrigerがtrueになったらコルーチンを停止し、参照をnullにする
        if (slipTriger && scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
            scoreCoroutine = null;
        }

        GameObject obj = GameObject.Find("SatisfyManager"); //SatisfyManagerっていうオブジェクトを探す
        SaManager = obj.GetComponent<SatisfyManager>(); //付いているスクリプトを取得

        // キャラクターのX座標とZ座標を取得
        float playerX = playerTransform.position.x;
        float playerZ = playerTransform.position.z;

        // テキストの内容を更新
        positionText.text = "進行度: " + Mathf.RoundToInt((playerX + playerZ) / 2680 * 100) + "%";

    }



    private void LateUpdate()
    {
        // Y軸の回転のみを維持し、X軸とZ軸の回転をリセットする
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        // Y座標を1に固定
        Vector3 position = transform.position;
        position.y = 1f;
        transform.position = position;
    }




    private void MoveForward()
    {
        Vector3 forwardMovement = moveDirection * _runspeed * Time.deltaTime;
        _rigidbody.MovePosition(_rigidbody.position + forwardMovement);



        // 移動方向に応じた回転を行う
        if (moveDirection == Vector3.right)
        {
            // X軸方向に移動中はX方向に向ける
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            // Z軸方向に移動中はZ方向に向ける
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        // Y座標を固定する
        //Vector3 position = transform.position;

        //transform.position = position;
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;

        Vector3 lateralMove = direction.normalized * 3; // 横方向に3ユニット移動
        Vector3 forwardMove = transform.forward.normalized * 3; // 前方に3ユニット移動

        Vector3 targetPosition = startPosition + lateralMove + forwardMove;

        targetPosition.y = 1f;

        float consumeTime = 0f;
        while (consumeTime < Timerag)
        {
            consumeTime += Time.deltaTime;
            _rigidbody.MovePosition(Vector3.Lerp(startPosition, targetPosition, Mathf.Clamp01(consumeTime / Timerag)));
            targetPosition.y = 1f;
            yield return null;
        }
        _rigidbody.MovePosition(targetPosition);

        isMoving = false;
    }

    private IEnumerator DecreaseScore()
    {
        while (RimitTime > 0 && !slipTriger) // slipTrigerがtrueならループを抜ける
        {
            RimitTime--;
            SetCountText();
            yield return new WaitForSeconds(1f);
        }
        scoreCoroutine = null;

    }




    void SpawnBarrier1st()
    {
        LeftBarrier = Random.value < 0.6f;
        CenterBarrier = Random.value < 0.6f;
        RightBarrier = Random.value < 0.6f;

        if (LeftBarrier && CenterBarrier && RightBarrier)
        {
            int? except = random_man.Next(1, 3);
            if (except == 1)
                LeftBarrier = false;
            else if (except == 2)
                CenterBarrier = false;
            else
                RightBarrier = false;
        }
        if (ChangeRotation1 > transform.position.z + 150) //420までの障害物生成
        {
            if (LeftBarrier)
            {
                Instantiate(Rock, new Vector3(-3.7f, 0, nextSpawnZ + 120), Quaternion.identity);
            }
            if (CenterBarrier)
            {
                Instantiate(Rock, new Vector3(-0.7f, 0, nextSpawnZ + 120), Quaternion.identity);
            }
            if (RightBarrier)
            {
                Instantiate(Rock, new Vector3(2.3f, 0, nextSpawnZ + 120), Quaternion.identity);
            }
            //NewSpawnZ 450 made
        }
    }

    void SpawnBarrier2nd()　// 障害物の設定
    {
        
        LeftBarrier = Random.value < 0.9f;
        CenterBarrier = Random.value < 0.9f;
        RightBarrier = Random.value < 0.9f;

        if (LeftBarrier && CenterBarrier && RightBarrier)
        {
            int? except = random_man.Next(1, 3);
            if (except == 1)
                LeftBarrier = false;
            else if (except == 2)
                CenterBarrier = false;
            else
                RightBarrier = false;
        }
        if (transform.position.x > 0 && transform.position.x < 500)
        {
            if (LeftBarrier)
            {
                Instantiate(Rock90, new Vector3(nextSpawnX + 120, 0, 417), Quaternion.identity);
            }
            if (CenterBarrier)
            {
                Instantiate(Rock90, new Vector3(nextSpawnX + 120, 0, 420), Quaternion.identity);
            }
            if (RightBarrier)
            {
                Instantiate(Rock90, new Vector3(nextSpawnX + 120, 0, 423), Quaternion.identity);
            }
        }
        if (transform.position.x >= 500 && transform.position.x < 990)
        {
            if (LeftBarrier)
            {
                Instantiate(Rock90, new Vector3(nextSpawnX + 120, 0, 417), Quaternion.identity);
            }
            if (CenterBarrier)
            {
                Instantiate(Rock90, new Vector3(nextSpawnX + 120, 0, 420), Quaternion.identity);
            }
            if (RightBarrier)
            {
                Instantiate(Rock90, new Vector3(nextSpawnX + 120, 0, 423), Quaternion.identity);
            }
        }//980notoki newSpawnX 1010
        if (transform.position.x >= 990 && transform.position.x <= 1089)
        {
            if (LeftBarrier)
            {
                Instantiate(Rock90, new Vector3(1091, 0, nextSpawnZ + readyBarrier), Quaternion.identity);
            }
            if (CenterBarrier)
            {
                Instantiate(Rock90, new Vector3(1094, 0, nextSpawnZ + readyBarrier), Quaternion.identity);
            }
            if (RightBarrier)
            {
                Instantiate(Rock90, new Vector3(1097, 0, nextSpawnZ + readyBarrier), Quaternion.identity);
            }
            readyBarrier += 30;//4回呼び出し Z450 480 510 540
            
        }

        
    }


    void SpawnBarrier3rd()
    {
        LeftBarrier = Random.value < 0.9f;
        CenterBarrier = Random.value < 0.9f;
        RightBarrier = Random.value < 0.9f;//出現率

        if (LeftBarrier && CenterBarrier && RightBarrier)
        {
            int? except = random_man.Next(1, 3);
            if (except == 1)
                LeftBarrier = false;
            else if (except == 2)
                CenterBarrier = false;
            else
                RightBarrier = false;
        }
        if (transform.position.z > 425 && ChangeRotation3 > transform.position.z + 140)
        {
            if (LeftBarrier)
            {
                Instantiate(Trash, new Vector3(1091, 2, nextSpawnZ + 130), Quaternion.identity);
            }
            if (CenterBarrier)
            {
                Instantiate(Trash, new Vector3(1094, 2, nextSpawnZ + 130), Quaternion.identity);
            }
            if (RightBarrier)
            {
                Instantiate(Trash, new Vector3(1097, 2, nextSpawnZ + 130), Quaternion.identity);
            }
            // デバッグ用ログ追加

        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rock"))
        {
            other.gameObject.SetActive(false);

            RimitTime -= 5;
            decreaseTime = 5;

            damageAmount = 2;//ハート1個ダメージ 

            SetCountText();

            animator.SetTrigger("Fall");
            animator.speed = 1.5f;

            slipTriger = true;
            _rigidbody.isKinematic = true;//物理法則を一旦切る

            if (seClip != null)
            {
                audioSource.PlayOneShot(seClip);  // 一度だけ再生
            }
            //StartCoroutine(BlinkDamagePanel((int)(damageAmount / 10)));

            StartCoroutine(FlashHPSlider(damageAmount));

            StartCoroutine(PlayAnimations());
        }

        if (other.gameObject.CompareTag("Cars"))
        {
            other.gameObject.SetActive(false);

            RimitTime -= 10;
            decreaseTime = 10;

            damageAmount = 6;//ハート3個ダメージ 

            SetCountText();

            animator.SetTrigger("Fall");
            animator.speed = 1.5f;

            slipTriger = true;
            _rigidbody.isKinematic = true;//物理法則を一旦切る

            if (seClip != null)
            {
                audioSource.PlayOneShot(seClip);  // 一度だけ再生車の音も鳴らしたい
            }

            StartCoroutine(FlashHPSlider(damageAmount));

            StartCoroutine(PlayAnimations());
        }

        if (other.gameObject.CompareTag("Bicycle"))
        {
            other.gameObject.SetActive(false);

            RimitTime -= 2;
            decreaseTime = 5;

            damageAmount = 2;//ハート1個ダメージ 

            SetCountText();

            animator.SetTrigger("Fall");
            animator.speed = 1.5f;

            slipTriger = true;
            _rigidbody.isKinematic = true;//物理法則を一旦切る

            if (seClip != null)
            {
                audioSource.PlayOneShot(seClip);  // 一度だけ再生
            }
            //StartCoroutine(BlinkDamagePanel((int)(damageAmount / 10)));

            StartCoroutine(FlashHPSlider(damageAmount));

            StartCoroutine(PlayAnimations());
        }

        if (other.gameObject.CompareTag("Trash"))
        {
            other.gameObject.SetActive(false);

            RimitTime -= 8;
            decreaseTime = 8;

            damageAmount = 1;//ハート0.5個ダメージ 

            SetCountText();

            animator.SetTrigger("Fall");
            animator.speed = 1.5f;

            slipTriger = true;
            _rigidbody.isKinematic = true;//物理法則を一旦切る

            if (seClip != null)
            {
                audioSource.PlayOneShot(seClip);  // 一度だけ再生
            }
            //StartCoroutine(BlinkDamagePanel((int)(damageAmount / 10)));

            StartCoroutine(FlashHPSlider(damageAmount));

            StartCoroutine(PlayAnimations());
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            if (SaManager.satisfaction <= 60)
            {
                SceneManager.LoadScene("BadEnding");
            }
            else
            {
                SceneManager.LoadScene("GoodEnding");
            }
        }
    }


    private IEnumerator PlayAnimations()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetTrigger("GetUp");
        animator.speed = 1.0f;
        _rigidbody.isKinematic = false;
        if (!isDisplaying)
        {
            StartCoroutine(ShowSpeechBubble());
        }
    }

    private IEnumerator FlashHPSlider(float damageAmount)
    {
        // 現在のHPスライダーの値を取得
        float initialHpValue = hpSlider.value;
        RectTransform panelRectTransform = HpPanel.GetComponent<RectTransform>();
        RectTransform panelRectTransformchild = Heartman.GetComponent<RectTransform>();
        SetCountText();
        DecTimePanel.SetActive(true);
        // 2回表示させる（ダメージ前のHP値を表示）
        for (int i = 0; i < 1; i++)
        {
            
                                                   
            HpPanel.SetActive(false);// パネルを非表示にする
            yield return new WaitForSeconds(0.3f); // 少し待機

            // パネルを再表示して現在のHPを再表示
            HpPanel.SetActive(true);
            panelRectTransform.sizeDelta = largeSize;
            panelRectTransformchild.sizeDelta = largeSize;
            hpSlider.value = initialHpValue;
            yield return new WaitForSeconds(0.3f); // 少し待機

            HpPanel.SetActive(false);// パネルを非表示にする
            yield return new WaitForSeconds(0.3f); // 少し待機

            HpPanel.SetActive(true);
            hpSlider.value = initialHpValue; // 現在のHPを表示
            yield return new WaitForSeconds(0.3f); // 少し待機

            // 再度パネルを非表示にする
            HpPanel.SetActive(false);
                        hpSlider.value = initialHpValue - damageAmount;
            yield return new WaitForSeconds(0.3f); // 少し待機

            // HPを減らした値を表示
            HpPanel.SetActive(true);
            hpSlider.value = initialHpValue - damageAmount;
            yield return new WaitForSeconds(0.3f); // 少し待機

            HpPanel.SetActive(false);
            yield return new WaitForSeconds(0.3f); // 少し待機


        }
        DecTimePanel.SetActive(false);
        // 実際のダメージ反映
        hpmanage -= damageAmount;
        HpPanel.SetActive(true);
        panelRectTransform.sizeDelta = originalSize;
        panelRectTransformchild.sizeDelta = originalSize;
        // スライダーの更新
        SetCountText();
    }

    private IEnumerator ShowSpeechBubble()
    {
        isDisplaying = true;

        speechBubble.sprite = damageSprite;
        speechBubble.enabled = true;
        speechBubble2.sprite = damageSprite2;
        speechBubble2.enabled = true;

        yield return new WaitForSeconds(3.6f);

        speechBubble.enabled = false;
        speechBubble2.enabled = false;
        isDisplaying = false;
        slipTriger = false;
        animator.speed = 1.0f;

    }

    void SetCountText()
    {
        if (!slipTriger)
        {
            if (TimeText != null)
            {
                TimeText.text = "Time: " + RimitTime.ToString();

            }
        }
        else
        {
            TimeText.text = "-" + decreaseTime + "sec";
            DecTimeText.text = "-" + decreaseTime + "sec";
        }
        
        
    }
}
