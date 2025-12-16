
using UnityEngine;

public class DamageableProjectile : Projectile // 继承自 Projectile
{
    private int currentHP;
    private UnityEngine.Transform target; // 追踪目标
    private float turnSpeed = 0.8f; // 追踪转向速度 (可以在 Inspector 中配置)

    // 重写 Awake 来获取 Rigidbody2D
    new void Awake()
    {
        // 确保调用父类的 Awake 来获取 Rigidbody2D
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    // 初始化时设置血量和目标
    public void Initialize(int hp, PlayerCharacter player)
    {
        currentHP = hp;
        if (player != null)
        {
            target = player.transform;
        }

        // 追踪弹幕不需要初始速度，它的速度由 Update 控制
        // 注意：base.Initialize 已经被父类 Awake() 中的 GetComponent<Projectile>() 替代
        // 我们只需要确保父类 Projectile 的字段被设置
        targetTag = "Player";
        // speed 和 damage 应该由 FireSixShooterController 统一设置
    }

    void Update()
    {
        // 确保 rb 已经获取且目标存在
        if (target != null && rb != null && speed > 0)
        {
            // 追踪逻辑 (使弹幕转向目标)
            Vector2 targetDirection = (target.position - transform.position).normalized;

            // 平滑转向
            Vector2 currentVelocity = rb.velocity.normalized;
            // 如果 currentVelocity 接近零，直接使用 targetDirection
            if (currentVelocity.magnitude < 0.1f)
            {
                currentVelocity = targetDirection;
            }

            Vector2 newDirection = Vector2.Lerp(currentVelocity, targetDirection, Time.deltaTime * turnSpeed);

            // ⭐️ 核心：持续更新 Rigidbody 的速度实现追踪
            rb.velocity = newDirection.normalized * speed;
        }
        // 如果 target 为空，弹幕应该继续沿当前速度飞行（如果 Initialize 时设置了速度）
    }

    // 假设您有一个通用的 TakeDamage 方法，供玩家攻击时调用
    public void TakeDamage(int damageTaken)
    {
        currentHP -= damageTaken;
        if (currentHP <= 0)
        {
            Debug.Log("Tracking Projectile Destroyed by Player!");
            // 播放销毁特效
            Destroy(gameObject);
        }
    }
}