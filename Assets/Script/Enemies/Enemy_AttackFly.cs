using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackFly : Enemy
{
    public void Initialize(bool eilite = false, HashSet<string> items = null)
    {
        health = 5.0f;
        rbEnemy = GetComponent<Rigidbody2D>();
        isEilite = eilite;
        droppedItems = items;
        speedMove= 0.4f;
        flyingEnemy = true;
    }
    private void FixedUpdate()
    {
        Vector2 direction = (Player.Instance.rbPlayer.position - rbEnemy.position).normalized;
        transform.Translate(direction * speedMove * Time.deltaTime);
    }
}
