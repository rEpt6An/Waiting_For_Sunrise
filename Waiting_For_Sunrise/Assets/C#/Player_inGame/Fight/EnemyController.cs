// EnemyController.cs
using UnityEngine;

// ȷ���������� SpriteRenderer ���
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyController : MonoBehaviour
{
    private Transform playerTarget;
    private SpriteRenderer spriteRenderer; // ��SpriteRenderer���������
    private float moveSpeed = 0.8f;

    void Start()
    {
        // ��ȡ SpriteRenderer ���������
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ��ȡ���Ŀ��
        playerTarget = PlayerMovement.Instance;

        if (playerTarget == null)
        {
            Debug.LogError("�޷��ҵ� PlayerMovement.Instance��...");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (playerTarget != null)
        {
            // 1. �����ƶ������ƶ�
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            // --- 2. ��Ӿ��鷭ת�߼� ---
            // ���������ڵ��˵���߻����ұ�
            if (playerTarget.position.x > transform.position.x)
            {
                // ������ұߣ����鳯�� (����ת)
                // �������ԭʼ�����ز��ǳ����ҵ�
                spriteRenderer.flipX = true;
            }
            else if (playerTarget.position.x < transform.position.x)
            {
                // �������ߣ����鳯�� (��ת)
                spriteRenderer.flipX = false;
            }
            // ���x������ȫ��ͬ���򱣳���һ�εĳ���
        }
    }
}