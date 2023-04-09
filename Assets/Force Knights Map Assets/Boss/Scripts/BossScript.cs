using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public float speed = 60f;
    public bool facingRight = true;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float moveDirection = facingRight ? 1f : -1f;
        Vector2 movement = new Vector2(moveDirection * speed * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = movement;
    }

    private void OnCollision2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            facingRight = !facingRight;
        }
    }
}