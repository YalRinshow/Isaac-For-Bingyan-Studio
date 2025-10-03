using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDropped : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG_PLAYER))
        {
            Player.Instance.GetBomb();
            Destroy(gameObject);
        }
    }
}
