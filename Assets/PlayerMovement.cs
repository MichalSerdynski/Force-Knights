using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    
    private Vector2 moveDirection;

    // Update is called once per frame
    void Update()
    {
        // Processing Inputs
        ProcessInputs();
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    /// </summary>
    ///Fixed Update is better for physicsc calculations - consistant calls
    void FixedUpdate()
    {
        // Physics Calculations
        Move();
    }


    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); //Raw gives value of 0 OR 1 - better for keyboard, where for controller we would use not Raw as it can have values between 0 and 1
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized; //every direction we move in the speed is normalized and capped on 1 (normally it will move faster diagonally, as we add 2 vectorsin this situation )
    }

    /// <summary>
    /// Callback for processing animation movmenmts for modifying root motion.
    /// </summary>

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

}
