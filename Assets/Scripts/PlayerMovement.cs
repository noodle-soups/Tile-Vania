using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    Vector2 moveInput;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 3f;
    float gravityDefault;


    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D myCapsuleCollider;

    bool isIdle = true;

    bool isClimbing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityDefault = rb.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();

        Debug.Log("isIdle: " + isIdle);
        Debug.Log("isClimbing: " + isClimbing);
        Debug.Log(moveInput.y);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon; // true when player is moving R/L
        anim.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void OnJump(InputValue value)
    {
        string jumpLayers = "Ground";
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask(jumpLayers))) {return;}

        if (value.isPressed)
        {
            rb.velocity += new Vector2 (0f, jumpSpeed);
        }
    }

    void ClimbLadder()
    {
        // animation
        anim.SetBool("isClimbing", isClimbing);
        
        // default gravity status
        rb.gravityScale = gravityDefault;

        // exit
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            isClimbing = false;
            return;
        }

        // isClimbing = ladderTouch + ladderLatch
        bool ladderTouch = false;
        bool ladderLatch = false;
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {ladderTouch = true;}
        if (moveInput.y > Mathf.Epsilon) {ladderLatch = true;}
        if (ladderTouch && ladderLatch) {isClimbing = true;}

        // player slides down or exits ladder
        if ((isClimbing && moveInput.y < 0) || (!ladderTouch)) {isClimbing = false;}

        // allow y-velocity while isClimbing
        if (isClimbing)
        {
            Vector2 climbVelocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
            rb.velocity = climbVelocity;
            rb.gravityScale = 0f;
        }

    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

}
