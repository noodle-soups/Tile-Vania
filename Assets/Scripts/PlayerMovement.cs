using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Inputs")]
    Vector2 inputMove;

    [Header("Values")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] float swimSpeed = 3f;
    Color colorSwimming = new Color(0.5f, 0.7f, 1f, 1f);
    float gravityDefault;

    [Header("Components")]
    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D myCapsuleCollider;
    SpriteRenderer mySpriteRenderer;

    [Header("States")]
    bool isIdle = true;
    bool isRunning = false;
    bool isGrounded = false;
    bool isClimbing = false;
    bool isSwimming = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        gravityDefault = rb.gravityScale;
    }

    void Update()
    {
        Idle();
        Run();
        Grounded();
        ClimbLadder();
        Swimming();
        Animations();
        FlipSprite();
        GravityApplication();

        //Debug.Log("Input Move: " + inputMove);
        Debug.Log("RB Velocity: " + rb.velocity);
        Debug.Log("Gravity: " + rb.gravityScale);
        //Debug.Log("isIdle: " + isIdle);
        //Debug.Log("isRunning: " + isRunning);
        Debug.Log("isGrounded: " + isGrounded);
        //Debug.Log("isClimbing: " + isClimbing);
        Debug.Log("isSwimming: " + isSwimming);
    }

    void OnMove(InputValue value)
    {
        inputMove = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isGrounded) {return;}
        if (value.isPressed) {rb.velocity += new Vector2 (0f, jumpSpeed);}
    }

    void Idle()
    {
        if (inputMove != new Vector2(0f, 0f))
        {
            isIdle = false;
        }
        else
        {
            isIdle = true;
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(inputMove.x * moveSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
        isRunning = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
    }

    void Grounded()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void ClimbLadder()
    {
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
        if (inputMove.y > Mathf.Epsilon) {ladderLatch = true;}
        if (ladderTouch && ladderLatch) {isClimbing = true;}

        // player slides down or exits ladder
        if ((isClimbing && inputMove.y < 0) || (!ladderTouch)) {isClimbing = false;}

        // allow y-velocity while isClimbing
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            Vector2 climbVelocity = new Vector2(rb.velocity.x, inputMove.y * climbSpeed);
            rb.velocity = climbVelocity;
        }
    }

    void Swimming()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            isSwimming = true;
        }
        else
        {
            isSwimming = false;
        }

        if (isSwimming)
        {
            Vector2 swimVelocity = new Vector2(inputMove.x * swimSpeed, inputMove.y * swimSpeed);
            rb.velocity = swimVelocity;
            mySpriteRenderer.color = colorSwimming;          
        }
    }

    void GravityApplication()
    {
        if (isClimbing || isSwimming)
        {
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = gravityDefault;
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

    void Animations()
    {
        anim.SetBool("isIdle", isIdle);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isClimbing", isClimbing);
    }

}
