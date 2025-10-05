using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public ItemManager.ItemType itemType;
    public bool isActiveItem = false;
    private float spawnTime = 0.0f;
    private void Start()
    {
        spawnTime = Time.time;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time < spawnTime + 0.5f) return;
        if (collision.gameObject.CompareTag(Constants.TAG_PLAYER))
        {
            Player.Instance.GetItem(itemType, isActiveItem);
            Destroy(gameObject);
        }
    }
}
