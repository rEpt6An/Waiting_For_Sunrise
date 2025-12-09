// PlayerCharacter.cs
using UnityEngine;
using Assets.C_.player;
using Assets.C_.player.player;
using Assets.C_.player.bag;
using Assets.C_.common.common;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("攻击设置")]
    [SerializeField] private WeaponData currentWeapon;
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private Transform attackSpawnPoint;

    // ⭐️ 引用 WeaponAnimator 
    private WeaponAnimator weaponAnimator;

    public IPlayerState PlayerState { get; private set; }
    public IPlayerAsset PlayerAsset { get; private set; }

    private float attackCooldownTimer = 0f;

    void Awake()
    {
        // 从后端的 Player 单例中获取 State 和 Asset
        Player backendPlayer = Player.GetInstance();
        PlayerState = backendPlayer.PlayerState;
        PlayerAsset = backendPlayer.PlayerAsset;
    }

    void Start()
    {
        // ⭐️ 获取 WeaponAnimator 引用并在 Awake/Start 之后初始化
        weaponAnimator = GetComponentInChildren<WeaponAnimator>();
        if (weaponAnimator != null)
        {
            weaponAnimator.Initialize(currentWeapon);
        }
    }

    void Update()
    {
        attackCooldownTimer += Time.deltaTime;
        if (currentWeapon == null || PlayerState == null)
        {
            return;
        }

        double playerAttackSpeed = PlayerState.AttackSpeed > 0 ? PlayerState.AttackSpeed : 1.0;
        float attackCooldown = 1f / (currentWeapon.baseAttackSpeed * (float)playerAttackSpeed);

        if (Input.GetMouseButton(0) && attackCooldownTimer >= attackCooldown)
        {
            if (weaponAnimator != null)
            {
                if (currentWeapon.attackType == 1 && weaponAnimator.IsReloading()) // 远程武器且正在装弹
                {
                    // 正在装弹，不攻击
                    return;
                }

                // 尝试消耗弹药。近战永远返回 true。远程只有有弹药才返回 true。
                if (!weaponAnimator.ConsumeClip())
                {
                    // 弹药不足，返回，等待自动装弹
                    return;
                }
            }

            PerformAttack();
            attackCooldownTimer = 0f;
        }
    }

    // --- 新增：提供一个给外部调用的方法，用于增加金币 ---
    public void GainCoins(int amount)
    {
        if (PlayerAsset != null)
        {
            PlayerAsset.ChangeMoney(amount);
            UnityEngine.Debug.Log($"玩家获得了 {amount} 金币");
        }
    }

    private void PerformAttack()
    {
        // ... (获取攻击方向、实例化攻击碰撞体、计算最终伤害的代码保持不变) ...
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - attackSpawnPoint.position).normalized;

        GameObject attackInstance = Instantiate(attackPrefab, attackSpawnPoint.position, Quaternion.identity);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

        float correspondingPlayerDamage = 0;
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

        WeaponAttack attackScript = attackInstance.GetComponent<WeaponAttack>();
        if (attackScript != null)
        {
            attackScript.Initialize(finalDamage, currentWeapon.repel, attackSpawnPoint.position);
        }

        // ⭐️ 触发动画
        if (weaponAnimator != null)
        {
            weaponAnimator.TriggerAttackAnimation(currentWeapon.attackType);
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