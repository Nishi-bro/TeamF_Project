using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerScript : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject canvasA; // CanvasAのGameObject

    void Start()
    {
        // 動画の準備を開始
        videoPlayer.Prepare();

        // 動画の準備が完了したら呼び出されるイベント
        videoPlayer.prepareCompleted += OnPrepareCompleted;
    }

    void OnPrepareCompleted(VideoPlayer vp)
    {
        // CanvasAを非表示にして動画を再生
        canvasA.SetActive(false);
        videoPlayer.Play();

        // 動画終了時のイベントを設定
        videoPlayer.loopPointReached += VideoEnded;
    }

    void VideoEnded(VideoPlayer vp)
    {
        // 次のシーンへ遷移
        SceneManager.LoadScene("GameScene");
    }
}
