using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Transform Player;
    public Transform Respawn;
    public Transform EndPortal;

    bool isFacingRight = true; 
    
    public Rigidbody2D Rigidbody; //player's rigidbody
    
    Vector2 moveDirection = Vector2.zero;
    GroundCheckController groundCheckController; //reference to another controller
    
    float speed = 25f;
    public Collider2D Collider;

    public GameObject Bullet;
    public Transform FirePoint; //bullets are created in this point
    
    Animator animator;
    
    AudioSource audio;
    bool canShoot;

    public string Level;


    void Start()
    {   
        groundCheckController = GetComponentInChildren<GroundCheckController>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        canShoot = true;
    }


    void Update()
    {
        if (!isFacingRight && moveDirection.x > 0f) //flip player depending on user actions
        {
            Flip();
        }
        else if (isFacingRight && moveDirection.x < 0f)
        {
            Flip();
        }

        Rigidbody.velocity = new Vector2(moveDirection.x * speed, Rigidbody.velocity.y);


        if (transform.position.y < -30f) //respawn after fall
        {
            OnRespawn();
        }

        if (transform.position.x > EndPortal.transform.position.x - 2f)
        {
            Debug.Log("In");

            if (Level == "Level 1")
            {
                SceneManager.LoadScene("Level 2");
            }
            else
            {
                SceneManager.LoadScene("Level 1");
            }
        }

        animator.SetBool("isWalking", moveDirection.x != 0); //running constant animations by changing anim parameters
        animator.SetBool("isJumping", !IsGrounded());

    }

    void OnTriggerEnter2D(Collider2D touchInfo)
    {
        if (touchInfo.gameObject.CompareTag("Enemy")) 
        {
            OnRespawn();
        }
    }

    void OnRespawn()
    {
        transform.position = Respawn.position;
    }


    bool IsGrounded()
    {   
        return groundCheckController.CheckIfGrounded();
    }

    public void OnMove(InputAction.CallbackContext context) //react on user interaction
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context) //react on user interaction
    {
        if (IsGrounded())
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 25f);
            
        }
    }

    public void OnShoot(InputAction.CallbackContext context) //react on user interaction
    { 
        if (CanAttack())
        {
            canShoot = false;
            animator.Play("Shooting");
            
            Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
            StartCoroutine(StopShooting());

        }
    }

    IEnumerator StopShooting() //stop player from constant attacks and blending animations
    {
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
        
    }


    bool CanAttack() // player can attack when stationary
    {
        return moveDirection.x == 0 && IsGrounded() && canShoot;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Player.Rotate(0f, 180f, 0f);
    }

    void OnWalk() //react on animation events
    {
        audio.Play();

    }

    void OnOther() //react on animation events
    {
        audio.Stop();
    }

}