using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rbEnemy;
    public int collisionDamage = Constants.ENEMY_COLISION_DAMAGE;

    public bool isEilite = false;
    public bool haveCollisionDamage = true;
    public float health;
    public HashSet<string> droppedItems;
    public float speedMove;

    public bool flyingEnemy = false;

    public bool teardropEffective = true;
    public virtual void Initialize(bool eilite)
    {
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0.001)
        {
            ItemsDropping();
            Destroy(gameObject);
        }
    }
    public virtual void TakeTeardropDamage(float damage, Vector2 direction)
    {

    }
    public void ItemsDropping()
    {

    }
}
