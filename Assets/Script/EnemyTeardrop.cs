using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeardrop : MonoBehaviour
{
    private float lifetime = Constants.TEARDROP_ENEMY_LIFETIME;
    private Vector2 direction;
    private float speedMove = Constants.TEARDROP_SPEED;
    public static int teardropDamage = Constants.TEARDROP_ENEMY;
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    void FixedUpdate()
    {
        transform.Translate(direction * speedMove * Time.deltaTime);
    }
    public void SetDirectionAndSpeed(Vector2 direction, float speedMove)
    {
        this.direction = direction;
        this.speedMove = speedMove;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG_PLAYER))
        {
            Player.Instance.TakeDamage(teardropDamage);
            Destroy(gameObject);
        }
    }
}
