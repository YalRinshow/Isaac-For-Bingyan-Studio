using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDropped : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG_PLAYER))
        {
            Player.Instance.GetHeart();
            Destroy(gameObject);
        }
    }
}
