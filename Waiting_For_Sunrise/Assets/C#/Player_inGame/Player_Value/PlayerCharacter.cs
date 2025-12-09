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
public class PlayerCharacter : MonoBehaviour
{
    [Header("武器配置")]
    [Tooltip("可供切换的武器列表。索引 0 对应按键 1，依此类推。")]
    public List<WeaponData> availableWeapons = new List<WeaponData>();

    [Tooltip("武器的攻击点")]
    [SerializeField] private Transform attackSpawnPoint;

    // 运行时引用
    private WeaponData currentWeapon;
    private int currentWeaponIndex = -1;
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
    }

    void Start()
    {
        // 获取 WeaponAnimator 引用
        weaponAnimator = GetComponentInChildren<WeaponAnimator>();
        if (weaponAnimator == null)
        {
            //Debug.LogError("PlayerCharacter: 在子对象中找不到 WeaponAnimator 组件！");
        }

        // 检查 UI 引用
        if (weaponUI == null)
        {
            //Debug.LogError("PlayerCharacter: WeaponUI 引用未在Inspector中设置！");
        }

        // 启动时装备第一个武器
        if (availableWeapons != null && availableWeapons.Count > 0 && availableWeapons[0] != null)
        {
            SwitchWeapon(0);
        }
        else
        {
            // 即使没有武器，也要通知UI显示空状态
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
            weaponUI.SetupWeaponSlots(availableWeapons ?? new List<WeaponData>(), currentWeaponIndex);
        }
    }
    void Update()
    {
        if (currentWeapon == null || PlayerState == null)
        {
            // 即使没有武器，也需要处理武器切换输入
            HandleWeaponSwitching();
            return;
        }

        // 攻击冷却计时
        attackCooldownTimer += Time.deltaTime;

        // 尝试攻击
        if (Input.GetMouseButton(0))
        {
            TryAttack();
        }

        // 武器切换输入
        HandleWeaponSwitching();
    }

    // --- 攻击核心逻辑 ---
    public void TryAttack()
    {
        double playerAttackSpeed = PlayerState.AttackSpeed > 0 ? PlayerState.AttackSpeed : 1.0;
        // 攻速计算： 1 / (武器基础攻速 * 玩家攻速加成)
        float attackCooldown = 1f / (currentWeapon.baseAttackSpeed * (float)playerAttackSpeed);

        // 检查冷却
        if (attackCooldownTimer < attackCooldown) return;

        if (weaponAnimator != null)
        {
            if (currentWeapon.attackType == 1 && weaponAnimator.IsReloading())
            {
                // 正在装弹，不攻击
                return;
            }

            // 尝试消耗弹药
            if (!weaponAnimator.ConsumeClip())
            {
                // 弹药不足
                return;
            }
        }

        // 攻击成功，执行伤害和动画
        PerformAttack();

        // 重置冷却计时器
        attackCooldownTimer = 0f;
    }

    private void PerformAttack()
    {
        float correspondingPlayerDamage = 0f;

        // 获取鼠标方向
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - attackSpawnPoint.position).normalized;

        // 获取伤害加成
        if (currentWeapon.damageScaleType == 0)
        {
            correspondingPlayerDamage = PlayerState.MeleeAttack;
        }
        else if (currentWeapon.damageScaleType == 1)
        {
            correspondingPlayerDamage = PlayerState.RangedAttack;
        }

        // 实例化攻击碰撞体
        GameObject prefabToInstantiate = currentWeapon.attackPrefab;
        if (prefabToInstantiate == null)
        {
            Debug.LogError($"Weapon '{currentWeapon.weaponName}' is missing an Attack Prefab!");
            return;
        }
        float attackLifetime = 0.1f; // 默认值
        if (currentWeapon.attackType == 0) // 近战
        {
            attackLifetime = currentWeapon.meleeLifetime;
        }

        GameObject attackInstance = Instantiate(prefabToInstantiate, attackSpawnPoint.position, Quaternion.identity);

        // 设置攻击角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 计算最终伤害
        float finalDamage = (currentWeapon.baseDamage + currentWeapon.scalingMultiplier * correspondingPlayerDamage)
                             * (float)PlayerState.DamageMultipler;

        // 初始化 WeaponAttack 脚本
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

        // 远程武器施加初始速度
        if (currentWeapon.attackType == 1)
        {
            Rigidbody2D rb = attackInstance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * currentWeapon.projectileSpeed;
            }
        }

        // 触发动画
        if (weaponAnimator != null)
        {
            weaponAnimator.TriggerAttackAnimation(currentWeapon.attackType);
        }
    }

    // --- 核心方法：武器切换 ---
    private void HandleWeaponSwitching()
    {
        // 遍历 1 到 5 对应的按键
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
        // 检查索引是否有效（即列表中是否存在该武器）
        if (index < 0 || index >= availableWeapons.Count)
        {
            // ... (没有武器时的逻辑保持不变) ...
            Debug.Log($"槽位 {index + 1} 没有武器。");
            currentWeapon = null;
            currentWeaponIndex = -1;

            if (weaponUI != null)
            {
                // 通知 UI 更新选中状态，并清除主显示区
                weaponUI.UpdateCurrentSelection(index);
                weaponUI.SetNewWeapon(null, null); // 传递 null 清除主显示区
            }
            return;
        }


        WeaponData newWeaponData = availableWeapons[index];

        currentWeapon = newWeaponData;
        currentWeaponIndex = index;
        attackCooldownTimer = 0f;

        // 1. 初始化 WeaponAnimator
        if (weaponAnimator != null)
        {
            weaponAnimator.Initialize(newWeaponData);
        }

        // 2. 更新 UI
        if (weaponUI != null)
        {
            // ⭐️ 这里是关键，确保在切换时同时更新主显示和武器栏的选中状态
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
        PlayerState.changeBlood(-damageAmount);
        if (PlayerState.isDie())
        {
            HandleDeath();
        }
    }

    public void GainExperience(int amount)
    {
        PlayerState.changeExperience(amount);
    }

    private void HandleDeath()
    {
        UnityEngine.Debug.LogWarning("玩家已死亡！");
        gameObject.SetActive(false);
    }
}