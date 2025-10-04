using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy_Knight : Enemy
{
    private float chargeSpeed = 2.0f;
    private float facingAngle = 60.0f;
    private Vector2[] roamingDir = new Vector2[4] { Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    private int facingDir;
    private float lastRoaming = 0.0f;
    private float roamingTime = 0.8f;
    private float lastCharge = 0.0f;
    private float nextCharge = 0.0f;
    private float chargeRate = 2.0f;
    private float chargeTime = 1.5f;
    private bool isRoaming = false;
    private bool isCharging = false;
    public GameObject head;
    public Head enemyHead;
    public override void Initialize(bool dropKey = false)
    {
        health = 30.0f;
        collisionDamage = 1;
        rbEnemy = GetComponent<Rigidbody2D>();
        speedMove = 1.0f;
        enemyHead = head.GetComponent<Head>();
        droppedItems.Add(ItemManager.ItemType.Key);
        droppedItems.Add(ItemManager.ItemType.Bomb);
        droppedItems.Add(ItemManager.ItemType.Heart);
        GetDropItem(dropKey);
    }
    private void FixedUpdate()
    {
        if (isCharging)
        {
            if (Time.time - lastCharge > chargeTime)
            {
                isCharging = false;
            }
            else return;
        }
        Vector2 directionToPlayer = (Player.Instance.rbPlayer.position - rbEnemy.position).normalized;
        if (CanCharge(directionToPlayer))
        {
            Charge(directionToPlayer);
            isRoaming = false;
            return;
        }
        if (isRoaming)
        {
            if (Time.time - lastRoaming > roamingTime)
            {
                isRoaming = false;
            }
            else return;
        }
        if (!isCharging)
        {
            Roaming();
        }
    }
    private bool CanCharge(Vector2 direction)
    {
        if (Time.time < nextCharge) return false;
        float dotProduct = Vector2.Dot(direction, roamingDir[facingDir]);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        return angle < facingAngle;
    }
    private void Roaming()
    {
        isRoaming = true;
        lastRoaming = Time.time;
        facingDir = Random.Range(0, 4);
        enemyHead.UpdateHeadImage(facingDir);
        rbEnemy.velocity = roamingDir[facingDir] * speedMove;
    }
    public override void TakeTeardropDamage(float damage, Vector2 direction)
    {
        if (Vector2.Equals(direction, roamingDir[facingDir ^ 1])) return;
        TakeDamage(damage);
    }
    private void Charge(Vector2 direction)
    {
        isCharging = true;
        lastCharge = Time.time;
        nextCharge = Time.time + chargeRate;
        rbEnemy.velocity = direction * chargeSpeed;
    }
}
