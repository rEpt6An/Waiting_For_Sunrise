using UnityEngine;
using Assets.C_.player.player;
using Assets.C_.player.bag;
using Assets.C_.player;
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

    private IPlayerState _playerState;
    private IPlayerAsset _playerAsset;

    private float attackCooldownTimer = 0f;

    void Awake()
    {
        _playerState = Player.GetInstance().PlayerState;
        _playerAsset = Player.GetInstance().PlayerAsset;
    }

    void Update()
    {
        attackCooldownTimer += Time.deltaTime;
        if (currentWeapon == null)
        {
            UnityEngine.Debug.LogWarning("PlayerCharacter: 武器未装备 (Current Weapon is null)!");
            return;
        }

        double playerAttackSpeed = _playerState.AttackSpeed > 0 ? _playerState.AttackSpeed : 1.0;
        float attackCooldown = 1f / (currentWeapon.baseAttackSpeed * (float)playerAttackSpeed);

        // --- 添加调试日志 ---
        //UnityEngine.Debug.Log($"Attack CD Timer: {attackCooldownTimer:F2} / Cooldown: {attackCooldown:F2}");

        if (Input.GetMouseButton(0) && attackCooldownTimer >= attackCooldown)
        {
            UnityEngine.Debug.Log("--- ATTACK TRIGGERED ---"); // 看看这行是否能打印出来
            PerformAttack();
            attackCooldownTimer = 0f;
        }
    }

    public void GainCoins(int amount)
    {
        _playerAsset.ChangeMoney(amount);
        UnityEngine.Debug.Log($"玩家获得了 {amount} 金币");
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