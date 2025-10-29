using UnityEngine;
using Assets.C_.player.player;

// 确保玩家对象上有这些核心组件
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerCharacter : MonoBehaviour
{
    // --- 后端数据引用 ---
    // 这是你的纯C#数据层，负责管理所有数值
    private PlayerState _playerState;

    // --- Unity组件引用 ---
    // [Header("UI组件引用")]
    // public HealthBarUI healthBar; // 示例：血条UI脚本
    // public ExperienceBarUI expBar; // 示例：经验条UI脚本
    // public CoinDisplayUI coinDisplay; // 示例：金币UI脚本

    // 静态单例，方便其他脚本（比如EnemyController）快速访问玩家
    public static PlayerCharacter Instance { get; private set; }

    public PlayerAsset PlayerAsset { get; private set; } // 将PlayerAsset改为公共属性

    void Awake()
    {
        // --- 单例模式实现 ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _playerState = PlayerState.Instance;
        PlayerAsset = new PlayerAsset(); // 在这里创建唯一实例

        // 可以在这里进行一次初始UI更新
        UpdateAllUI();
    }

    // --- 核心功能方法 ---

    /// <summary>
    /// 当玩家受到伤害时调用
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        // 可以在这里加入防御和闪避计算
        // 示例： if (Random.value < _playerState.Dodge) { return; } // 闪避成功
        // int finalDamage = Mathf.Max(1, damageAmount - _playerState.DefensivePower);

        _playerState.changeBlood(-damageAmount); // 直接调用后端方法
        UnityEngine.Debug.Log($"玩家受到 {damageAmount} 伤害, 剩余血量: {_playerState.Blood}");

        // 更新血量UI
        // healthBar.UpdateHealth(_playerState.Blood, _playerState.MaxHP);

        if (_playerState.isDie())
        {
            HandleDeath();
        }
    }

    /// <summary>
    /// 当玩家获得经验时调用
    /// </summary>
    public void GainExperience(int amount)
    {
        _playerState.changeExperience(amount);
        UnityEngine.Debug.Log($"获得 {amount} 经验, 总经验: {_playerState.Experience}");

        // 更新经验UI
        // expBar.UpdateExperience(_playerState.Experience, requiredExp); // (需要一个计算升级所需经验的逻辑)
    }

    private void HandleDeath()
    {
        UnityEngine.Debug.LogWarning("玩家已死亡！");
        // 禁用玩家移动和攻击等
        GetComponent<PlayerMovement>().enabled = false;
        // 可以在这里触发Game Over逻辑
    }

    /// <summary>
    /// 一个集中的方法，用于在游戏开始或需要时更新所有UI
    /// </summary>
    public void UpdateAllUI()
    {
        // healthBar.UpdateHealth(_playerState.Blood, _playerState.MaxHP);
        // expBar.UpdateExperience(_playerState.Experience, requiredExp);
        // coinDisplay.UpdateCoins(PlayerAsset.Instance.Money); // 假设PlayerAsset也是单例
    }

    // 在Update中检测攻击输入
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0代表鼠标左键
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        UnityEngine.Debug.Log("玩家发动了攻击！");
        // TODO: 在这里实现攻击逻辑
        // 1. 确定攻击范围 (一个圆或一个扇形)
        // 2. 检测范围内的所有敌人
        // 3. 对检测到的每个敌人调用其TakeDamage方法
    }
}