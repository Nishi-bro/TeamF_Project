using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CanvasScalerEditor : EditorWindow
{
    [MenuItem("Tools/Update All Canvases")]
    static void UpdateAllCanvases()
    {
        // シーン内のすべてのCanvasオブジェクトを取得
        CanvasScaler[] canvasScalers = GameObject.FindObjectsOfType<CanvasScaler>();

        foreach (CanvasScaler canvasScaler in canvasScalers)
        {
            // UI Scale Modeを Scale With Screen Size に設定
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            // 基準解像度を1920x1080に設定
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            // スクリーンマッチモードをWidth or Heightに設定
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            // WidthとHeightのバランスを0.5に設定（幅と高さの中間）
            canvasScaler.matchWidthOrHeight = 0.5f;
        }

        // 結果をコンソールに表示
        Debug.Log($"{canvasScalers.Length} canvas scalers have been updated.");
    }
}
