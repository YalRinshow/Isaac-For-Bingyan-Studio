using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private float smoothing = 0.05f;
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
