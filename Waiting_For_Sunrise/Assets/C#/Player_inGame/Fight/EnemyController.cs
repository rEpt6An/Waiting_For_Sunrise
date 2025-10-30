using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // --- �������� ---
    [Header("ս������")]
    public float maxHealth = 10f;
    public int contactDamage = 10;
    [Tooltip("���﹥��һ�κ����ȴʱ�䣨�룩")]
    public float attackCooldown = 1f;

    [Header("�ƶ���ɳ�")]
    public float moveSpeed = 0.8f;
    public int experienceReward = 5;

    // --- �ڲ�״̬���� ---
    private float currentHealth;
    private float timeSinceLastAttack = 0f;

    // --- �޸ģ�����ֱ������Transform���������ú��Ľű� ---
    private PlayerCharacter playerCharacter;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        // --- �����޸ģ��ڳ����в���Ψһ��PlayerCharacterʵ�� ---
        playerCharacter = FindObjectOfType<PlayerCharacter>();

        if (playerCharacter == null)
        {
            UnityEngine.Debug.LogError("�������Ҳ��� PlayerCharacter ʵ�������ｫ�����ƶ��򹥻���");
            this.enabled = false;
        }

        // �����������Ϸ��ʼʱ��������
        timeSinceLastAttack = attackCooldown;
    }

    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        // ֻ�е����Ŀ�����ʱ��ִ���߼�
        if (playerCharacter != null)
        {
            Transform playerTransform = playerCharacter.transform;

            // �ƶ��߼�
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            // ��ת�߼� (������ķ����ѵ���)
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
        // --- �����޸ģ�ͨ����������ø��辭�� ---
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
                // --- �����޸ģ�ͨ���������������˺� ---
                if (playerCharacter != null)
                {
                    playerCharacter.TakeDamage(contactDamage);
                }
                timeSinceLastAttack = 0f;
            }
        }
    }
}