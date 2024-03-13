using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkBlobMovement : MonoBehaviour
{
    //float moveSpeed = 1f;
    float moveSpeed;
    float directionFacing;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        directionFacing = transform.localScale.x;
        moveSpeed = 1f * directionFacing;
    }



    void Update()
    {
        rb.velocity = new Vector2 (moveSpeed, 0f);
    }


    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2 (-(Mathf.Sign(rb.velocity.x)), 1f);
    }

}
