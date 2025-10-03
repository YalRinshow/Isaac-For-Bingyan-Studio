using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_NerveEnding : Enemy
{
    public void Initialize(bool eilite = false, HashSet<string> items = null)
    {
        health = 20.0f;
        collisionDamage = 2;
        rbEnemy = GetComponent<Rigidbody2D>();
        isEilite = eilite;
        droppedItems = items;
    }
}
