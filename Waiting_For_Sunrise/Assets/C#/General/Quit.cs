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
        UnityEngine.Application.Quit();
    }
}
