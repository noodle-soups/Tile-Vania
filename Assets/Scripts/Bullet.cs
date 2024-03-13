using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] float bulletSpeed = 20f;
    PlayerMovement player; //reference to PlayerMovement script on Player game object
    float xSpeed;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }


    void Update()
    {
        rb.velocity = new UnityEngine.Vector2 (xSpeed, 0f);
    }

    //what happens when bullet collides with enemy
    //change this to be a collision instead of trigger?
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject); //destroy enemy
        }
        Destroy(gameObject); //destroy bullet
    }

    //what happens if bullet hits anything else
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);        
    }

}
