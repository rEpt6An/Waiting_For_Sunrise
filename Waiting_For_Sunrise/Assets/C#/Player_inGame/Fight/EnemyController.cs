using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    // ⭐️ 保护字段：允许子类访问
    protected SpriteRenderer spriteRenderer;
    protected PlayerCharacter playerCharacter;
    protected Rigidbody2D rb;

    // --- 数值字段 (改为 protected) ---
    protected float maxHealth;
    protected int attackDamage;
    protected float attackCooldown;
    protected float moveSpeed;
    protected int experienceReward;
    protected int coinDropAmount;

    // --- 内部状态变量 (改为 protected) ---
    protected float currentHealth;
    protected float timeSinceLastAttack = 0f;
    protected bool isInitialized = false;

    /// <summary>
    /// 外部调用的初始化方法，设为 virtual 允许子类重写。
    /// </summary>
    public virtual void Initialize(EnemyData data, PlayerCharacter player) // ⭐️ 设为 virtual
    {
        // 设置所有数值
        this.maxHealth = data.maxHealth;
        this.attackDamage = data.attackDamage;
        this.attackCooldown = data.attackCooldown;
        this.moveSpeed = data.moveSpeed;
        this.experienceReward = data.experienceReward;
        this.coinDropAmount = data.coinDropAmount;

        // 初始化状态
        this.currentHealth = this.maxHealth;
        this.timeSinceLastAttack = this.attackCooldown;

        // 获取组件和目标
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.playerCharacter = player;

        // 标记为已初始化
        this.isInitialized = true;
        this.rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Start 方法现在可以非常干净
    }

    // ⭐️ Update 方法设为 public virtual，允许子类重写并只调用计时和移动委托
    public virtual void Update() // ⭐️ 设为 public virtual
    {
        // 只有在完全初始化后才执行逻辑
        if (!isInitialized || playerCharacter == null)
        {
            return;
        }

        // 计时器更新
        timeSinceLastAttack += Time.deltaTime;

        // ⭐️ 将移动和翻转逻辑委托给一个可重写的方法
        HandleMovementAndFlip();
    }

    /// <summary>
    /// 默认的移动和翻转逻辑，允许子类重写以禁用或更改默认行为。
    /// </summary>
    public virtual void HandleMovementAndFlip() // ⭐️ 核心修正：新增 public virtual 方法
    {
        // 移动和翻转逻辑 (原 Update 中的代码)
        Transform playerTransform = playerCharacter.transform;
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // 优化移动：如果敌人当前正在被击退，则不执行自身的寻路移动
        if (rb != null && rb.velocity.magnitude < 0.1f)
        {
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }

        if (spriteRenderer != null)
        {
            if (direction.x > 0) spriteRenderer.flipX = true;
            else if (direction.x < 0) spriteRenderer.flipX = false;
        }
    }

    // ApplyRepel 方法保持 public，以便 WeaponAttack 可以调用
    public void ApplyRepel(Vector2 direction, float force)
    {
        if (!isInitialized || rb == null) return;

        rb.velocity = Vector2.zero;

        Vector2 pushVector = direction.normalized * force;
        rb.AddForce(pushVector, ForceMode2D.Impulse);
    }

    public void TakeDamage(float damageAmount)
    {
        if (!isInitialized) return;
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ⭐️ Die 方法改为 public virtual，允许子类重写并调用 base.Die()
    public virtual void Die() // ⭐️ 设为 public virtual
    {
        if (playerCharacter != null)
        {
            playerCharacter.GainExperience(experienceReward);
            playerCharacter.GainCoins(coinDropAmount);
        }
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isInitialized) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            if (timeSinceLastAttack >= attackCooldown)
            {
                if (playerCharacter != null)
                {
                    playerCharacter.TakeDamage(attackDamage);
                }
                timeSinceLastAttack = 0f;
            }
        }
    }
}