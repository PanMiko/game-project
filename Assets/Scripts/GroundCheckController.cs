using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckController : MonoBehaviour
{
    public GameObject Player;
    public LayerMask GroundLayer;


    void OnTriggerEnter2D(Collider2D touchInfo)
    {
        if (touchInfo.gameObject.CompareTag("MovingEnvironment"))
        {
            Player.transform.parent = touchInfo.gameObject.transform;
        }
    }

    void OnTriggerExit2D(Collider2D touchInfo)
    {
        if (touchInfo.gameObject.CompareTag("MovingEnvironment"))
        {
            Player.transform.parent = null;
        }
    }

    public bool CheckIfGrounded()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size,
            0, Vector2.down, 0.1f, GroundLayer);
    }
}