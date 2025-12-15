using UnityEngine;
using System;

[RequireComponent(typeof(EnemyController))]
public class BossHealthMonitor : MonoBehaviour
{
    [Header("BOSS 信息")]
    [Tooltip("在血条上显示的 Boss 名称")]
    public string bossDisplayName = "史莱姆王";

    // ⭐️ 事件：用于通知 UI Manager 状态变化
    public event Action<float, float> OnHealthChanged; // (当前血量, 最大血量)
    public event Action OnBossDied;
    public event Action OnBossInitialized; // 用于初始化 UI

    private EnemyController enemyController;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        if (enemyController == null)
        {
            Debug.LogError("BossHealthMonitor: ❌ 必须挂载在 EnemyController 的同一对象上!");
            enabled = false;
        }

        // 假设初始化完成后，BossUIManager 才能安全地监听
        OnBossInitialized?.Invoke();
    }

    // 监听 BossHealthMonitor 的 Update 间隔（可以自定义，这里跟随 Unity 的 Update）
    void Update()
    {
        if (enemyController == null) return;

        // 实时触发血量变化事件
        OnHealthChanged?.Invoke(enemyController.CurrentHealth, enemyController.MaxHealth);
    }



    void OnDisable()
    {
        // 如果 Boss 被禁用或销毁，通知 UI 移除血条
        if (enemyController != null && enemyController.CurrentHealth <= 0)
        {
            OnBossDied?.Invoke();
        }
    }
}