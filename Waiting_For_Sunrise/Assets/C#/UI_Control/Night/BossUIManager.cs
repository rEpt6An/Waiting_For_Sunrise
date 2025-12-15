using UnityEngine;
using UnityEngine.UI;
using TMPro; // 假设使用 TextMeshPro

public class BossUIManager : MonoBehaviour
{
    // ⭐️ 单例模式：确保全局只有一个 UI 实例管理 Boss 血条
    public static BossUIManager Instance { get; private set; }

    [Header("UI 元素引用 (固定位置)")]
    [Tooltip("包含血条、名称的父容器，用于显示/隐藏")]
    [SerializeField] private GameObject bossUIContainer;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private TextMeshProUGUI healthValueText;

    private BossHealthMonitor currentBoss;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // 如果 Canvas 需要跨场景，则启用
        }
        else
        {
            Destroy(gameObject);
        }

        // 初始状态：隐藏血条
        if (bossUIContainer != null)
        {
            bossUIContainer.SetActive(false);
        }
    }

    void Start()
    {
        // 尝试在启动时立即查找场景中是否存在 Boss
    }

    /// <summary>
    /// 在 Boss 死亡时调用，清理 UI。
    /// </summary>
    private void OnBossDiedHandler()
    {
        if (currentBoss != null)
        {
            // 解除监听
            currentBoss.OnHealthChanged -= UpdateHealthBar;
            currentBoss.OnBossDied -= OnBossDiedHandler;
            currentBoss.OnBossInitialized -= OnBossInitializedHandler;

            currentBoss = null;
        }

        // 隐藏 UI
        if (bossUIContainer != null)
        {
            bossUIContainer.SetActive(false);
        }
        Debug.Log("BossUIManager: 血条已隐藏 (Boss 死亡)。");
    }

    /// <summary>
    /// 在 Boss 健康变化时调用，更新血条显示。
    /// </summary>
    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
        if (healthValueText != null)
        {
            healthValueText.text = $"{currentHealth:F0} / {maxHealth:F0}";
        }
    }

    /// <summary>
    /// 在 Boss 初始化时调用，设置名称并显示 UI。
    /// </summary>
    private void OnBossInitializedHandler()
    {
        if (currentBoss == null) return;

        if (bossNameText != null)
        {
            bossNameText.text = currentBoss.bossDisplayName;
        }
        if (bossUIContainer != null)
        {
            bossUIContainer.SetActive(true);
        }

        // 立即更新一次血量
        if (currentBoss.GetComponent<EnemyController>() is EnemyController ec)
        {
            UpdateHealthBar(ec.CurrentHealth, ec.MaxHealth);
        }
        Debug.Log($"BossUIManager: 发现并开始监控 Boss: {currentBoss.bossDisplayName}");
    }

    /// <summary>
    /// 查找场景中唯一存在的 BossHealthMonitor 实例。
    /// </summary>
    /// <summary>
    /// ⭐️ 核心新增：公开方法，供 Spawner 在 Boss 生成时调用。
    /// </summary>
    public void RegisterNewBoss(BossHealthMonitor newBoss)
    {
        if (currentBoss != null)
        {
            // 如果场景中已经有 Boss，先清理旧的监听（以防万一）
            OnBossDiedHandler();
        }

        currentBoss = newBoss;

        // 订阅 Boss 的事件
        currentBoss.OnHealthChanged += UpdateHealthBar;
        currentBoss.OnBossDied += OnBossDiedHandler;
        currentBoss.OnBossInitialized += OnBossInitializedHandler;

        // 由于 Boss 刚刚被实例化，我们立即调用初始化方法
        // 注意：这里我们主动调用初始化处理，而不是等待 BossHealthMonitor.Start() 触发事件。
        OnBossInitializedHandler();
    }
}