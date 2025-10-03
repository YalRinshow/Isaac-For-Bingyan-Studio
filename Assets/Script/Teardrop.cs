using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Teardrop : MonoBehaviour
{
    private float lifetime = Constants.TEARDROP_LIFETIME;
    private Rigidbody2D rbTeardrop;
    private Collider2D cdTeardrop;
    private Vector2 direction;
    private float speedMove = Constants.TEARDROP_SPEED;
    private float teardropDamage = Constants.TEARDROP_DAMAGE;
    private void Start()
    {
        rbTeardrop = GetComponent<Rigidbody2D>();
        cdTeardrop = GetComponent<Collider2D>();
        Destroy(gameObject, lifetime);
    }
    private void Update()
    {
    }
    void FixedUpdate()
    {
        //if (MapColisionCheck())
        //{
            transform.Translate(direction * speedMove * Time.deltaTime);
        //}
        //else
        //{
            //Destroy(gameObject);
        //}
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
                enemy.TakeDamage(teardropDamage);
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
