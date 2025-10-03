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

    private void Start()
    {
        Initialize();
    }
    public void Initialize(bool eilite = false, HashSet<string>items = null)
    {
        //health = Constants.ENEMY_DEFAULT_HEALTH;
        rbEnemy = GetComponent<Rigidbody2D>();
        isEilite = eilite;
        droppedItems = items;
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
    public void ItemsDropping()
    {

    }
}
