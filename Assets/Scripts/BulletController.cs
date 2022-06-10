using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    float speed = 30f;

    void Start()
    {
        Rigidbody.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            EnemyController enemy = hitInfo.GetComponent<EnemyController>();
            enemy.OnHit(10);
        }
        else if (hitInfo.gameObject.tag == "Environment" || hitInfo.gameObject.tag == "MovingEnvironment")
        {
            Destroy(gameObject);
        }
        
    }
}
