using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float stompRange;
    public Transform stompPosition;
    public float stompForce;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            stompPlayerIfInRange();
        }
    }
    public void stompPlayerIfInRange()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(stompPosition.position, stompRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Finds what direction to push the player
                Vector2 pushDirection = (collider.transform.position - stompPosition.position).normalized;
                // Move the player back
                collider.GetComponent<Rigidbody2D>().AddForce(pushDirection * stompForce, ForceMode2D.Impulse);

                Debug.Log("Pushback");

            }
        }
    }
}
