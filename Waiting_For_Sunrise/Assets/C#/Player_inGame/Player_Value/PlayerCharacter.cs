// PlayerCharacter.cs
using UnityEngine;
using Assets.C_.player;
using Assets.C_.player.player;
using Assets.C_.player.bag;
using Assets.C_.common.common;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]

public class PlayerCharacter : MonoBehaviour
{
    [Header("音效配置")]
    [Tooltip("玩家受伤时播放的音效片段")]
    [SerializeField] private AudioClip hitSound;
    [Tooltip("玩家死亡时播放的音效片段")]
    [SerializeField] private AudioClip deathSound;


    // --- 升级配置 ---
    [Header("升级配置")]
    [Tooltip("每次升级时随机抽取的武器库")]
    [SerializeField] private List<WeaponData> upgradeWeaponPool;
    [Tooltip("用于在升级时暂停游戏并选择武器的 UI Panel (需要在 Inspector 中引用)")]
    [SerializeField] private GameObject weaponSelectionUIPanel;

    private AudioSource audioSource;



    [Header("武器配置")]
    [Tooltip("第一次启动时的初始武器配置。注意：切换场景后此列表无效。")]
    [SerializeField] private List<WeaponData> initialWeaponsConfig = new List<WeaponData>();

    // ⭐️ 修正：使用一个属性来始终获取持久化武器列表
    public List<WeaponData> AvailableWeapons => Player.GetInstance().GlobalWeaponArsenal;


    [Tooltip("武器的攻击点")]
    [SerializeField] private Transform attackSpawnPoint;

    // 运行时引用
    private WeaponData currentWeapon;
    public int currentWeaponIndex = -1;
    private WeaponAnimator weaponAnimator;

    [Header("UI 引用")]
    [Tooltip("用于显示武器信息的 UI 脚本")]
    public WeaponUI weaponUI;

    public IPlayerState PlayerState { get; private set; }
    public IPlayerAsset PlayerAsset { get; private set; }

    private float attackCooldownTimer = 0f;
    private const int MAX_WEAPON_SLOTS = 5; // 定义最大武器槽位数量

    void Awake()
    {
        // 从后端的 Player 单例中获取 State 和 Asset
        Player backendPlayer = Player.GetInstance();
        PlayerState = backendPlayer.PlayerState;
        PlayerAsset = backendPlayer.PlayerAsset;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("PlayerCharacter: ❌ 缺少 AudioSource 组件！请检查 [RequireComponent] 或手动添加。");
        }
    }

    void Start()
    {
        if (AvailableWeapons.Count == 0 && initialWeaponsConfig.Count > 0)
        {
            // 将初始配置拷贝到持久化列表中
            AvailableWeapons.AddRange(initialWeaponsConfig);
            Debug.Log($"玩家首次启动，从配置加载了 {AvailableWeapons.Count} 个初始武器。");
        }

        // 获取 WeaponAnimator 引用 (保持不变)
        weaponAnimator = GetComponentInChildren<WeaponAnimator>();
        // ... (错误检查保持不变) ...

        // ⭐️ 核心修正 2：启动时装备武器 (使用持久化列表)
        if (AvailableWeapons != null && AvailableWeapons.Count > 0)
        {
            // 确保 currentWeaponIndex 有一个合理的初始值（场景切换后这个值可能保持不变）
            if (currentWeaponIndex < 0 || currentWeaponIndex >= AvailableWeapons.Count)
            {
                currentWeaponIndex = 0; // 默认装备第一个武器
            }
            SwitchWeapon(currentWeaponIndex);
        }
        else
        {
            Debug.Log("玩家没有初始武器。");
            currentWeaponIndex = -1;
            if (weaponUI != null)
            {
                weaponUI.SetNewWeapon(null, null);
            }
        }

        // 无论是否有武器，都调用 SetupWeaponSlots 来初始化武器库 UI
        if (weaponUI != null)
        {
            // 传入持久化列表
            weaponUI.SetupWeaponSlots(AvailableWeapons ?? new List<WeaponData>(), currentWeaponIndex);
        }
    }

    // -------------------------------------------------------------
    // --- 以下方法的内部实现，只需将所有对 availableWeapons 的引用
    // --- 替换为新的 AvailableWeapons 属性即可。
    // -------------------------------------------------------------

    void Update()
    {
        if (currentWeapon == null || PlayerState == null)
        {
            HandleWeaponSwitching();
            return;
        }

        attackCooldownTimer += Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            TryAttack();
        }

        HandleWeaponSwitching();
    }

    // --- 攻击核心逻辑 ---
    public void TryAttack()
    {
        double playerAttackSpeed = PlayerState.AttackSpeed > 0 ? PlayerState.AttackSpeed : 1.0;
        float attackCooldown = 1f / (currentWeapon.baseAttackSpeed * (float)playerAttackSpeed);

        if (attackCooldownTimer < attackCooldown) return;

        if (weaponAnimator != null)
        {
            if (currentWeapon.attackType == 1 && weaponAnimator.IsReloading())
            {
                return;
            }

            if (!weaponAnimator.ConsumeClip())
            {
                return;
            }
        }

        PerformAttack();

        attackCooldownTimer = 0f;
    }

    private void PerformAttack()
    {
        if (currentWeapon == null || attackSpawnPoint == null) return;

        float correspondingPlayerDamage = 0f;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 baseDirection = (mousePosition - attackSpawnPoint.position).normalized;

        if (currentWeapon.damageScaleType == 0)
        {
            correspondingPlayerDamage = PlayerState.MeleeAttack;
        }
        else if (currentWeapon.damageScaleType == 1)
        {
            correspondingPlayerDamage = PlayerState.RangedAttack;
        }

        float finalDamage = (currentWeapon.baseDamage + currentWeapon.scalingMultiplier * correspondingPlayerDamage)
                             * (float)PlayerState.DamageMultipler;

        if (weaponAnimator != null)
        {
            weaponAnimator.TriggerAttackAnimation(currentWeapon.attackType);
        }

        int count = currentWeapon.attackType == 1 ? currentWeapon.projectileCount : 1;

        for (int i = 0; i < count; i++)
        {
            GameObject prefabToInstantiate = currentWeapon.attackPrefab;
            if (prefabToInstantiate == null)
            {
                Debug.LogError($"Weapon '{currentWeapon.weaponName}' is missing an Attack Prefab!");
                continue;
            }

            float randomAngleOffset = 0f;
            if (currentWeapon.attackType == 1 && currentWeapon.spreadAngle > 0f)
            {
                randomAngleOffset = Random.Range(-currentWeapon.spreadAngle / 2f, currentWeapon.spreadAngle / 2f);
            }

            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
            float finalAngle = baseAngle + randomAngleOffset;

            Vector2 direction = new Vector2(
                Mathf.Cos(finalAngle * Mathf.Deg2Rad),
                Mathf.Sin(finalAngle * Mathf.Deg2Rad)
            ).normalized;


            GameObject attackInstance = Instantiate(prefabToInstantiate, attackSpawnPoint.position, Quaternion.identity);

            float attackLifetime = 0.1f;
            if (currentWeapon.attackType == 0)
            {
                attackLifetime = currentWeapon.meleeLifetime;
            }

            attackInstance.transform.rotation = Quaternion.Euler(0, 0, finalAngle);

            WeaponAttack attackScript = attackInstance.GetComponent<WeaponAttack>();
            if (attackScript != null)
            {
                attackScript.Initialize(
                    finalDamage,
                    currentWeapon.repel,
                    attackSpawnPoint.position,
                    currentWeapon.penetrate,
                    currentWeapon.attackType,
                    attackLifetime
                );
            }

            if (currentWeapon.attackType == 1)
            {
                Rigidbody2D rb = attackInstance.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direction * currentWeapon.projectileSpeed;
                }
            }
        }

        attackCooldownTimer = 0f;
    }
    // --- 核心方法：武器切换 ---
    private void HandleWeaponSwitching()
    {
        for (int i = 0; i < MAX_WEAPON_SLOTS; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchWeapon(i);
                break;
            }
        }
    }

    public void SwitchWeapon(int index)
    {
        // ⭐️ 使用持久化列表
        if (index < 0 || index >= AvailableWeapons.Count)
        {
            Debug.Log($"槽位 {index + 1} 没有武器。");
            currentWeapon = null;
            currentWeaponIndex = -1;

            if (weaponUI != null)
            {
                weaponUI.UpdateCurrentSelection(index);
                weaponUI.SetNewWeapon(null, null);
            }
            return;
        }

        // ⭐️ 使用持久化列表
        WeaponData newWeaponData = AvailableWeapons[index];

        currentWeapon = newWeaponData;
        currentWeaponIndex = index;
        attackCooldownTimer = 0f;

        if (weaponAnimator != null)
        {
            weaponAnimator.Initialize(newWeaponData);
        }

        if (weaponUI != null)
        {
            weaponUI.SetNewWeapon(weaponAnimator, newWeaponData);
            weaponUI.UpdateCurrentSelection(index);
        }

        Debug.Log($"已切换到武器: {newWeaponData.weaponName} (索引: {index + 1})");
    }

    public void GainCoins(int amount)
    {
        if (PlayerAsset != null)
        {
            PlayerAsset.ChangeMoney(amount);
            UnityEngine.Debug.Log($"玩家获得了 {amount} 金币");
        }
    }

    public void TakeDamage(int damageAmount)
    {
        // 播放受伤音效
        if (audioSource != null && hitSound != null)
        {
            // 使用 PlayOneShot 以确保不会打断其他正在播放的声音 (如射击声)
            audioSource.PlayOneShot(hitSound);
        }

        // 扣血逻辑
        PlayerState.changeBlood(-damageAmount);

        if (PlayerState.isDie())
        {
            HandleDeath();
        }
    }
    public void GainExperience(int amount)
    {
        if (PlayerState == null) return;

        // 1. 增加总经验值
        PlayerState.changeExperience(amount);

        int debugLoopCounter = 0; // 调试计数器，用于防止无限循环崩溃

        // 循环检查，直到经验值不足以升级
        while (true)
        {
            debugLoopCounter++;
            int currentLevel = PlayerState.Level;

            // 计算下一级所需总经验值 (例如：Lv2 需要 50, Lv3 需要 150)
            int expRequiredToNextLevelTotal = CalculateExpRequired(currentLevel + 1);

            // 当前等级总共需要的经验值 (例如：Lv2 需要 50 - 0 = 50; Lv3 需要 150 - 50 = 100)
            int expNeededToCompleteLevel = expRequiredToNextLevelTotal - CalculateExpRequired(currentLevel);

            // 当前等级进度经验 (PlayerState.Experience 存储的是总经验)
            int currentExpProgress = PlayerState.Experience - CalculateExpRequired(currentLevel);

            // ⭐️ 调试日志：在每次循环开始时打印当前状态
            Debug.Log($"尝试升级: [Lv {currentLevel}] -> 当前总经验: {PlayerState.Experience} / 下一级总经验: {expRequiredToNextLevelTotal}。 " +
                      $"当前等级进度: {currentExpProgress} / {expNeededToCompleteLevel}. 循环次数: {debugLoopCounter}");

            // 🚨 检查是否进入无限循环
            if (debugLoopCounter > 50)
            {
                Debug.LogError("🚨 警告：升级循环次数过多 (超过 50 次)，可能进入无限循环或经验值设置有问题！已停止升级检查。");
                break;
            }

            // 检查总经验值是否达到升级所需总经验
            if (PlayerState.Experience >= expRequiredToNextLevelTotal)
            {
                // 经验值足够升级
                LevelUp();

                // 循环会继续检查，看是否能连升多级
            }
            else
            {
                // 经验不足，退出循环
                break;
            }
        }
    }

    private void LevelUp()
    {
        int currentLevel = PlayerState.Level;

        // ⭐️ 核心修正：提升等级
        PlayerState.changeLevel(1); // 必须调用 PlayerState 中的 changeLevel(1)

        Debug.Log($"🎉 玩家升级成功！当前等级: {currentLevel + 1}。");

        // 2. 提升属性
        // 遵循您的需求：每升一级 +5 最大生命值，+5% 伤害
        PlayerState.changeMaxHP(5); // +5 最大生命值

        // 3. 检查是否触发武器选择 (前4次升级)
        if (currentLevel < 5) // currentLevel 是升级前的等级 (1 -> 5 会触发)
        {
            TriggerWeaponSelection();
        }
    }

    public int CalculateExpRequired(int targetLevel)
    {
        if (targetLevel <= 1) return 0; // 1 级需要总经验 0
        if (targetLevel == 2) return 50; // 2 级需要总经验 50
        if (targetLevel == 3) return 150; // 3 级需要总经验 50 + 100 = 150

        // 从 Lv4 开始，增量为 (N-3) * 100。
        // Lv4 增量 = 200, 总经验 = 150 + 200 = 350
        // Lv5 增量 = 300, 总经验 = 350 + 300 = 650

        // 使用公式计算 Lv4 及以上所需的总经验
        int totalExp = 150; // 达到 Lv3 的总经验
        for (int n = 4; n <= targetLevel; n++)
        {
            // 增量经验是 (N - 2) * 100
            int requiredExp = (n - 2) * 100;
            totalExp += requiredExp;
        }

        return totalExp;
    }
    private void TriggerWeaponSelection()
    {
        if (weaponSelectionUIPanel != null)
        {
            Time.timeScale = 0f;
            weaponSelectionUIPanel.SetActive(true);

            WeaponSelectionPanel selectionScript = weaponSelectionUIPanel.GetComponent<WeaponSelectionPanel>();
            if (selectionScript != null)
            {
                // ⭐️ 传入持久化列表
                selectionScript.SetupSelection(upgradeWeaponPool, AvailableWeapons);
            }
            else
            {
                Debug.LogError("WeaponSelectionUIPanel 缺少 WeaponSelectionPanel 脚本！");
            }
        }
    }

    private void HandleDeath()
    {
        // ... (保持不变) ...
        UnityEngine.Debug.LogWarning("玩家已死亡！");

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        gameObject.SetActive(false);
        Time.timeScale = 0f;
    }


}