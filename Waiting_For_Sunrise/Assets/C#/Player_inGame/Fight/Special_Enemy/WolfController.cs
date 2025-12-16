using UnityEngine;
using System.Collections; // 用于协程

[RequireComponent(typeof(Animator))]
public class WolfController : EnemyController
{
    // --- 攻击与范围配置 ---
    [Header("狼王攻击配置")]
    [Tooltip("近战挥击攻击尺寸 (宽度和高度都为 2)")]
    [SerializeField] private Vector2 meleeBoxSize = new Vector2(2f, 2f);
    [Tooltip("近战攻击必选距离阈值")]
    [SerializeField] private float meleeThreshold = 3f;
    [Tooltip("跳跃攻击必选距离阈值")]
    [SerializeField] private float jumpThreshold = 8f;
    [Tooltip("跳跃攻击的伤害半径")]
    [SerializeField] private float jumpDamageRadius = 1f;

    [Header("状态时间配置")]
    [Tooltip("近战挥击后的闲置时间")]
    [SerializeField] private float idleAfterAttackDuration = 1.5f;
    [Tooltip("跳跃攻击的准备时间")]
    [SerializeField] private float jumpChargeDuration = 0.5f;

    [Header("轨迹效果")]
    [Tooltip("用于绘制攻击轨迹的 LineRenderer 组件")]
    [SerializeField] private LineRenderer lineRenderer;
    [Tooltip("轨迹线的最大显示长度 (用于跳跃轨迹限制)")]
    [SerializeField] private float maxLineLength = 10f;
    // ⭐️ 新增：轨迹线颜色，确保不是紫色
    [Tooltip("轨迹线的颜色")]
    [SerializeField] private Color trajectoryColor = Color.red;


    // --- 内部状态 ---
    private enum WolfState { Pursuing, IdleAfterAttack, JumpReady, Jumping, AttackMelee, Dead }
    private WolfState currentState = WolfState.Pursuing;
    private Animator animator;

    private float stateTimer = 0f;
    private Vector3 jumpTargetPosition;
    private bool isJumpDamageApplied = false;

    // 动画 Hash ID
    private readonly int AnimWalk = Animator.StringToHash("Walk");
    private readonly int AnimIdle = Animator.StringToHash("Idle");
    private readonly int AnimAtk = Animator.StringToHash("Atk");
    private readonly int AnimJumpReady = Animator.StringToHash("JumpReady");
    private readonly int AnimJump = Animator.StringToHash("Jump");

    // ⭐️ 初始化
    public override void Initialize(EnemyData data, PlayerCharacter player)
    {
        base.Initialize(data, player);
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("WolfController: ❌ 找不到 Animator 组件!");
            this.enabled = false;
            return;
        }

        if (lineRenderer == null)
        {
            // 尝试在子对象中查找 LineRenderer
            lineRenderer = GetComponentInChildren<LineRenderer>();
        }
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            // ⭐️ 初始化时设置颜色，方便统一调整
            lineRenderer.startColor = trajectoryColor;
            lineRenderer.endColor = trajectoryColor;
            // 💡 提示：您仍然需要在 Inspector 中为 LineRenderer 设置一个正确的 Material。
        }

        UpdateAnimator(WolfState.Pursuing);
        Debug.Log("WolfController: ✅ 初始化成功。");
    }

    // ⭐️ 核心：重写移动逻辑，实现状态机
    public override void HandleMovementAndFlip()
    {
        if (currentState == WolfState.Dead) return;

        // 击退逻辑优先
        if (rb != null && rb.velocity.magnitude >= 0.1f) return;

        if (playerCharacter == null) return;
        Vector3 playerPos = playerCharacter.transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);

        stateTimer += Time.deltaTime;

        // 状态机
        switch (currentState)
        {
            case WolfState.Pursuing:
                HandlePursuing(distanceToPlayer, playerPos);
                break;
            case WolfState.IdleAfterAttack:
                HandleIdleAfterAttack();
                break;
            case WolfState.JumpReady:
                HandleJumpReady();
                break;
            case WolfState.Jumping:
                HandleJumping();
                break;
            case WolfState.AttackMelee:
                // 攻击逻辑通过协程控制，这里只需要保持静止
                break;
        }

        // 翻转逻辑 (追逐时翻转)
        if (currentState == WolfState.Pursuing)
        {
            Vector2 direction = (playerPos - transform.position).normalized;

            if (spriteRenderer != null)
            {
                if (direction.x > 0) spriteRenderer.flipX = true;
                else if (direction.x < 0) spriteRenderer.flipX = false;
            }
        }
    }

    // --- 状态处理逻辑 (保持不变) ---

    private void HandlePursuing(float distanceToPlayer, Vector3 playerPos)
    {
        // 1. 移动：向玩家移动
        if (distanceToPlayer > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, moveSpeed * Time.deltaTime);
        }

        // 只有在冷却计时器结束后才检查攻击切换
        if (stateTimer < 0.5f) return;

        // 2. 状态切换检查
        if (distanceToPlayer <= meleeThreshold)
        {
            StartMeleeAttack();
        }
        else if (distanceToPlayer >= jumpThreshold)
        {
            StartJumpReady(playerPos);
        }
        else // 距离在 3m-8m：随机选择攻击
        {
            if (Random.value < 0.5f) // 50% 概率挥击
            {
                StartMeleeAttack();
            }
            else // 50% 概率跳跃
            {
                StartJumpReady(playerPos);
            }
        }
    }

    private void HandleIdleAfterAttack()
    {
        if (rb != null) rb.velocity = Vector2.zero; // 强制停止
        if (stateTimer >= idleAfterAttackDuration)
        {
            currentState = WolfState.Pursuing;
            stateTimer = 0f;
            UpdateAnimator(WolfState.Pursuing);
        }
    }

    private void HandleJumpReady()
    {
        if (rb != null) rb.velocity = Vector2.zero; // 强制停止
        if (stateTimer >= jumpChargeDuration)
        {
            // 准备结束，开始跳跃
            StartCoroutine(PerformJumpAttack());
        }
    }

    private void HandleJumping()
    {
        // 协程负责移动和状态切换
        if (isJumpDamageApplied) return;

        // 落地时应用伤害
        if (Vector3.Distance(transform.position, jumpTargetPosition) < 0.1f)
        {
            ApplyJumpDamage();
            isJumpDamageApplied = true;

            // 跳跃完成后，进入闲置状态
            currentState = WolfState.IdleAfterAttack;
            stateTimer = 0f;
            UpdateAnimator(WolfState.IdleAfterAttack);
        }
    }

    // --- 攻击实现 (保持不变) ---

    private void StartMeleeAttack()
    {
        currentState = WolfState.AttackMelee;
        stateTimer = 0f; // 重置计时器
        StartCoroutine(PerformMeleeAttack());
    }

    private IEnumerator PerformMeleeAttack()
    {
        if (rb != null) rb.velocity = Vector2.zero; // 攻击期间强制静止

        // 轨迹显示
        StartCoroutine(DrawMeleeSwipe(0.4f));

        animator.SetBool(AnimAtk, true);

        yield return new WaitForSeconds(0.4f); // 等待攻击动画时间结束

        CheckSwipeDamage(); // 伤害判定

        animator.SetBool(AnimAtk, false);

        // 切换到 IdleAfterAttack 状态
        currentState = WolfState.IdleAfterAttack;
        stateTimer = 0f;
        UpdateAnimator(WolfState.IdleAfterAttack);
    }

    private void CheckSwipeDamage()
    {
        // ... (伤害判定逻辑保持不变) ...
        float directionFactor = spriteRenderer.flipX ? 1 : -1;
        float xOffset = directionFactor * (meleeBoxSize.x / 2f);
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(xOffset, 0f);
        float angle = 0f;
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, meleeBoxSize, angle);

        foreach (Collider2D h in hits)
        {
            if (h.CompareTag("Player"))
            {
                playerCharacter.TakeDamage(attackDamage);
                Debug.Log("WolfController: 🐺 近战挥击命中! (2x2 Box)");
                break;
            }
        }
    }

    private void StartJumpReady(Vector3 playerPos)
    {
        currentState = WolfState.JumpReady;
        stateTimer = 0f;
        isJumpDamageApplied = false;

        // 计算目标点：在玩家周围 3m 随机位置
        Vector2 randomOffset = Random.insideUnitCircle.normalized * 3f;
        jumpTargetPosition = playerPos + (Vector3)randomOffset;

        animator.SetBool(AnimJumpReady, true);

        UpdateAnimator(WolfState.JumpReady);

        // 轨迹显示
        StartCoroutine(DrawJumpTrajectory(jumpTargetPosition, jumpChargeDuration));
    }

    private IEnumerator PerformJumpAttack()
    {
        currentState = WolfState.Jumping;

        animator.SetBool(AnimJumpReady, false);
        animator.SetBool(AnimJump, true);

        UpdateAnimator(WolfState.Jumping);

        Vector3 startPos = transform.position;
        float jumpTime = 0.5f; // 假设跳跃过程持续 0.5s

        // 模拟跳跃动画移动
        float elapsed = 0f;
        while (elapsed < jumpTime)
        {
            float t = elapsed / jumpTime;
            // 简单线性插值模拟水平移动
            transform.position = Vector3.Lerp(startPos, jumpTargetPosition, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 确保最终位置准确
        transform.position = jumpTargetPosition;

        animator.SetBool(AnimJump, false);

        // 落地伤害判定和状态切换在 HandleJumping 中完成
    }

    private void ApplyJumpDamage()
    {
        // ... (伤害判定逻辑保持不变) ...
        Collider2D[] hits = Physics2D.OverlapCircleAll(jumpTargetPosition, jumpDamageRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerCharacter.TakeDamage(attackDamage);
                Debug.Log("WolfController: 💣 跳跃落地范围命中!");
                break;
            }
        }
    }

    // --- 动画辅助方法 & Die (保持不变) ---

    private void UpdateAnimator(WolfState state)
    {
        if (animator == null) return;

        // 重置 Walk 和 Idle (攻击/跳跃 bool 在协程中管理)
        animator.SetBool(AnimWalk, false);
        animator.SetBool(AnimIdle, false);

        // ⭐️ 确保非 Pursuing, IdleAfterAttack, JumpReady 时，Walk/Idle 都是 false
        // 确保正在进行的攻击动画 bool 不受影响

        switch (state)
        {
            case WolfState.Pursuing:
                animator.SetBool(AnimWalk, true);
                break;
            case WolfState.IdleAfterAttack:
            case WolfState.JumpReady:
                animator.SetBool(AnimIdle, true);
                break;
            default:
                // AttackMelee, Jumping, Dead 状态时不设置 Walk/Idle
                break;
        }
    }

    public override void Die()
    {
        Debug.Log("WolfController: 💥 死亡。");
        currentState = WolfState.Dead;
        UpdateAnimator(WolfState.Dead);

        if (lineRenderer != null) lineRenderer.enabled = false; // 隐藏轨迹

        base.Die();
    }

    // --- 轨迹绘制协程 ---

    // 绘制挥击的 2x2 矩形轨迹
    private IEnumerator DrawMeleeSwipe(float duration)
    {
        if (lineRenderer == null) yield break;

        float timer = 0f;
        lineRenderer.enabled = true;
        lineRenderer.loop = true;
        lineRenderer.positionCount = 4;

        // ⭐️ 解决颜色问题：设置 LineRenderer 颜色（如果 LineRenderer 的材质支持 Color Gradient）
        // 推荐在 LineRenderer 组件中设置 Start Color 和 End Color，或者使用 Color Gradient 属性
        // 但如果 LineRenderer 的材质是丢失的，下面这行代码可能无效。
        lineRenderer.startColor = trajectoryColor;
        lineRenderer.endColor = trajectoryColor;

        while (timer < duration)
        {
            // 实时获取怪物朝向
            float directionFactor = spriteRenderer.flipX ? 1 : -1;

            // 计算 Box 的中心点
            float xOffsetCenter = directionFactor * (meleeBoxSize.x / 2f);
            Vector2 boxCenter = (Vector2)transform.position + new Vector2(xOffsetCenter, 0f);

            Vector2 halfSize = meleeBoxSize / 2f;

            // 计算四个角点，相对于 BoxCenter 
            // 确保绘制顺序正确，形成闭合矩形
            Vector3 p1 = (Vector3)boxCenter + new Vector3(directionFactor * halfSize.x, halfSize.y);   // 前上
            Vector3 p2 = (Vector3)boxCenter + new Vector3(directionFactor * halfSize.x, -halfSize.y);  // 前下
            Vector3 p3 = (Vector3)boxCenter + new Vector3(-directionFactor * halfSize.x, -halfSize.y); // 后下
            Vector3 p4 = (Vector3)boxCenter + new Vector3(-directionFactor * halfSize.x, halfSize.y);  // 后上

            // 按照顺序连接
            lineRenderer.SetPosition(0, p1);
            lineRenderer.SetPosition(1, p2);
            lineRenderer.SetPosition(2, p3);
            lineRenderer.SetPosition(3, p4);

            timer += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
    }

    // 绘制跳跃的直线轨迹
    private IEnumerator DrawJumpTrajectory(Vector3 targetPos, float duration)
    {
        if (lineRenderer == null) yield break;

        lineRenderer.enabled = true;
        lineRenderer.loop = false; // 直线不闭合
        lineRenderer.positionCount = 2; // 直线指示器

        // ⭐️ 设置颜色
        lineRenderer.startColor = trajectoryColor;
        lineRenderer.endColor = trajectoryColor;

        float timer = 0f;
        while (timer < duration)
        {
            Vector3 startPos = transform.position; // 实时更新起点

            // ⭐️ 优化：限制轨迹长度 (参考 SlimeRushController)
            Vector3 directionVector = targetPos - startPos;
            float distance = directionVector.magnitude;

            // 限制轨迹长度：如果距离超过 maxLineLength，则截断
            Vector3 trajectoryEnd = startPos + directionVector.normalized * Mathf.Min(distance, maxLineLength);

            lineRenderer.SetPosition(0, startPos); // 实时更新起点
            lineRenderer.SetPosition(1, trajectoryEnd); // 目标点不变

            timer += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
    }


}