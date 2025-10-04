using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackFly : Enemy
{
    public override void Initialize(bool eilite = false)
    {
        health = 5.0f;
        rbEnemy = GetComponent<Rigidbody2D>();
        isEilite = eilite;
        speedMove= 0.4f;
        flyingEnemy = true;
        Debug.Log("fly");
    }
    private void FixedUpdate()
    {
        Vector2 direction = (Player.Instance.rbPlayer.position - rbEnemy.position).normalized;
        transform.Translate(direction * speedMove * Time.deltaTime);
    }
}
