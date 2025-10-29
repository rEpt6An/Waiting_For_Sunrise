// EnemyController.cs
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // --- 怪物属性 ---
    [Header("战斗属性")]
    public float maxHealth = 10f;
    public int contactDamage = 10;
    [Tooltip("怪物攻击一次后的冷却时间（秒）")]
    public float attackCooldown = 1f;

    [Header("移动与成长")]
    public float moveSpeed = 0.8f;
    public int experienceReward = 5;

    // --- 内部状态变量 ---
    private float currentHealth;
    private Transform playerTransform;
    private float timeSinceLastAttack = 0f; // 独立的攻击冷却计时器

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        if (PlayerCharacter.Instance != null)
        {
            playerTransform = PlayerCharacter.Instance.transform;
        }
        else
        {
            UnityEngine.Debug.LogError("无法找到 PlayerCharacter.Instance！...");
            this.enabled = false;
        }

        // 允许怪物在游戏开始时立即攻击
        timeSinceLastAttack = attackCooldown;
    }

    void Update()
    {
        // --- 冷却计时器 ---
        // 每帧都增加计时器的时间
        timeSinceLastAttack += Time.deltaTime;

        if (playerTransform != null)
        {
            // --- 移动和翻转逻辑 ---
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            // 根据你的反馈调整了翻转逻辑
            if (direction.x > 0) spriteRenderer.flipX = true;
            else if (direction.x < 0) spriteRenderer.flipX = false;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (PlayerCharacter.Instance != null)
        {
            PlayerCharacter.Instance.GainExperience(experienceReward);
        }
        Destroy(gameObject);
    }

    // --- 核心修改：使用 OnCollisionStay2D ---
    /// <summary>
    /// 当碰撞体持续接触时，此方法会每帧被调用
    /// </summary>
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 检查是否与玩家持续碰撞
        if (collision.gameObject.CompareTag("Player"))
        {
            // 检查冷却时间是否已到
            if (timeSinceLastAttack >= attackCooldown)
            {
                // 如果冷却时间到了，就执行攻击
                if (PlayerCharacter.Instance != null)
                {
                    PlayerCharacter.Instance.TakeDamage(contactDamage);
                }

                // 攻击后，立即重置计时器
                timeSinceLastAttack = 0f;
            }
        }
    }
}