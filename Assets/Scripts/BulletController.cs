using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BulletController : NetworkBehaviour
{
    public float Lifetime = 2f;
    
    void Awake()
    {
        StartCoroutine(WaitLifetime());
    }

    IEnumerator WaitLifetime()
    {
        yield return new WaitForSeconds(Lifetime);
        //if(gameObject.)
        NetworkServer.Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
        {
            NetworkServer.Destroy(gameObject);
        }
    }

}
