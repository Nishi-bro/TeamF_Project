using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerScript : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // VideoPlayerの設定（必要に応じて）
        videoPlayer.isLooping = false; // ループ再生しない
        videoPlayer.Play();

        // 動画終了時のイベント
        videoPlayer.loopPointReached += VideoEnded;
    }

    void VideoEnded(VideoPlayer vp)
    {
        // 次のシーンへ遷移
        SceneManager.LoadScene("GameScene");
    }
}