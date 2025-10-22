// EnemyController.cs
using UnityEngine;

// 确保对象上有 SpriteRenderer 组件
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyController : MonoBehaviour
{
    private Transform playerTarget;
    private SpriteRenderer spriteRenderer; // 对SpriteRenderer组件的引用
    private float moveSpeed = 0.8f;

    void Start()
    {
        // 获取 SpriteRenderer 组件的引用
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 获取玩家目标
        playerTarget = PlayerMovement.Instance;

        if (playerTarget == null)
        {
            Debug.LogError("无法找到 PlayerMovement.Instance！...");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (playerTarget != null)
        {
            // 1. 计算移动方向并移动
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            // --- 2. 添加精灵翻转逻辑 ---
            // 检查玩家是在敌人的左边还是右边
            if (playerTarget.position.x > transform.position.x)
            {
                // 玩家在右边，精灵朝右 (不翻转)
                // 假设你的原始精灵素材是朝向右的
                spriteRenderer.flipX = true;
            }
            else if (playerTarget.position.x < transform.position.x)
            {
                // 玩家在左边，精灵朝左 (翻转)
                spriteRenderer.flipX = false;
            }
            // 如果x坐标完全相同，则保持上一次的朝向
        }
    }
}