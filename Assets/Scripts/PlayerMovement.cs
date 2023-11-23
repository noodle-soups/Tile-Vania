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
    [SerializeField] float climbSpeed = 5f;
    float gravityDefault;


    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D myCapsuleCollider;

    bool ladderLatch = false;


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
        //string jumpLayers = "Ground";

        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {return;}
        //if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask(jumpLayers))) {return;}

        if (value.isPressed)
        {
            rb.velocity += new Vector2 (0f, jumpSpeed);
        }
    }

    void ClimbLadder()
    {
        rb.gravityScale = gravityDefault;
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {return;}

        bool ladderTouch = false;
        bool upPress = false;

        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {ladderTouch = true;}
        if (moveInput.y > Mathf.Epsilon) {upPress = true;}
        if (ladderTouch && upPress) {ladderLatch = true;}

        if ((ladderLatch && moveInput.y < 0) || (!ladderTouch)) {ladderLatch = false;}

        if (ladderLatch)
        {
            Vector2 climbVelocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
            rb.velocity = climbVelocity;
            rb.gravityScale = 0f;
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon; // true when player is moving R/L

        if (playerHasHorizontalSpeed) // only flip when moving
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

}
