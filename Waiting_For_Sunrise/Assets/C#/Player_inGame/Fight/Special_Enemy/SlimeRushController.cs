using UnityEngine;

// 继承自基础控制器
[RequireComponent(typeof(Animator))]
public class SlimeRushController : EnemyController
{
    // --- 配置字段 (保持不变) ---
    [Header("冲锋史莱姆配置")]
    [Tooltip("触发冲锋的玩家距离")]
    [SerializeField] private float triggerDistance = 6f;
    [Tooltip("准备冲锋的持续时间 (秒)")]
    [SerializeField] private float chargeDuration = 0.8f;
    [Tooltip("冲锋时的移动速度")]
    [SerializeField] private float rushSpeed = 10f;
    [Tooltip("冲锋后静止的持续时间 (秒)")]
    [SerializeField] private float stunDuration = 0.5f;
    [Tooltip("两次冲锋间的冷却时间 (秒)")]
    [SerializeField] private float cooldownDuration = 2f;
    [Tooltip("冲锋目标点周围的随机半径")]
    [SerializeField] private float rushTargetRadius = 0.5f;

    [Header("冲锋轨迹视觉效果")]
    [Tooltip("用于绘制冲锋轨迹的 LineRenderer 组件")]
    [SerializeField] private LineRenderer lineRenderer;
    [Tooltip("冲锋轨迹线的最大长度")]
    [SerializeField] private float maxLineLength = 8f;

    // --- 内部状态 ---
    private enum RushState { Pursuing, Charging, Rushing, Cooldown, Dead }
    private RushState currentState = RushState.Pursuing;
    private Animator animator;

    // 计时器
    private float stateTimer = 0f;
    private Vector3 rushTargetPosition;

    // 动画 Hash ID
    private readonly int AnimIsRushing = Animator.StringToHash("IsRushing");
    private readonly int AnimIsCharging = Animator.StringToHash("IsCharging");
    private readonly int AnimIsStunned = Animator.StringToHash("IsStunned");

    // ⭐️ 初始化：保持不变
    public override void Initialize(EnemyData data, PlayerCharacter player)
    {
        base.Initialize(data, player);
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("RushSlime: ❌ 找不到 Animator 组件!");
            this.enabled = false;
            return;
        }

        if (lineRenderer == null)
        {
            lineRenderer = GetComponentInChildren<LineRenderer>();
        }
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }

        Debug.Log("RushSlime: ✅ 初始化成功。");
    }

    // ⭐️ Update 方法被移除，依赖父类调用 HandleMovementAndFlip

    // ⭐️ 核心：重写移动逻辑，实现状态机
    public override void HandleMovementAndFlip()
    {
        if (currentState == RushState.Dead) return;

        // 1. 被击退时暂停自主移动 (通用逻辑)
        if (rb != null && rb.velocity.magnitude >= 0.1f)
        {
            Debug.Log($"RushSlime: ⚠️ 被击退中，速度: {rb.velocity.magnitude:F2}");
            return;
        }

        // 确保 playerCharacter 不为空
        if (playerCharacter == null) return;

        Vector3 playerPos = playerCharacter.transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);

        // ⭐️ 强制日志：追踪状态和距离
        // 请在 Console 中启用 'Info' 级别日志以查看此条。
        Debug.Log($"RushSlime: 🔁 当前状态: {currentState} | 距离: {distanceToPlayer:F2}");

        // 2. 状态机逻辑
        switch (currentState)
        {
            case RushState.Pursuing:
                HandlePursuing(distanceToPlayer, playerPos);
                break;

            case RushState.Charging:
                HandleCharging();
                break;

            case RushState.Rushing:
                HandleRushing();
                break;

            case RushState.Cooldown:
                HandleCooldown();
                break;
        }

        // 3. 翻转逻辑 (仅在追逐时翻转)
        if (currentState == RushState.Pursuing && spriteRenderer != null)
        {
            Vector2 direction = (playerPos - transform.position).normalized;
            if (direction.x > 0) spriteRenderer.flipX = true;
            else if (direction.x < 0) spriteRenderer.flipX = false;
        }
    }

    // --- 状态处理逻辑 ---

    private void HandlePursuing(float distanceToPlayer, Vector3 playerPos)
    {
        // 1. 移动逻辑：向玩家移动
        if (distanceToPlayer > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, moveSpeed * Time.deltaTime);
        }

        // 2. 状态切换检查：距离足够近 -> 准备冲锋
        if (distanceToPlayer <= triggerDistance)
        {
            // ⭐️ 强制日志：如果史莱姆一直追，这个日志是不会出现的！
            Debug.Log($"RushSlime: 🎯 满足触发距离 ({distanceToPlayer:F2}m)，**切换到 Charging!**");

            currentState = RushState.Charging;
            stateTimer = 0f;

            PrepareRushTarget(playerPos);
            UpdateAnimator(isCharging: true);
        }
    }

    private void PrepareRushTarget(Vector3 playerPos)
    {
        Vector2 randomOffset = Random.insideUnitCircle * rushTargetRadius;
        rushTargetPosition = playerPos + (Vector3)randomOffset;

        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);

            Vector3 directionVector = (rushTargetPosition - transform.position);
            Vector3 trajectoryEnd = transform.position + directionVector.normalized * Mathf.Min(directionVector.magnitude, maxLineLength);
            lineRenderer.SetPosition(1, trajectoryEnd);
        }
    }

    private void HandleCharging()
    {
        // ⭐️ 强制日志：如果代码运行到这里，移动必须停止
        if (rb != null) rb.velocity = Vector2.zero; // 强制停止
        Debug.Log("RushSlime: 🛑 处于 Charging 状态，已强制停止移动。");

        stateTimer += Time.deltaTime;

        if (stateTimer >= chargeDuration)
        {
            Debug.Log("RushSlime: ⚡️ 蓄力完成，发射！");

            if (lineRenderer != null) lineRenderer.enabled = false;

            currentState = RushState.Rushing;
            stateTimer = 0f;
            UpdateAnimator(isRushing: true);
        }
    }

    private void HandleRushing()
    {
        // 冲锋移动 (使用 rushSpeed)
        transform.position = Vector3.MoveTowards(transform.position, rushTargetPosition, rushSpeed * Time.deltaTime);

        // 检查是否到达
        if (Vector3.Distance(transform.position, rushTargetPosition) < 0.1f)
        {
            Debug.Log("RushSlime: 🏁 冲锋结束，进入僵直。");

            currentState = RushState.Cooldown;
            stateTimer = 0f;
            UpdateAnimator(isStunned: true);
        }
    }

    private void HandleCooldown()
    {
        // 僵直/冷却中停止移动
        if (rb != null) rb.velocity = Vector2.zero; // 强制停止
        stateTimer += Time.deltaTime;

        if (stateTimer >= stunDuration + cooldownDuration)
        {
            Debug.Log("RushSlime: ✅ 冷却完毕，恢复追逐。");

            currentState = RushState.Pursuing;
            stateTimer = 0f;
            UpdateAnimator();
        }
    }

    // --- 辅助方法 (保持不变) ---

    private void UpdateAnimator(bool isRushing = false, bool isCharging = false, bool isStunned = false)
    {
        if (animator == null) return;
        animator.SetBool(AnimIsRushing, isRushing);
        animator.SetBool(AnimIsCharging, isCharging);
        animator.SetBool(AnimIsStunned, isStunned);
    }

    public override void Die()
    {
        Debug.Log("RushSlime: 💥 死亡。");
        currentState = RushState.Dead;
        if (lineRenderer != null) lineRenderer.enabled = false;

        base.Die();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerDistance);

        if (currentState == RushState.Charging)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, rushTargetPosition);
            Gizmos.DrawWireSphere(rushTargetPosition, 0.2f);
        }
    }
}