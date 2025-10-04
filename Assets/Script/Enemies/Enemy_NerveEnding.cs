using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_NerveEnding : Enemy
{
    public override void Initialize(bool dropKey = false)
    {
        health = 20.0f;
        collisionDamage = 2;
        rbEnemy = GetComponent<Rigidbody2D>();
        droppedItems.Add(ItemManager.ItemType.Key);
        GetDropItem(dropKey);
    }
    public override void TakeTeardropDamage(float damage, Vector2 direction)
    {
        TakeDamage(damage);
    }
}
