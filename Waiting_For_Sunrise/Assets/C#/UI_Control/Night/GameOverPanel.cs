using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    [Header("UI 引用")]
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [Header("配置")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Awake()
    {
        // 确保初始时面板是隐藏的
        gameObject.SetActive(false);

        // 绑定按钮事件
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f; // 停止游戏逻辑

        if (gameOverText != null)
        {
            // 你可以在这里根据 GameManager.Instance.Day 显示你存活了多久
            int survivedDays = (GameManager.Instance != null) ? GameManager.Instance.Day : 1;
            gameOverText.text = $"You survived for {survivedDays} days.";
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        // 如果你的 GameManager 是 DontDestroyOnLoad，重启前可能需要重置它或销毁它
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        SceneManager.LoadScene(mainMenuSceneName);
    }
}