using UnityEngine;
using Assets.C_.player.player;
using Assets.C_.player.bag;
using Assets.C_.player;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("攻击设置")]
    [SerializeField] private WeaponData currentWeapon;
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private Transform attackSpawnPoint;

    private PlayerState _playerState;
    public PlayerAsset PlayerAsset { get; private set; }

    // 【重要】将属性类型从 IPlayerState 改为 PlayerState 以匹配 Awake 中的赋值
    public PlayerState PlayerState => _playerState;

    private float attackCooldownTimer = 0f;

    void Awake()
    {
        _playerState = (PlayerState)Player.GetInstance().PlayerState;
        PlayerAsset = new PlayerAsset();

    }

    void Update()
    {
        // 攻击冷却计时
        attackCooldownTimer += Time.deltaTime;

        // 检查武器数据是否存在
        if (currentWeapon == null) return;

        // 计算攻击冷却时间
        // 注意：玩家攻速 AttackSpeed 的初始值不应为0，否则会导致除以零错误
        // 我们在这里加一个保护，如果玩家攻速小于等于0，则使用一个默认值
        double playerAttackSpeed = _playerState.AttackSpeed > 0 ? _playerState.AttackSpeed : 1.0;
        float attackCooldown = 1f / (currentWeapon.baseAttackSpeed * (float)playerAttackSpeed);

        // 检测鼠标左键按住，并检查冷却
        if (Input.GetMouseButton(0) && attackCooldownTimer >= attackCooldown)
        {
            PerformAttack();
            attackCooldownTimer = 0f; // 重置冷却
        }
    }

    private void PerformAttack()
    {
        // 1. 获取鼠标方向
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - attackSpawnPoint.position).normalized;

        // 2. 创建攻击实例
        GameObject attackInstance = Instantiate(attackPrefab, attackSpawnPoint.position, Quaternion.identity);



        // 3. 旋转攻击实例使其朝向鼠标
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 4. 计算最终伤害
        float correspondingPlayerDamage = 0;
        // 检查伤害加成类型: 0=近战, 1=远程
        if (currentWeapon.damageScaleType == 0)
        {
            correspondingPlayerDamage = _playerState.MeleeAttack;
        }
        else if (currentWeapon.damageScaleType == 1)
        {
            correspondingPlayerDamage = _playerState.RangedAttack;
        }

        float finalDamage = (currentWeapon.baseDamage + currentWeapon.scalingMultiplier * correspondingPlayerDamage)
                            * (float)_playerState.DamageMultipler;

        // 5. 将伤害值传递给攻击脚本
        WeaponAttack attackScript = attackInstance.GetComponent<WeaponAttack>();
        if (attackScript != null)
        {
            attackScript.Initialize(finalDamage);
        }
    }

    // --- 玩家状态接口 ---
    // 其他脚本现在可以通过 FindObjectOfType<PlayerCharacter>() 来调用这些方法
    public void TakeDamage(int damageAmount)
    {
        // 未来可以在这里加入防御和闪避计算
        _playerState.changeBlood(-damageAmount);

        if (_playerState.isDie())
        {
            HandleDeath();
        }
    }

    public void GainExperience(int amount)
    {
        _playerState.changeExperience(amount);
    }

    private void HandleDeath()
    {
        UnityEngine.Debug.LogWarning("玩家已死亡！");
        // 简单地禁用玩家对象，可以换成更复杂的死亡逻辑
        gameObject.SetActive(false);
    }
}