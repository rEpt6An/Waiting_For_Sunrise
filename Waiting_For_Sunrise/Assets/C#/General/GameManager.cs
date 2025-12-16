// 📜 GameManager.cs (修正后)
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 单例模式
    public static GameManager Instance { get; private set; }

    // --- 游戏状态数据 ---
    public int Day { get; private set; } = 1;
    private const int MAX_DAYS = 10; // ⭐️ 修正：第 10 天 Night 结束时胜利

    // 运行时引用：用于跨场景找到玩家，方便回血等操作
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
            // ⭐️ 进入商店，天数+1 (如果不是胜利)
            if (Day < MAX_DAYS)
            {
                IncrementDay();
            }
            // 否则 Day 保持 MAX_DAYS (10)，等待跳转到 Night 场景时CheckForVictory
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

    // ⭐️ 核心胜利判断：时间结束时在第 10 天夜晚触发胜利
    private void CheckForVictory()
    {
        // 这里的逻辑是：如果当前天数达到 MAX_DAYS，并且我们即将开始 Night 场景，就胜利。
        // 但根据您的胜利逻辑：“day10结束游戏胜利”，我们应该在第 10 天的计时器结束时触发胜利。
        // 所以我们只在 Day 10 Night 开始时打印日志，胜利逻辑主要在 CountdownTimer 中实现。
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
            // 尚未达到最终天数，正常流程：弹出面板，进入商店
            if (playerCharacter != null && playerCharacter.PlayerState != null)
            {
                // ⭐️ 核心修正：使用现有方法实现回满血
                int maxHP = playerCharacter.PlayerState.MaxHP;
                // 假设 changeBlood 接受一个增量值，我们直接传入 MaxHP 回满
                // 🚨 注意：这需要您的 changeBlood 方法逻辑是：CurrentHP = MaxHP
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
        Debug.LogWarning("🎉 游戏胜利！你成功存活了 " + MAX_DAYS + " 天！");

        // 确保游戏暂停
        Time.timeScale = 0f;

        // 查找胜利面板并显示
        VictoryPanel victoryPanel = FindObjectOfType<VictoryPanel>(true); // 查找所有对象，包括非活动的
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