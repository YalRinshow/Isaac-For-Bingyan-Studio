using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Teardrop : MonoBehaviour
{
    private float lifetime = Constants.TEARDROP_LIFETIME;
    private Vector2 direction;
    private float speedMove = Constants.TEARDROP_SPEED;
    public static float teardropDamage = Constants.TEARDROP_DAMAGE;
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    void FixedUpdate()
    {
        transform.Translate(direction * speedMove * Time.deltaTime);
    }
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG_ENEMY))
        {
            var enemy = collision.gameObject.GetComponentInParent<Enemy>();
            if (enemy.teardropEffective)
            {
                enemy.TakeTeardropDamage(teardropDamage, this.direction);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag(Constants.TAG_GROUND))
        {
            var ground = collision.gameObject.GetComponent<Ground>();
            if (ground is Rock)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag(Constants.TAG_WALL))
        {
            Destroy(gameObject);
        }
    }
}
