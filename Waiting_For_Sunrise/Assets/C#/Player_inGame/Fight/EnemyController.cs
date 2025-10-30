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
    private float timeSinceLastAttack = 0f;

    // --- 修改：不再直接引用Transform，而是引用核心脚本 ---
    private PlayerCharacter playerCharacter;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        // --- 核心修改：在场景中查找唯一的PlayerCharacter实例 ---
        playerCharacter = FindObjectOfType<PlayerCharacter>();

        if (playerCharacter == null)
        {
            UnityEngine.Debug.LogError("场景中找不到 PlayerCharacter 实例！怪物将不会移动或攻击。");
            this.enabled = false;
        }

        // 允许怪物在游戏开始时立即攻击
        timeSinceLastAttack = attackCooldown;
    }

    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        // 只有当玩家目标存在时才执行逻辑
        if (playerCharacter != null)
        {
            Transform playerTransform = playerCharacter.transform;

            // 移动逻辑
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            // 翻转逻辑 (根据你的反馈已调整)
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
        // --- 核心修改：通过缓存的引用给予经验 ---
        if (playerCharacter != null)
        {
            playerCharacter.GainExperience(experienceReward);
        }
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (timeSinceLastAttack >= attackCooldown)
            {
                // --- 核心修改：通过缓存的引用造成伤害 ---
                if (playerCharacter != null)
                {
                    playerCharacter.TakeDamage(contactDamage);
                }
                timeSinceLastAttack = 0f;
            }
        }
    }
}