using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerCharacter playerCharacter;

    // --- 数值字段 ---
    private float maxHealth;
    private int attackDamage; // 【修改】
    private float attackCooldown;
    private float moveSpeed;
    private int experienceReward;
    private int coinDropAmount; // 【新增】

    // --- 内部状态变量 ---
    private float currentHealth;
    private float timeSinceLastAttack = 0f;

    public void Initialize(EnemyData data)
    {
        this.maxHealth = data.maxHealth;
        this.attackDamage = data.attackDamage; // 【修改】
        this.attackCooldown = data.attackCooldown;
        this.moveSpeed = data.moveSpeed;
        this.experienceReward = data.experienceReward;
        this.coinDropAmount = data.coinDropAmount; // 【新增】

        this.currentHealth = this.maxHealth;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCharacter = FindObjectOfType<PlayerCharacter>();

        if (playerCharacter == null)
        {
            UnityEngine.Debug.LogError("场景中找不到 PlayerCharacter 实例！");
            this.enabled = false;
        }

        timeSinceLastAttack = attackCooldown;
    }

    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (playerCharacter != null)
        {
            Transform playerTransform = playerCharacter.transform;
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

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
        if (playerCharacter != null)
        {
            // 给予玩家经验
            playerCharacter.GainExperience(experienceReward);

            // 给予玩家金币 
            playerCharacter.GainCoins(coinDropAmount);
        }
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
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