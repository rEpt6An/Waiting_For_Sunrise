using UnityEngine;
using System.Collections.Generic;

public class WeaponAttack : MonoBehaviour
{
    private float _damage;
    private float _repelForce;
    private Vector2 _attackPosition;
    private List<Collider2D> _hitEnemies = new List<Collider2D>();
    private int _remainingPenetrations;
    private int _attackType;

    public void Initialize(float damage, float repelForce, Vector2 attackPosition, int penetrateCount, int attackType, float meleeLifetime)
    {
        this._damage = damage;
        this._repelForce = repelForce;
        this._attackPosition = attackPosition;
        this._attackType = attackType;

        // 计算可击中敌人总数（1次初始打击 + N次穿透）
        this._remainingPenetrations = penetrateCount + 1;

        if (attackType == 1) // 远程
        {
            Destroy(gameObject, 8f); // 远程保留默认长生命周期
        }
        else // 近战
        {
            Destroy(gameObject, meleeLifetime); // 使用 Data 中设置的生命周期
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 检查是否在本次攻击中已击中过，且还有穿透次数，防止重复伤害
            if (_hitEnemies.Contains(other) && _attackType == 1)
            {
                return;
            }

            EnemyController enemy = other.GetComponent<EnemyController>();

            if (enemy != null)
            {
                // 造成伤害
                enemy.TakeDamage(_damage);

                // 实现击退
                if (_repelForce > 0)
                {
                    Vector2 enemyPosition = other.transform.position;
                    Vector2 repelDirection = (enemyPosition - _attackPosition).normalized;
                    enemy.ApplyRepel(repelDirection, _repelForce);
                }

                // 消耗穿透计数
                if (_attackType == 1) // 远程武器
                {
                    if (_remainingPenetrations > 0)
                    {
                        _remainingPenetrations--;
                        _hitEnemies.Add(other);
                    }

                    // 穿透次数用完，销毁自身
                    if (_remainingPenetrations == 0)
                    {
                        Destroy(gameObject);
                    }
                }
                //else if (_attackType == 0) // 近战武器：立即销毁
                //{
                //    Destroy(gameObject);
                //}
            }
        }
    }
}