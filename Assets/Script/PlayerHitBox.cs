using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponentInParent<Enemy>();
            if (enemy.haveCollisionDamage)
            {
                Player.Instance.TakeDamage(enemy.collisionDamage);
            }
        }
    }
}
