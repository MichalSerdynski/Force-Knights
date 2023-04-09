using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public float dirX;
    public float moveSpeed;
    public Rigidbody2D rb;
    public bool facingRight = false;
    private Vector3 localScale;

    // Use this for initialization
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent <Rigidbody2D>();
        dirX = -1f;
        moveSpeed = 2f;
    }

    // Update is called once per frame
    private void OnCollision2D(Collider2D other)
    {
        Debug.Log("Object that collided with me: " + other.gameObject.name);
        
        if (other.gameObject.name == "Walls")
        {
            dirX *= -1f;
        }
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }
}