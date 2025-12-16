
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // 确保这些字段是 public 或 internal，以便其他脚本（如 DamageableProjectile）访问
    [HideInInspector] public float speed;
    [HideInInspector] public int damage;
    [HideInInspector] public string targetTag; // 目标标签，例如 "Player"

    private Vector2 direction;
    protected Rigidbody2D rb; // 设为 protected，以便子类访问

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Projectile: ❌ Rigidbody2D 组件缺失！弹幕将无法移动。", this);
        }
    }

    public void Initialize(Vector2 dir, float spd, int dmg, string tag)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
        targetTag = tag;

        // 立即设置速度 (只对非追踪弹幕生效)
        if (rb != null && dir != Vector2.zero)
        {
            rb.velocity = direction * speed;
        }

        // 设置销毁计时器，避免弹幕无限存在
        Destroy(gameObject, 5f);
    }

    // ⭐️ 解决：角色扣血和弹幕消失
    void OnTriggerEnter2D(Collider2D other)
    {
        // 检查碰撞体是否为目标
        if (other.CompareTag(targetTag))
        {
            // 假设 PlayerCharacter 脚本在玩家根对象上
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            if (player == null)
            {
                // 如果玩家对象上没有 PlayerCharacter 组件，尝试查找父级
                player = other.GetComponentInParent<PlayerCharacter>();
            }

            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log($"Projectile hit Player! Damage: {damage}");

                // 击中目标后，弹幕消失
                Destroy(gameObject);
            }
        }

        // 如果是可被击毁的弹幕，它可能会与其他对象碰撞
        // 如果是普通弹幕，通常只需要关心目标碰撞。
    }
}