using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 引入 UI 命名空间，虽然在这个脚本中不再直接使用 Slider，但保持引入以防万一

public class PauseMenuController : MonoBehaviour
{
    // 拖拽您要显示和隐藏的暂停/设置面板
    [Header("UI 引用")]
    [Tooltip("拖拽整个暂停菜单的父级面板 (通常是Canvas下的一个Panel)")]
    [SerializeField] private GameObject pausePanel;

    // 场景配置保持不变
    [Header("场景配置")]
    [Tooltip("主菜单场景的名称，用于回到主菜单按钮")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    // --- 新增音量控制 UI 引用 ---
    // ⚠️ 注意：我们在这里引用滑块是为了让 PauseMenuController 能在运行时设置它们的初始值。
    [Header("设置 UI 引用")]
    [Tooltip("拖拽 BGM 音量滑块（用于初始化设置）")]
    [SerializeField] private Slider bgmSlider;
    [Tooltip("拖拽 SFX 音量滑块（用于初始化设置）")]
    [SerializeField] private Slider sfxSlider;

    // --- 内部变量保持不变 ---
    private bool isGamePaused = false;
    private SceneSwitcher sceneSwitcher;


    void Awake()
    {
        sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        if (sceneSwitcher == null)
        {
            Debug.LogError("PauseMenuController: ❌ 找不到 SceneSwitcher 实例!");
        }

        // 确保游戏启动时，暂停面板是隐藏的
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // 可选：在这里获取 SettingsManager 的实例，确保它在场景中存在并运行
        // FindObjectOfType<SettingsManager>();
    }

    // Update 方法和 PauseGame 保持不变，用于处理 Esc 键暂停

    void Update()
    {
        // 监听 Esc 键输入
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// 暂停游戏并显示设置面板
    /// </summary>
    public void PauseGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        // 核心：设置时间流速为 0 实现暂停
        Time.timeScale = 0f;
        isGamePaused = true;
        Debug.Log("Game Paused. TimeScale set to 0.");
    }

    /// <summary>
    /// 按钮事件和 Esc 键调用：恢复游戏 (即“继续游戏”按钮)
    /// </summary>
    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // 核心：设置时间流速为 1 恢复游戏
        Time.timeScale = 1f;
        isGamePaused = false;
        Debug.Log("Game Resumed. TimeScale set to 1.");
    }

    // BackToMainMenu 和 RestartCurrentGame 保持不变

    // ... （省略 BackToMainMenu 和 RestartCurrentGame 方法） ...

    /// <summary>
    /// 按钮事件：回到主菜单
    /// </summary>
    public void BackToMainMenu()
    {
        // 确保游戏时间恢复，避免主菜单卡住
        Time.timeScale = 1f;

        if (sceneSwitcher != null)
        {
            Debug.Log($"Returning to Main Menu: {mainMenuSceneName}");
            sceneSwitcher.SwitchScene(mainMenuSceneName);
        }
    }

    /// <summary>
    /// 按钮事件：重新开始游戏（当前场景）
    /// </summary>
    public void RestartCurrentGame()
    {
        // TODO: 重新开始逻辑
        // 1. 恢复游戏时间
        Time.timeScale = 1f;

        // 2. 重置后端单例数据 (如 Player.ResetPlayerState()) - 尚未实现，先空着

        // 3. 重新加载当前场景
        sceneSwitcher.SwitchScene(SceneManager.GetActiveScene().name);

        Debug.Log("Game Restarted (Current Scene).");
    }
}