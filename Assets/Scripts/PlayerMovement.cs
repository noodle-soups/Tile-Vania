using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Inputs")]
    Vector2 inputMove;

    [Header("Values")]
    float moveSpeed = 10f;
    float jumpSpeed = 11f;
    float climbSpeed = 3f;
    float swimSpeed = 3f;
    float gravityDefault;
    
    [Header("Colors")]
    Color colorDefault = new Color(1f, 1f, 1f, 1f);
    Color colorSwimming = new Color(0.5f, 0.7f, 1f, 1f);

    [Header("Components")]
    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D myColliderBody;
    SpriteRenderer mySpriteRenderer;

    [Header("States")]
    bool isIdle = true;
    bool isRunning = false;
    bool isGrounded = false;
    bool isClimbing = false;
    bool isSwimming = false;
    bool isJumpFall = false;

    [Header("CheckTouching")]
    GameObject playerFeet;
    GameObject playerHead;
    CheckTouching checkFeetTouching;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myColliderBody = GetComponent<CapsuleCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        
        gravityDefault = rb.gravityScale;

        playerFeet = transform.Find("Feet").gameObject;
        checkFeetTouching = playerFeet.GetComponent<CheckTouching>();
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

        Falling();

        //Debug.Log("Input Move: " + inputMove);
        Debug.Log("RB Velocity?: " + rb.velocity);
        //Debug.Log("Gravity: " + rb.gravityScale);
        //Debug.Log("isIdle: " + isIdle);
        //Debug.Log("isRunning: " + isRunning);
        //Debug.Log("isGrounded: " + isGrounded);
        //Debug.Log("isClimbing: " + isClimbing);
        //Debug.Log("isSwimming: " + isSwimming);
        //Debug.Log(playerFeet.name);
        //Debug.Log("Jump Speed: " + jumpSpeed);
        Debug.Log("Falling: " + isJumpFall);
    }

    void OnMove(InputValue value)
    {
        inputMove = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isGrounded) {return;}
        //if (value.isPressed) {rb.velocity += new Vector2 (0f, jumpSpeed);}
        if (value.isPressed) {rb.velocity += new Vector2 (0f, 12f);}
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
        isGrounded = checkFeetTouching.feetGrounded;
    }

    void ClimbLadder()
    {
        // exit
        if (!myColliderBody.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            isClimbing = false;
            return;
        }

        // isClimbing = ladderTouch + ladderLatch
        bool ladderTouch = false;
        bool ladderLatch = false;
        if (myColliderBody.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {ladderTouch = true;}
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
        if (myColliderBody.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            isSwimming = true;
        }
        else
        {
            isSwimming = false;
            mySpriteRenderer.color = colorDefault;
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
        anim.SetBool("isJumpFall", isJumpFall);
    }

    void Falling()
    {
                
        if (rb.velocity.y < 0)
        {
            isJumpFall = true;
        }
        else
        {
            isJumpFall = false;
        }
    }

}
