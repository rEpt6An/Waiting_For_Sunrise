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
//        Debug.Log("�����˳���Ϸ...");

#if UNITY_EDITOR
        // �����Unity�༭���У�ֹͣ����ģʽ
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ����ڹ�������Ϸ�У��˳�Ӧ�ó���
        Application.Quit();
#endif
    }
}
