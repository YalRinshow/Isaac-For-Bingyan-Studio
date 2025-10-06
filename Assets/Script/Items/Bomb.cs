using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float lifetime = Constants.BOMB_LIFETIME;
    private float damageRadius = Constants.BOMB_RADIUS;
    private int bombDamage = Constants.BOMB_DAMAGE;
    public Collider2D explosionCollider;
    private void Start()
    {
        explosionCollider.enabled = false;
        Invoke("Explode", lifetime);
    }
    private void Explode()
    {
        explosionCollider.enabled = true;
        DamageToObjects();
        Destroy(gameObject);
    }
    private void DamageToObjects()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(Constants.TAG_ENEMY))
            {
                var enemy = hitCollider.GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    if (enemy.isElite)
                    {
                        enemy.TakeHpPercentDamage(0.2f);
                    }
                    else
                    {
                        enemy.TakeDamage(bombDamage);
                    }
                }
            }
            else if (hitCollider.CompareTag(Constants.TAG_PLAYER))
            {
                var player = hitCollider.GetComponentInParent<Player>();
                if (player != null)
                {
                    Player.Instance.TakeDamage(1);
                }
            }
            else if (hitCollider.CompareTag(Constants.TAG_GROUND))
            {
                var ground = hitCollider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (ground is Rock)
                    {
                        ground.Exploded();
                    }
                }
            }
        }
    }
}