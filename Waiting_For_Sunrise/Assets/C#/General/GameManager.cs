using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 单例模式
    public static GameManager Instance { get; private set; }

    // --- 游戏状态数据 ---
    public int Day { get; private set; } = 1;
    private const int MAX_DAYS = 10; 

    private PlayerCharacter playerCharacter;

    void Awake()
    {
        // --- 单例实现 ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"场景 '{scene.name}' 已加载. 当前是第 {Day} 天.");

        if (scene.name == "Shop")
        {
            if (Day < MAX_DAYS)
            {
                IncrementDay();
            }
        }

        if (scene.name == "Night")
        {
            // 在 Night 场景中查找 PlayerCharacter
            playerCharacter = FindObjectOfType<PlayerCharacter>();

            // 检查是否应该触发胜利（当进入第 10 天 Night 时）
            CheckForVictory();
        }
    }

    private void IncrementDay()
    {
        if (Day < MAX_DAYS)
        {
            Day++;
            Debug.Log($"新的一天开始了! 现在是第 {Day} 天.");
        }
    }

    private void CheckForVictory()
    {

        if (Day == MAX_DAYS)
        {
            Debug.Log($"🚨 最终考验：第 {Day} 天的夜晚开始...");
        }
        else if (Day < MAX_DAYS)
        {
            Debug.Log($"第 {Day} 天的夜晚开始...");
        }
    }

    /// <summary>
    /// ⭐️ 由 CountdownTimer 调用，检查是否胜利 (第 10 天结束)。
    /// </summary>
    public void HandleDayEnd(DayCompletionPanel panel, SceneSwitcher switcher)
    {
        if (Day >= MAX_DAYS)
        {
            // 达到最终天数，触发胜利
            HandleGameVictory();
        }
        else
        {
            if (playerCharacter != null && playerCharacter.PlayerState != null)
            {
                int maxHP = playerCharacter.PlayerState.MaxHP;
                playerCharacter.PlayerState.changeBlood(maxHP);

                Debug.Log("倒计时结束，玩家生命值已回满。");
            }

            // 正常显示 Day Completion Panel
            if (panel != null)
            {
                panel.Show(Day); // 显示面板，由面板处理跳转到 Shop
            }
            else
            {
                Debug.LogError("DayCompletionPanel 丢失，直接跳转 Shop。");
                switcher.SwitchScene("Shop");
            }
        }
    }

    /// <summary>
    /// ⭐️ 核心： Boss 死亡或第 10 天结束触发胜利。
    /// </summary>
    public void HandleGameVictory()
    {
        Debug.LogWarning("游戏胜利！你成功存活了 " + MAX_DAYS + " 天！");

        if (GlobalAudioManager.Instance != null)
        {
            GlobalAudioManager.Instance.StopBGM();
        }

        // 确保游戏暂停
        Time.timeScale = 0f;

        // 查找胜利面板并显示
        VictoryPanel victoryPanel = FindObjectOfType<VictoryPanel>(true);
        if (victoryPanel != null)
        {
            victoryPanel.Show();
        }
        else
        {
            Debug.LogError("场景中缺少 VictoryPanel 实例！");
        }
    }
    // --- 辅助方法 ---
    public string GetDayString()
    {
        return $"Day: {Day}";
    }

    /// <summary>
    /// ⭐️ 修正：提供玩家角色的引用
    /// </summary>
    public PlayerCharacter GetPlayerCharacter()
    {
        return playerCharacter;
    }
}