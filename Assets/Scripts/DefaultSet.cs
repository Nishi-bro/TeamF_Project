using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSet : MonoBehaviour
{
    void Start()
    {
        // VSyncCount を Dont Sync に変更
        QualitySettings.vSyncCount = 0;
        // 60fpsを目標に設定
        Application.targetFrameRate = 60;
    }
}
