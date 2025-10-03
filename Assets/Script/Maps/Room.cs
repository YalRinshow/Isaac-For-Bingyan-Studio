using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Room : MonoBehaviour
{
    int number;
    public Vector2 roomCenter;
    public Vector2[] spawnPosition = new Vector2[4];
    public Vector2[] doorPosition = new Vector2[4];
    public float[] doorRotation = new float[4];
    public bool isEnemyClear = false;
    public bool isActivated = false;
    private void Start()
    {
    }
    public void Initialize()
    {
        doorPosition[0] = new Vector2(0, 3.4f);
        doorRotation[0] = 0.0f;
        doorPosition[1] = new Vector2(0, -4.4f);
        doorRotation[1] = 180.0f;
        doorPosition[2] = new Vector2(-6.4f, 0);
        doorRotation[2] = 90.0f;
        doorPosition[3] = new Vector2(6.4f, 0);
        doorRotation[3] = -90.0f;
        spawnPosition[0] = new Vector2(0, 3.1f);
        spawnPosition[1] = new Vector2(0, -3.3f);
        spawnPosition[2] = new Vector2(-5.0f, 0);
        spawnPosition[3] = new Vector2(5.0f, 0);
    }
    public void GenerateEnemisAndGround()
    {
        if (isActivated) return;
        isActivated = true;
    }
}