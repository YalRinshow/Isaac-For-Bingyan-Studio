using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Monstro : Enemy
{
    public override void Initialize(bool dropKey = false)
    {
        health = 250.0f;
        rbEnemy = GetComponent<Rigidbody2D>();
        GetDropItem(dropKey);
        UIManager.Instance.InitializeHealthBar(health);
    }
    public override void TakeTeardropDamage(float damage, Vector2 direction)
    {
        TakeDamage(damage);
    }
}
