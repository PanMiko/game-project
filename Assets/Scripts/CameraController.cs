using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform PlayerTransform;

    void LateUpdate()
    {
        Vector3 temp = transform.position;

        temp.x = PlayerTransform.position.x;

        transform.position = temp;

    }
}
