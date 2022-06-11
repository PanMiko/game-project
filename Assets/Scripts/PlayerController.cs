using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public float speed = 25f;
    public float jumpPower = 10f;
    public Rigidbody2D Rigidbody;
    public LayerMask GroundLayer;
    public GameObject BulletPrefab;

    public Transform FirePoint;
    bool isFacingRight = true;
    Vector2 moveDirection = Vector2.zero;
    Vector2 mousePosition = Vector2.zero;
    private bool canShoot = true;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
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

           
            var direction = Camera.main.ScreenToWorldPoint(mousePosition);
            direction.z = transform.position.z;

            direction = Vector3.ClampMagnitude(direction - transform.position, 2.5f); //HARDCODED VALUE
            var newPosition = transform.position + direction;
            var distance = Vector3.Distance(transform.position, newPosition);
            if (distance >= 2.5f)
            {
                FirePoint.transform.position = newPosition;
            }
        }
    }


    void LateUpdate()
    {
        if (isLocalPlayer)
        {
            Vector3 pos = transform.position;
            pos.y += 8f;
            pos.z = Camera.main.transform.position.z;
            Camera.main.transform.position = pos;
        }
    }

    public void OnMove(InputAction.CallbackContext context) //react on user interaction
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context) //react on user interaction
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Debug.Log("Jump");
        if (IsGrounded())
        {
            Debug.Log("Grounded");
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jumpPower);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Debug.Log("OnFire");


        if (canShoot)
        {
            canShoot = false;
            CmdFire();
            StartCoroutine(StopShooting());
        }
    }

    IEnumerator StopShooting() //stop player from constant attacks and blending animations
    {
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        //Player.Rotate(0f, 180f, 0f);
    }

    public bool IsGrounded() //HARDCODED DISTANCE VALUE CHECK IT LATER!
    {
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        var raycastHit = Physics2D.Raycast(collider.bounds.center, Vector2.down, 2.6f, GroundLayer);
        Debug.Log(raycastHit.distance);

        return raycastHit;
    }

    [Command]
    void CmdFire()
    {
        GameObject bulletClone = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
        var bulletRb = bulletClone.GetComponent<Rigidbody2D>();
        float bulletSpeed = 30f;

        Vector3 newVector = FirePoint.transform.position - transform.position;

        bulletRb.velocity = newVector.normalized * bulletSpeed;
        NetworkServer.Spawn(bulletClone);
    }


    // //Server
    // [ServerRpc]
    // public void MoveServerRpc(Vector2 directions)
    // {
    //     moveDirection.Value = directions;
    // }
    //
    // //Server
    // [ServerRpc]
    // public void JumpServerRpc()
    // {
    //     Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jumpPower);
    // }
    //
    // [ServerRpc]
    // public void FireServerRpc()
    // {
    //     GameObject bullet = objectPool.GetNetworkObject(BulletPrefab).gameObject;
    //
    //     bullet.transform.position = FirePoint.position;
    //     bullet.transform.rotation = FirePoint.rotation;
    //
    //     var bulletRb = bullet.GetComponent<Rigidbody2D>();
    //     float bulletSpeed = 30f;
    //     bulletRb.velocity = transform.right * bulletSpeed;
    //
    //     Debug.Log("OnFireSpawn");
    //     bullet.GetComponent<BulletController>().Spawn();
    // }
}