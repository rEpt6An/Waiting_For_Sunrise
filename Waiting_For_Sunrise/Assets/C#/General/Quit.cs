using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
//        Debug.Log("正在退出游戏...");

#if UNITY_EDITOR
        // 如果在Unity编辑器中，停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 如果在构建的游戏中，退出应用程序
        Application.Quit();
#endif
    }
}
