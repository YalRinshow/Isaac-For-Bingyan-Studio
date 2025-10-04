using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_NerveEnding : Enemy
{
    public override void Initialize(bool eilite = false)
    {
        health = 20.0f;
        collisionDamage = 2;
        rbEnemy = GetComponent<Rigidbody2D>();
        isEilite = eilite;
    }
    public override void TakeTeardropDamage(float damage, Vector2 direction)
    {
        TakeDamage(damage);
    }
}
