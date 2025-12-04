// EnemyController.cs (最终修正版)
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerCharacter playerCharacter;

    // --- 数值字段 (现在没有默认值) ---
    private float maxHealth;
    private int attackDamage;
    private float attackCooldown;
    private float moveSpeed;
    private int experienceReward;
    private int coinDropAmount;

    // --- 内部状态变量 ---
    private float currentHealth;
    private float timeSinceLastAttack = 0f;
    private bool isInitialized = false; // 添加一个初始化标志

    /// <summary>
    /// 外部调用的初始化方法，所有设置都在这里完成
    /// </summary>
    public void Initialize(EnemyData data, PlayerCharacter player)
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
        this.timeSinceLastAttack = this.attackCooldown; // 允许立即攻击

        // 获取组件和目标
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.playerCharacter = player;

        // 标记为已初始化
        this.isInitialized = true;
    }

    // Start 方法可以留空，或者用于一些不依赖外部数据的初始化
    void Start()
    {
        // Start 方法现在可以非常干净
    }

    void Update()
    {
        // 只有在完全初始化后才执行逻辑
        if (!isInitialized || playerCharacter == null)
        {
            return;
        }

        timeSinceLastAttack += Time.deltaTime;

        // 移动和翻转逻辑
        Transform playerTransform = playerCharacter.transform;
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        if (direction.x > 0) spriteRenderer.flipX = true;
        else if (direction.x < 0) spriteRenderer.flipX = false;
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

    private void Die()
    {
        if (playerCharacter != null)
        {
            playerCharacter.GainExperience(experienceReward);
             playerCharacter.GainCoins(coinDropAmount); // 确保PlayerCharacter有GainCoins方法
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