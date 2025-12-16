using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class FireSixShooterController : EnemyController
{
    // --- 弹幕配置 ---
    [Header("弹幕配置")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private int projectileDamage = 6;
    [SerializeField] private GameObject trackingProjectilePrefab;
    [SerializeField] private int trackingProjectileHP = 30;
    [SerializeField] private float spawnRadius = 1.5f;

    // --- 攻击配置 ---
    [Header("攻击参数")]
    [SerializeField] private float regularAttackInterval = 1.5f;
    [SerializeField] private float enragedAttackInterval = 0.8f;

    // --- 狂暴配置 (半血) ---
    [Header("狂暴模式")]
    // ⭐️ 解决：UI Panel 挂载问题。确保这是一个 GameObject 引用
    [Tooltip("首次进入狂暴时展示的 UI Panel (确保在 Inspector 中拖入一个 GameObject)")]
    [SerializeField] public GameObject rageUIPanel;
    [Tooltip("狂暴攻击结束后的冷却时间 (秒)")]
    [SerializeField] private float rageCooldownDuration = 2f;
    [Tooltip("半血狂暴时播放的音效")]
    [SerializeField] private AudioClip rageSound;

    // --- 动画 & 音效 ---
    private AudioSource audioSource;
    private readonly int AnimScatter = Animator.StringToHash("ScatterAttack");
    private readonly int AnimTracker = Animator.StringToHash("TrackerAttack");
    private readonly int AnimIdle = Animator.StringToHash("Idle");

    // --- 内部状态 (保持不变) ---
    private enum ShooterState { Pursuing, Attacking, PostAttackCooldown, EnragedRageSequence, EnragedCooldown, Dead }
    private ShooterState currentState = ShooterState.Pursuing;
    private Animator animator;

    private float attackTimer = 0f;
    private bool hasEnraged = false;
    private bool isDead = false;

    // ⭐️ 初始化
    public override void Initialize(EnemyData data, PlayerCharacter player)
    {
        base.Initialize(data, player);
        animator = GetComponent<Animator>();
        // ⭐️ 获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();

        if (animator == null) Debug.LogWarning("FireSixShooter: 找不到 Animator 组件。");
        if (audioSource == null) Debug.LogWarning("FireSixShooter: 找不到 AudioSource 组件。");

        attackTimer = regularAttackInterval;

        // 初始化时隐藏 UI Panel
        if (rageUIPanel != null) rageUIPanel.SetActive(false);

        UpdateAnimator(AnimIdle, true);
        Debug.Log("FireSixShooter: ✅ 初始化成功。");
    }

    // ⭐️ 核心 Update 逻辑 (保持不变，只依赖状态机)
    public override void HandleMovementAndFlip()
    {
        if (isDead || rb.velocity.magnitude >= 0.1f) return;

        Vector3 playerPos = playerCharacter.transform.position;
        HandleFlip(playerPos);

        switch (currentState)
        {
            case ShooterState.Pursuing:
                HandlePursuing(playerPos);
                CheckEnrageAndAttack();
                break;

            case ShooterState.PostAttackCooldown:
            case ShooterState.EnragedCooldown:
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0f)
                {
                    currentState = ShooterState.Pursuing;
                    attackTimer = hasEnraged ? enragedAttackInterval : regularAttackInterval;
                }
                break;

            case ShooterState.EnragedRageSequence:
            case ShooterState.Attacking:
                // 协程控制，停止移动
                if (rb != null) rb.velocity = Vector2.zero;
                break;
        }
    }
    private void CheckEnrageAndAttack()
    {
        // 1. 狂暴检查 (仅检查一次)
        if (!hasEnraged && currentHealth <= maxHealth * 0.5f)
        {
            StartCoroutine(EnragedSequence());
            return;
        }

        // 2. 正常/狂暴后攻击检查
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            currentState = ShooterState.Attacking;

            if (!hasEnraged)
            {
                // 普通阶段：随机选择攻击
                if (Random.value < 0.5f)
                {
                    StartCoroutine(Attack_ScatterSix());
                }
                else
                {
                    StartCoroutine(Attack_RandomThreeTracker());
                }
            }
            else
            {
                // 狂暴阶段：随机选择攻击
                if (Random.value < 0.5f)
                {
                    StartCoroutine(Attack_ScatterSix());
                }
                else
                {
                    StartCoroutine(Attack_RandomSixTracker());
                }
            }

            // 重置计时器将在协程结束时处理
        }
    }

    // ⭐️ 狂暴序列 (半血触发)
    private IEnumerator EnragedSequence()
    {
        Debug.Log("FireSixShooter: 🔥 触发狂暴！");
        hasEnraged = true;
        currentState = ShooterState.EnragedRageSequence;
        UpdateAnimator(AnimIdle, true);

        // 1. UI 展示和声音 (展示 1s)
        if (rageUIPanel != null) rageUIPanel.SetActive(true);
        if (audioSource != null && rageSound != null) audioSource.PlayOneShot(rageSound);

        yield return new WaitForSeconds(1f);

        if (rageUIPanel != null) rageUIPanel.SetActive(false);

        // 2. 连续 3 次散射攻击 (每次间隔 0.8s)
        for (int i = 0; i < 3; i++)
        {
            yield return StartCoroutine(Attack_ScatterSix(false));
            yield return new WaitForSeconds(0.8f);
        }

        // 3. 狂暴追踪弹幕 (6颗)
        yield return StartCoroutine(Attack_RandomSixTracker(false));

        // 4. 休息 2s
        Debug.Log("FireSixShooter: 😴 狂暴序列结束，休息 2s。");
        currentState = ShooterState.EnragedCooldown;
        attackTimer = rageCooldownDuration;
    }


    // --- 攻击方法 ---

    // 1. 散射攻击
    private IEnumerator Attack_ScatterSix(bool enterCooldown = true)
    {
        currentState = ShooterState.Attacking;
        UpdateAnimator(AnimScatter, true); // ⭐️ 播放散射动画

        float startAngle = Random.Range(0f, 60f);

        for (int i = 0; i < 6; i++)
        {
            float angle = startAngle + i * 60f;
            Vector2 direction = AngleToVector2(angle);
            SpawnProjectile(projectilePrefab, transform.position, direction, projectileDamage, projectileSpeed);
        }

        yield return new WaitForSeconds(0.2f); // 动画持续时间

        UpdateAnimator(AnimScatter, false);
        if (enterCooldown)
        {
            currentState = ShooterState.PostAttackCooldown;
            attackTimer = hasEnraged ? enragedAttackInterval : regularAttackInterval;
        }
    }

    // 2. 普通追踪弹幕 (3颗)
    private IEnumerator Attack_RandomThreeTracker(bool enterCooldown = true)
    {
        currentState = ShooterState.Attacking;
        UpdateAnimator(AnimTracker, true); // ⭐️ 播放追踪动画

        for (int i = 0; i < 3; i++)
        {
            Vector3 randomOffset = (Vector3)Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = transform.position + randomOffset;
            Vector2 direction = (playerCharacter.transform.position - spawnPosition).normalized;

            SpawnProjectile(projectilePrefab, spawnPosition, direction, projectileDamage, projectileSpeed);
        }

        yield return new WaitForSeconds(0.2f);

        UpdateAnimator(AnimTracker, false);
        if (enterCooldown)
        {
            currentState = ShooterState.PostAttackCooldown;
            attackTimer = regularAttackInterval;
        }
    }

    // 3. 狂暴追踪弹幕 (6颗，可被击毁)
    private IEnumerator Attack_RandomSixTracker(bool enterCooldown = true)
    {
        if (trackingProjectilePrefab == null) { Debug.LogError("FireSixShooter: ❌ 追踪弹幕预制体未设置！"); yield break; }

        currentState = ShooterState.Attacking;
        UpdateAnimator(AnimTracker, true); // ⭐️ 播放追踪动画

        for (int i = 0; i < 6; i++)
        {
            Vector3 randomOffset = (Vector3)Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = transform.position + randomOffset;

            // 实例化追踪弹幕
            // 注意：我们传入 Vector2.zero 方向，因为移动由 DamageableProjectile.Update 控制
            GameObject go = SpawnProjectile(trackingProjectilePrefab, spawnPosition, Vector2.zero, projectileDamage, projectileSpeed);

            // ⭐️ 初始化追踪/可击毁属性
            DamageableProjectile damageable = go.GetComponent<DamageableProjectile>();
            if (damageable != null)
            {
                // 初始化血量和目标
                damageable.Initialize(trackingProjectileHP, playerCharacter);
            }
        }

        yield return new WaitForSeconds(0.2f);

        UpdateAnimator(AnimTracker, false);
        if (enterCooldown)
        {
            currentState = ShooterState.PostAttackCooldown;
            attackTimer = enragedAttackInterval;
        }
    }

    // ⭐️ 动画辅助方法
    private void UpdateAnimator(int hash, bool state)
    {
        if (animator != null)
        {
            animator.SetBool(AnimScatter, false);
            animator.SetBool(AnimTracker, false);
            animator.SetBool(AnimIdle, false);

            if (state)
            {
                animator.SetBool(hash, true);
            }
            else
            {
                // 如果是关闭状态，则切换回 Idle
                animator.SetBool(AnimIdle, true);
            }
        }
    }

    // --- 辅助方法 ---

    // 将角度转换为 Vector2 (0度为右侧)
    private Vector2 AngleToVector2(float angle)
    {
        // 转换为弧度，注意 Unity 的 0 度通常是 X 轴正方向
        float radians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    // 统一弹幕生成逻辑
    private GameObject SpawnProjectile(GameObject prefab, Vector3 position, Vector2 direction, int damage, float speed)
    {
        GameObject go = Instantiate(prefab, position, Quaternion.identity);

        // 假设弹幕预制体上有一个 Projectile 脚本
        Projectile projectile = go.GetComponent<Projectile>();
        if (projectile != null)
        {
            // 假设 Projectile 有一个 Initialize 方法来设置方向、速度和伤害
            projectile.Initialize(direction, speed, damage, "Player");
        }
        else
        {
            // 默认行为：如果找不到 Projectile 脚本，使用 Rigidbody2D 推动
            Rigidbody2D projRb = go.GetComponent<Rigidbody2D>();
            if (projRb != null)
            {
                projRb.velocity = direction * speed;
            }
        }
        return go;
    }

    private void HandlePursuing(Vector3 playerPos)
    {
        // 怪物向玩家移动的逻辑
        transform.position = Vector3.MoveTowards(transform.position, playerPos, moveSpeed * Time.deltaTime);
    }

    private void HandleFlip(Vector3 targetPos)
    {
        // 翻转逻辑 (追逐时翻转)
        if (spriteRenderer != null && (currentState == ShooterState.Pursuing || currentState == ShooterState.PostAttackCooldown))
        {
            Vector2 direction = (targetPos - transform.position).normalized;
            if (direction.x > 0) spriteRenderer.flipX = true;
            else if (direction.x < 0) spriteRenderer.flipX = false;
        }
    }

    // ⭐️ 死亡重写
    public override void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("FireSixShooter: 💥 死亡。");
        // 停止所有协程，清理
        StopAllCoroutines();

        base.Die();
    }

    // Gizmos (可选)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}