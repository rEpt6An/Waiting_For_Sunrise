// SlimeBombController.cs (最终版本 - 增加爆炸视觉效果)
using UnityEngine;

// 继承自基础控制器
public class SlimeBombController : EnemyController
{
    [Header("Slime Bomb 配置")]
    [Tooltip("爆炸对玩家造成的伤害值 (Int)")]
    [SerializeField] private int explosionDamage = 8;
    [Tooltip("爆炸的半径")]
    [SerializeField] private float explosionRadius = 1.5f;

    // ⭐️ 新增字段：拖拽您在步骤一中创建的爆炸效果预制体到 Inspector
    [Header("视觉效果")]
    [Tooltip("爆炸时的红色圆形特效预制体")]
    [SerializeField] private GameObject explosionVFXPrefab;

    // 攻击阶段的常量 (可根据 Inspector 配置调整)
    private const float TRIGGER_DISTANCE = 3f;
    private const float ATTACK_DISTANCE = 1f;
    private const float ATTACK_SPEED = 0.5f;
    private const float INITIAL_SPEED_MULTIPLIER = 1.5f;
    private const float FLASH_INTERVAL = 0.1f;
    private const float EXPLOSION_DELAY = 0.5f;

    private enum SlimeState { Pursuing, Exploding, Dead }
    private SlimeState currentState = SlimeState.Pursuing;

    private float explosionTimer = 0f;
    private float flashTimer = 0f;

    // ... (Initialize, Update, HandleMovementAndFlip 等方法保持不变) ...

    public override void Initialize(EnemyData data, PlayerCharacter player)
    {
        base.Initialize(data, player);
        Debug.Log("SlimeBombController: ✅ 成功执行 Initialize()，isInitialized 已设置为 true。");
    }

    public override void Update()
    {
        if (!isInitialized || playerCharacter == null)
        {
            if (!isInitialized)
            {
                Debug.LogError("SlimeBombController: ❌ 未初始化或找不到玩家！(检查 Spawner 是否调用 Initialize)");
            }
            return;
        }
        base.Update();
    }

    public override void HandleMovementAndFlip()
    {
        // 阻止被击退时的自主移动
        if (rb.velocity.magnitude >= 0.1f) return;

        Vector3 playerPos = playerCharacter.transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);

        // 状态机核心逻辑
        switch (currentState)
        {
            case SlimeState.Pursuing:
                HandlePursuing(distanceToPlayer, playerPos);
                break;

            case SlimeState.Exploding:
                HandleExploding(distanceToPlayer, playerPos);
                break;

            case SlimeState.Dead:
                break;
        }

        // 处理翻转逻辑
        Vector2 direction = (playerPos - transform.position).normalized;
        if (spriteRenderer != null)
        {
            if (direction.x > 0) spriteRenderer.flipX = true;
            else if (direction.x < 0) spriteRenderer.flipX = false;
        }
    }

    public override void Die()
    {
        if (currentState != SlimeState.Exploding)
        {
            Debug.Log($"SlimeBombController: 💥 史莱姆在 Pursuing 状态被击杀，直接销毁。");
            base.Die();
        }
    }

    private void HandlePursuing(float distanceToPlayer, Vector3 playerPos)
    {
        float currentSpeed = moveSpeed;
        if (distanceToPlayer >= TRIGGER_DISTANCE)
        {
            currentSpeed *= INITIAL_SPEED_MULTIPLIER;
        }

        if (distanceToPlayer > ATTACK_DISTANCE)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, currentSpeed * Time.deltaTime);
        }

        if (distanceToPlayer <= ATTACK_DISTANCE)
        {
            currentState = SlimeState.Exploding;
            explosionTimer = 0f;
            flashTimer = 0f;
            Debug.Log($"SlimeBombController: 💣 距离 {distanceToPlayer:F2}m，**进入爆炸阶段** ({ATTACK_DISTANCE}米)。");
        }
    }

    private void HandleExploding(float distanceToPlayer, Vector3 playerPos)
    {
        // 1. 缓慢移动
        if (distanceToPlayer > ATTACK_DISTANCE)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, ATTACK_SPEED * Time.deltaTime);
        }
        // ... (闪烁和计时逻辑保持不变) ...

        explosionTimer += Time.deltaTime;
        flashTimer += Time.deltaTime;

        if (spriteRenderer != null)
        {
            if (flashTimer >= FLASH_INTERVAL)
            {
                spriteRenderer.color = spriteRenderer.color == Color.red ? Color.white : Color.red;
                flashTimer = 0f;
            }
        }

        if (explosionTimer >= EXPLOSION_DELAY)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (currentState == SlimeState.Dead) return;

        currentState = SlimeState.Dead;

        // ⭐️ 核心功能：实例化爆炸视觉效果
        if (explosionVFXPrefab != null)
        {
            // 实例化 VFX
            GameObject vfxInstance = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);

            // 计算需要的缩放比例 (基于预制体的初始大小)
            // 假设圆形 Sprite 的初始尺寸是 1x1 单位
            float targetScale = explosionRadius * 2f; // 因为半径是 diameter/2
            vfxInstance.transform.localScale = new Vector3(targetScale, targetScale, 1f);

            // 确保特效脚本 ExplosionsVFX 存在，并启动淡出
            ExplosionVFX vfxController = vfxInstance.GetComponent<ExplosionVFX>();
            if (vfxController != null)
            {
                vfxController.StartFadeOutAndDestroy();
            }
            else
            {
                // 如果没有特效脚本，至少在短时间后销毁它
                Destroy(vfxInstance, 0.5f);
            }
        }

        // ... (伤害逻辑保持不变) ...
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerCharacter player = hit.GetComponent<PlayerCharacter>();
                if (player != null)
                {
                    player.TakeDamage(explosionDamage);
                    Debug.Log($"SlimeBombController: 🎯 玩家在爆炸范围内，受到 {explosionDamage} 伤害。");
                }
            }
        }

        // 2. 销毁史莱姆
        base.Die();
    }

    // 绘制爆炸范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}