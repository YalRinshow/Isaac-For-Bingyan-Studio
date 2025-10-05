using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public ItemManager.ItemType itemType;
    public bool isActiveItem = false;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG_PLAYER))
        {
            Player.Instance.GetItem(itemType, isActiveItem);
            Destroy(gameObject);
        }
    }
}
