using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTouching : MonoBehaviour
{

    BoxCollider2D myColliderFeet;
    public bool feetGrounded;

    void Start()
    {
        myColliderFeet = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        TouchingGround();
    }

    void TouchingGround()
    {
        if (myColliderFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            feetGrounded = true;
        }
        else
        {
            feetGrounded = false;
        }
    }

}