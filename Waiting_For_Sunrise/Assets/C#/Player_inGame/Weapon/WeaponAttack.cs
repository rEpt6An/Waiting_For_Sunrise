using UnityEngine;
using System.Collections.Generic;

public class WeaponAttack : MonoBehaviour
{
    private float _damage;
    private List<Collider2D> _hitEnemies = new List<Collider2D>();

    public void Initialize(float damage)
    {
        this._damage = damage;
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
                enemy.TakeDamage(_damage);
            }
        }
    }
}