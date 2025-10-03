using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int roomDir;
    public int roomNumber;
    public bool isOpen = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Map.CurrentRoomEnemyClear()) return;
        if (collision.gameObject.CompareTag(Constants.TAG_PLAYER))
        {
            if (isOpen)
            {
                Map.RoomTransfer(roomNumber, roomDir);
            }
            else if (Player.Instance.keyNumber > 0)
            {
                Player.Instance.UseKey();
                isOpen = true;
            }
        }
    }
}
