// 📜 VictoryPanel.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryPanel : MonoBehaviour
{
    [Header("UI 引用")]
    [SerializeField] private TextMeshProUGUI victoryMessageText;
    [SerializeField] private Button returnToMenuButton;

    [Header("配置")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Awake()
    {
        // 确保面板初始是非活动的
        gameObject.SetActive(false);

        if (returnToMenuButton != null)
        {
            returnToMenuButton.onClick.RemoveAllListeners();
            returnToMenuButton.onClick.AddListener(ReturnToMenu);
        }
    }

    public void Show()
    {
        if (victoryMessageText != null)
        {
            victoryMessageText.text = $"Victory!!!";
        }
        gameObject.SetActive(true);
        Time.timeScale = 0f; // 确保游戏停止
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // 恢复时间
        // 清理 GameManager 实例 (因为是 DontDestroyOnLoad)
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        SceneManager.LoadScene(mainMenuSceneName);
    }
}