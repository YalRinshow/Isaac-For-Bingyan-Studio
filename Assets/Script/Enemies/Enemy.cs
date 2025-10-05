using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rbEnemy;
    public int collisionDamage = Constants.ENEMY_COLISION_DAMAGE;
    public bool haveCollisionDamage = true;
    public float health;
    public List<ItemManager.ItemType> droppedItems = new List<ItemManager.ItemType>();
    public float speedMove;
    public bool flyingEnemy = false;
    public bool teardropEffective = true;
    public ItemManager.ItemType droppedItemType;
    public bool isElite;
    public virtual void Initialize(bool dropKey)
    {
    }
    public void TakeDamage(float damage)
    {
        if (isElite)
        {
            UIManager.Instance.DecreaseHealth(damage);
        }
        health -= damage;
        if (health < 0.001)
        {
            rbEnemy.velocity = Vector2.zero;
            ItemsDropping();
            Destroy(gameObject);
        }
    }
    public virtual void TakeTeardropDamage(float damage, Vector2 direction)
    {

    }
    public void GetDropItem(bool dropKey = false)
    {
        if (dropKey)
        {
            droppedItemType = ItemManager.ItemType.Key;
            return;
        }
        if (droppedItems.Count == 0) return;
        int randItems = Random.Range(0, droppedItems.Count);
        droppedItemType = droppedItems[randItems];
    }
    public void ItemsDropping()
    {
        ItemManager.GenerateItem(droppedItemType, transform.localPosition);
    }
}
