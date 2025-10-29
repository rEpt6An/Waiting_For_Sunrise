// EnemyController.cs
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
    private Transform playerTransform;
    private float timeSinceLastAttack = 0f; // �����Ĺ�����ȴ��ʱ��

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
            UnityEngine.Debug.LogError("�޷��ҵ� PlayerCharacter.Instance��...");
            this.enabled = false;
        }

        // �����������Ϸ��ʼʱ��������
        timeSinceLastAttack = attackCooldown;
    }

    void Update()
    {
        // --- ��ȴ��ʱ�� ---
        // ÿ֡�����Ӽ�ʱ����ʱ��
        timeSinceLastAttack += Time.deltaTime;

        if (playerTransform != null)
        {
            // --- �ƶ��ͷ�ת�߼� ---
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            // ������ķ��������˷�ת�߼�
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

    // --- �����޸ģ�ʹ�� OnCollisionStay2D ---
    /// <summary>
    /// ����ײ������Ӵ�ʱ���˷�����ÿ֡������
    /// </summary>
    private void OnCollisionStay2D(Collision2D collision)
    {
        // ����Ƿ�����ҳ�����ײ
        if (collision.gameObject.CompareTag("Player"))
        {
            // �����ȴʱ���Ƿ��ѵ�
            if (timeSinceLastAttack >= attackCooldown)
            {
                // �����ȴʱ�䵽�ˣ���ִ�й���
                if (PlayerCharacter.Instance != null)
                {
                    PlayerCharacter.Instance.TakeDamage(contactDamage);
                }

                // �������������ü�ʱ��
                timeSinceLastAttack = 0f;
            }
        }
    }
}