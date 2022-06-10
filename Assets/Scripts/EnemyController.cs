using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int HitPoints;
    public Animator Animator;
    
    public void OnHit(int damage)
    {
        HitPoints -= damage;
        Animator.Play("EnemyHit");

        if (HitPoints < 1)
        {
            Destroy(gameObject);
        }
    }

}
