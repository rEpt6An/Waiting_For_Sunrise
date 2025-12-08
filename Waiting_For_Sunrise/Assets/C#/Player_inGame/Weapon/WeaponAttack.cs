// WeaponAttack.cs
using UnityEngine;
using System.Collections.Generic;

public class WeaponAttack : MonoBehaviour
{
    private float _damage;
    private float _repelForce;
    private Vector2 _attackPosition; // 记录攻击发起点 (例如玩家的中心位置)
    private List<Collider2D> _hitEnemies = new List<Collider2D>();

    // ⭐️ 初始化方法更新：接受击退数据
    public void Initialize(float damage, float repelForce, Vector2 attackPosition)
    {
        this._damage = damage;
        this._repelForce = repelForce;
        this._attackPosition = attackPosition;

        // 销毁碰撞体，近战武器通常是瞬时伤害，远程武器可能需要根据弹药类型设置更长的生命周期
        Destroy(gameObject, 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !_hitEnemies.Contains(other))
        {
            _hitEnemies.Add(other);
            EnemyController enemy = other.GetComponent<EnemyController>();

            if (enemy != null)
            {
                // 造成伤害
                enemy.TakeDamage(_damage);

                // 实现击退
                if (_repelForce > 0)
                {
                    // 计算击退方向：从攻击发起点(_attackPosition)指向被击中的敌人
                    Vector2 enemyPosition = other.transform.position;
                    Vector2 repelDirection = (enemyPosition - _attackPosition).normalized;

                    enemy.ApplyRepel(repelDirection, _repelForce);
                }
            }
        }
    }
}