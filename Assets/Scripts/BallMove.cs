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

    private bool isLeft = false;
    private bool hasChangedDirection = false; // Z座標が420に達したかどうかを記録するフラグ

    public float RimitTime = 180;
    public Text TimeText; // 時間管理に使う関数

    public GameObject Rock; // 岩のプレハブ
    public GameObject Heart; // ハートのプレハブ
    private bool _xBarrier, Barrier, xBarrier;
    private int nextSpawnZ = 30; // 次に生成するZ位置の初期値
    System.Random random_man = new System.Random(); // 障害物のランダム生成に使う

    public Image speechBubble; // 吹き出しのImageコンポーネント
    public Sprite damageSprite; // 吹き出しで表示するスプライト
    public Image speechBubble2; // 吹き出しのImageコンポーネント
    public Sprite damageSprite2; // 吹き出しで表示するスプライト
    private bool isDisplaying = false; // 吹き出しが表示中かどうかのフラグ

    public Animator animator; // アニメーションのAnimatorコンポーネント

    public string fallAnimation = "Fall"; // 転倒するアニメーションの名前
    public string getUpAnimation = "GetUp"; // 起き上がるアニメーションの名前

    [SerializeField]
    private Slider hpSlider;
    private float hpmanage = 100; // HP管理に使う

    [SerializeField]
    private float _runspeed = 10f; // RUNの速さ

    private Rigidbody _rigidbody; // 物理判定に使う

    private Vector3 moveDirection = Vector3.forward; // デフォルトの進行方向

    private void Start()
    {

        animator.Play("FastRun1", 0); // アニメーションクリップの名前

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY; // Y軸方向の移動を固定

        StartCoroutine(DecreaseScore());
        TimeText.text = "Time: " + RimitTime.ToString();

        // 吹き出しを非表示にする
        speechBubble.enabled = false;
        speechBubble2.enabled = false;
    }

    private void Update()
    {
        // Y座標を固定する
        Vector3 position = transform.position;
        position.y = 1f;
        transform.position = position;
        if (transform.position.z >= 420f && !hasChangedDirection)
        {
            hasChangedDirection = true;
            // 進行方向をX軸に変更
            moveDirection = Vector3.right;
            // 右方向に回転させる
            transform.rotation = Quaternion.Euler(0, 90, 0); // Y軸方向に90度回転
        }

        if (!isMoving && !slipTriger)
        {
            if (Input.GetKeyDown(KeyCode.A) && ManageTransform > -1)
            {
                StartCoroutine(MovePlayer(-transform.right)); // 左に動くコルーチンを開始
                ManageTransform -= 1;
            }

            if (Input.GetKeyDown(KeyCode.D) && ManageTransform < 1)
            {
                StartCoroutine(MovePlayer(transform.right)); // 右に動くコルーチンを開始
                ManageTransform += 1;
            }
        }

        if (!slipTriger)
        {
            MoveForward();
        }

        if (transform.position.z >= nextSpawnZ)
        {
            SpawnBarrier(); // 障害物を生成する処理
            nextSpawnZ += 20; // 次の生成位置を更新
        }

        if (hpmanage <= 0)
        {
            SceneManager.LoadScene("GameOverscene");
        }
        if (RimitTime <= 0)
        {
            SceneManager.LoadScene("GameOverTime");
        }
    }
    private void LateUpdate()
    {
        // Y軸の回転のみを維持し、X軸とZ軸の回転をリセットする
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
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
        Vector3 position = transform.position;
        position.y = 1f;
        transform.position = position;
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;

        Vector3 lateralMove = direction.normalized * 3; // 横方向に3ユニット移動
        Vector3 forwardMove = transform.forward.normalized * 3; // 前方に3ユニット移動

        Vector3 targetPosition = startPosition + lateralMove + forwardMove;

        float consumeTime = 0f;
        while (consumeTime < Timerag)
        {
            consumeTime += Time.deltaTime;
            _rigidbody.MovePosition(Vector3.Lerp(startPosition, targetPosition, Mathf.Clamp01(consumeTime / Timerag)));
            yield return null;
        }
        _rigidbody.MovePosition(targetPosition);

        isMoving = false;
    }

    private IEnumerator DecreaseScore()
    {
        while (RimitTime > 0)
        {
            RimitTime--;
            SetCountText();
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnBarrier()
    {
        _xBarrier = Random.value < 0.6f;
        Barrier = Random.value < 0.6f;
        xBarrier = Random.value < 0.6f;

        if (_xBarrier && Barrier && xBarrier)
        {
            int? except = random_man.Next(1, 3);
            if (except == 1)
                _xBarrier = false;
            else if (except == 2)
                Barrier = false;
            else
                xBarrier = false;
        }

        if (_xBarrier)
        {
            Instantiate(Rock, new Vector3(-3, 0, nextSpawnZ + 120), Quaternion.identity);
        }
        if (Barrier)
        {
            Instantiate(Rock, new Vector3(0, 0, nextSpawnZ + 120), Quaternion.identity);
        }
        if (xBarrier)
        {
            Instantiate(Rock, new Vector3(3, 0, nextSpawnZ + 120), Quaternion.identity);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rock"))
        {
            other.gameObject.SetActive(false);

            RimitTime -= 5;
            hpSlider.value -= 20;
            hpmanage -= 20;

            SetCountText();

            animator.Play("Fall");
            animator.speed = 1.5f;

            slipTriger = true;

            StartCoroutine(PlayAnimations());
        }
        if (other.gameObject.CompareTag("Cars"))
        {
            SceneManager.LoadScene("GameOverCar");
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            SceneManager.LoadScene("ClearScene");
        }
    }

    private IEnumerator PlayAnimations()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetTrigger("GetUp");
        animator.speed = 1.0f;

        if (!isDisplaying)
        {
            StartCoroutine(ShowSpeechBubble());
        }
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
        if (TimeText != null)
        {
            TimeText.text = "Time: " + RimitTime.ToString();
        }
    }
}
