using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Ground
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG_PLAYER))
        {
            Player.Instance.TakeDamage(Constants.SPIKES_DAMAGE_TO_PLAYER);
        }
        else if (collision.gameObject.CompareTag(Constants.TAG_ENEMY))
        {
            var enemy = collision.gameObject.GetComponentInParent<Enemy>();
            if (!enemy.flyingEnemy) enemy.TakeDamage(Constants.SPIKES_DAMAGE_TO_ENEMY);
        }
    }
}
