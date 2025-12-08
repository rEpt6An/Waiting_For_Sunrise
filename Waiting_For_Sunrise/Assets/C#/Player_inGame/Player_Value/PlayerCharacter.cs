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

    // --- 【核心修正】: 将后端数据接口设为公共属性 ---
    public IPlayerState PlayerState { get; private set; }
    public IPlayerAsset PlayerAsset { get; private set; }

    private float attackCooldownTimer = 0f;


    void Start()
    {

        WeaponAnimator weaponAnimator = GetComponentInChildren<WeaponAnimator>();
        if (weaponAnimator != null)
        {
            weaponAnimator.Initialize(currentWeapon);
        }
    }
    void Awake()
    {
        // 从后端的 Player 单例中获取 State 和 Asset
        Player backendPlayer = Player.GetInstance();
        PlayerState = backendPlayer.PlayerState;
        PlayerAsset = backendPlayer.PlayerAsset;
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
        // 1. 获取攻击方向和角度
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - attackSpawnPoint.position).normalized;

        // 2. 实例化攻击碰撞体
        GameObject attackInstance = Instantiate(attackPrefab, attackSpawnPoint.position, Quaternion.identity);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 3. 计算最终伤害
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

        // 4. 初始化 WeaponAttack 脚本
        WeaponAttack attackScript = attackInstance.GetComponent<WeaponAttack>();
        if (attackScript != null)
        {
            attackScript.Initialize(finalDamage, currentWeapon.repel, attackSpawnPoint.position);
        }

        WeaponAnimator weaponAnimator = GetComponentInChildren<WeaponAnimator>();
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