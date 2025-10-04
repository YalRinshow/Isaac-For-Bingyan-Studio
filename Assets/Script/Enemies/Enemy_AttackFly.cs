using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackFly : Enemy
{
    public override void Initialize(bool dropKey = false)
    {
        health = 5.0f;
        rbEnemy = GetComponent<Rigidbody2D>();
        speedMove= 0.4f;
        flyingEnemy = true;
        droppedItems.Add(ItemManager.ItemType.Key);
        droppedItems.Add(ItemManager.ItemType.Heart);
        GetDropItem(dropKey);
    }
    private void FixedUpdate()
    {
        Vector2 direction = (Player.Instance.rbPlayer.position - rbEnemy.position).normalized;
        transform.Translate(direction * speedMove * Time.deltaTime);
    }
    public override void TakeTeardropDamage(float damage, Vector2 direction)
    {
        TakeDamage(damage);
    }
}
