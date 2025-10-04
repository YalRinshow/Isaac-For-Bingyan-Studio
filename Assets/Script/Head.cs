using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Head : MonoBehaviour
{
    public SpriteRenderer headImage;
    public Sprite headFront;
    public Sprite headBack;
    public Sprite headRight;
    private bool isFlashing;
    private float flashingRate = 0.3f;
    private float lastFlash = 0.0f;
    private float colorSize;
    private void Update()
    {
        if (!isFlashing) return;
        int cycleCount = Mathf.FloorToInt((Time.time - lastFlash) / (2.0f * flashingRate));
        float resTime = (Time.time - lastFlash) - (2.0f * flashingRate) * cycleCount;
        if (resTime < flashingRate)
        {
            colorSize = resTime / flashingRate;
        }
        else
        {
            resTime -= flashingRate;
            colorSize = 1.0f - (resTime / flashingRate);
        }
        headImage.color = new Color(1.0f, colorSize, colorSize, 1.0f);
    }
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
    public void StartFlash()
    {
        isFlashing = true;
        colorSize = 0.0f;
        headImage.color = new Color(1.0f, colorSize, colorSize, 1.0f);
        lastFlash = Time.time;
    }
    public void StopFlash()
    {
        isFlashing = false;
        headImage.color = Color.white;
    }
}
