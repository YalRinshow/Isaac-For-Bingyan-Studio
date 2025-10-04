using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public SpriteRenderer headImage;
    public Sprite headFront;
    public Sprite headBack;
    public Sprite headRight;
    public void UpdateHeadImage(int dir)
    {
        transform.rotation = Quaternion.identity;
        if (dir == 0)
        {
            headImage.sprite = headBack;
        }
        else if (dir == 1)
        {
            headImage.sprite = headFront;
        }
        else
        {
            headImage.sprite = headRight;
            if (dir == 2)
            {
                transform.rotation = Quaternion.Euler(180.0f, 0.0f, 180.0f);
            }
        }
    }
}
