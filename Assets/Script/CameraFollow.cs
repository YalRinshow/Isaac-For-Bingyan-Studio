using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private float smoothing = 0.05f;
    private void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        Camera mainCamera = GetComponentInChildren<Camera>();
        mainCamera.orthographicSize = Screen.height / (2f * 64);
    }
    private void LateUpdate()
    {
        if (target != null)
        {
            if (transform.position != target.position)
            {
                transform.position = Vector2.Lerp(transform.position, target.position, smoothing);
            }
        }

    }
}
