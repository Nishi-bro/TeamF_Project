using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerScript : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject canvas0; // エラーハンドリング用のCanvas
    public GameObject canvas1; // 1つ目のキャンバス
    public GameObject canvas2; // 2つ目のキャンバス
    public GameObject canvas3; // 3つ目のキャンバス
    private int currentCanvasIndex = 0; // 現在のキャンバスインデックス
    private bool videoStarted = false; // 動画が既に開始されているかどうか

    void Start()
    {
        // エラーハンドリング用キャンバスは非表示
        HideCanvas(canvas0);

        // 最初にcanvas1を表示
        ShowCanvas(canvas1);
        HideCanvas(canvas2);
        HideCanvas(canvas3);

        // 動画の準備を開始
        videoPlayer.Prepare();

        // 動画の準備が完了した時のイベント
        videoPlayer.prepareCompleted += OnPrepareCompleted;

        // 動画のエラー時のイベント
        videoPlayer.errorReceived += OnVideoError;
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
        if (currentCanvasIndex == 0)
        {
            HideCanvas(canvas1);
            ShowCanvas(canvas2); // 1つ目の新しいキャンバスを表示
        }
        else if (currentCanvasIndex == 1)
        {
            HideCanvas(canvas2);
            ShowCanvas(canvas3); // 2つ目の新しいキャンバスを表示
        }
        else if (currentCanvasIndex == 2)
        {
            HideCanvas(canvas3);
            StartVideo(); // 3つ目のキャンバスが表示された後に動画を再生
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

    // 動画の準備が完了したら呼ばれる
    void OnPrepareCompleted(VideoPlayer vp)
    {
        // 動画の再生位置をリセット
        vp.time = 0;
        vp.frame = 0;  // フレームもリセット
        Debug.Log("動画の準備が完了し、再生位置をリセットしました。");
    }

    void StartVideo()
    {
        // 再生位置をリセット（追加）
        videoPlayer.time = 0;
        videoPlayer.frame = 0;

        videoStarted = true; // 動画開始フラグを立てる
        videoPlayer.Play(); // 動画再生
        videoPlayer.loopPointReached += VideoEnded; // 動画終了時のイベント設定
        Debug.Log("動画が再生されています");
    }

    // 動画が終了したら次のシーンに移動
    void VideoEnded(VideoPlayer vp)
    {
        Debug.Log("動画が終了しました。次のシーンに遷移します。");
        SceneManager.LoadScene("GameScene");
    }

    // 動画のエラーを受け取った場合に呼ばれる
    void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError("動画の再生エラー: " + message);
        // エラー時はcanvas0（エラーハンドリング用キャンバス）を表示
        ShowCanvas(canvas0);
    }
}
