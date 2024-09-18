using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerScript : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject canvas0; // CanvasAのGameObject
    public GameObject canvas1; // 1つ目の新しいキャンバス
    public GameObject canvas2; // 2つ目の新しいキャンバス
    public GameObject canvas3; // 2つ目の新しいキャンバス
    private int currentCanvasIndex = 0; // 現在のキャンバスインデックス
    private bool videoStarted = false; // 動画が既に開始されているかどうか

    void Start()
    {
        // CanvasAだけ最初に表示
        ShowCanvas(canvas1);
        HideCanvas(canvas2);

        // 動画の準備を開始
        videoPlayer.Prepare();

        // 動画の準備が完了したら呼び出されるイベント
        videoPlayer.prepareCompleted += OnPrepareCompleted;
    }

    void Update()
    {
        // Qキーが押された時
        if (Input.GetKeyDown(KeyCode.Q) && !videoStarted)
        {
            ShowNextCanvas();
        }
    }

    void ShowNextCanvas()
    {
        // 現在のキャンバスを非表示
        if (currentCanvasIndex == 0)
        {
            ShowCanvas(canvas1); // 1つ目の新しいキャンバスを表示
        }
        else if (currentCanvasIndex == 1)
        {
            HideCanvas(canvas2);
            ShowCanvas(canvas3); // 1つ目の新しいキャンバスを表示
        }
        else if (currentCanvasIndex == 2)
        {
            HideCanvas(canvas3);
            StartVideo(); // 2つ目のキャンバスが表示されていたら、動画を再生
        }

        // キャンバスインデックスを進める
        currentCanvasIndex++;
    }

    void ShowCanvas(GameObject canvas)
    {
        canvas.SetActive(true);
    }

    void HideCanvas(GameObject canvas)
    {
        canvas.SetActive(false);
    }
    void StartVideo()
    {
        videoStarted = true; // 動画開始フラグを立てる
        videoPlayer.Play(); // 動画再生
        videoPlayer.loopPointReached += VideoEnded; // 動画終了時のイベント設定
    }

    void OnPrepareCompleted(VideoPlayer vp)
    {
        
    }

    void VideoEnded(VideoPlayer vp)
    {
        // 次のシーンへ遷移
        SceneManager.LoadScene("GameScene");
    }
}
