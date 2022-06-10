using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;
    Vector3 nextPosition;
    float speed = 16f;

    void Start()
    {
        nextPosition = StartPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == StartPoint.position)
        {
            nextPosition = EndPoint.position;
        }

        if (transform.position == EndPoint.position)
        {
            nextPosition = StartPoint.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);

    }
}
