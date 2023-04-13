using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    public float detectionRange = 3f;
    public float health = 1f;
    public GameObject roomObject;
    public AudioSource saberHit;
    private Vector2 targetPosition;
    public Rigidbody2D rb;
    public float pushbackForce = 500f;
    public GameObject player;
    public LayerMask wallLayer;

    private void Start()
    {
        targetPosition = GetRandomPosition();
        saberHit = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Move towards the target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // If close to the target position, get a new random position
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = GetRandomPosition();
        }

        // Check if the player is nearby and move towards them
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null && Vector2.Distance(transform.position, playerObject.transform.position) < detectionRange)
        {
            targetPosition = playerObject.transform.position;
        }
    }

    public float raycastDistance = 2f;

    private Vector2 GetRandomPosition()
    {
        // Get the bounds of the walls object
        Collider2D roomCollider = roomObject.GetComponent<Collider2D>();
        Vector2 minBounds = roomCollider.bounds.min;
        Vector2 maxBounds = roomCollider.bounds.max;

        // Choose a random direction for the enemy to move
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        // Check if there is a wall in the direction of movement
        RaycastHit2D hit = Physics2D.Raycast(transform.position, randomDirection, raycastDistance, wallLayer);

        // If there is a wall, choose a new random direction
        if (hit.collider != null)
        {
            // Choose a new random direction that is not blocked by walls
            int numIterations = 0;
            while (numIterations < 100)
            {
                randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

                Collider2D[] hitColliders = Physics2D.OverlapCircleAll((Vector2)transform.position + randomDirection * 0.5f, 0.1f, wallLayer);
                if (hitColliders.Length == 0)
                {
                    break;
                }

                numIterations++;
            }

            // If we can't find a clear direction after 100 iterations, return current position
            if (numIterations >= 100)
            {
                return transform.position;
            }
        }

        // Calculate a random position in the chosen direction
        Vector2 randomPosition = (Vector2)transform.position + randomDirection * Random.Range(1f, maxBounds.magnitude);

        // Make sure the random position is within the bounds of the room
        randomPosition.x = Mathf.Clamp(randomPosition.x, minBounds.x, maxBounds.x);
        randomPosition.y = Mathf.Clamp(randomPosition.y, minBounds.y, maxBounds.y);

        return randomPosition;
    }




    //// Get the bounds of the walls object
    //Collider2D roomCollider = roomObject.GetComponent<Collider2D>();
    //Vector2 minBounds = roomCollider.bounds.min;
    //Vector2 maxBounds = roomCollider.bounds.max;

    // Get a random position within the bounds
    //Vector2 randomPosition = new Vector2(Random.Range(minBounds.x, maxBounds.x), Random.Range(minBounds.y, maxBounds.y));

    // Cast a Raycast2D in the direction of the random position
    //Vector2 direction = randomPosition - (Vector2)transform.position;
    //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, WallLayer);

    // If the Raycast2D hits a wall, choose a new random position
    //if (hit.collider != null)
    //{
    //    randomPosition = GetRandomPosition();
    //}
    //return randomPosition;


    public void TakeDamage(float damage)
    {
        saberHit.Play();
        health -= damage;
        //Vector2 direction = (transform.position - player.transform.position).normalized;
        //rb.AddForce(direction * pushbackForce, ForceMode2D.Impulse);

        if (health <= 0)
        {
            // Flash and destroy the game object
            StartCoroutine(DestroyEnemy());
        }
    }

    //private IEnumerator PushBack(Vector2 direction)
  //  {
       // rb.AddForce(direction * 500f, ForceMode2D.Impulse);
       // yield return new WaitForSeconds(0.1f);
       // rb.velocity = Vector2.zero;
   // }

    private IEnumerator DestroyEnemy()
    {
        float flashTime = 0.1f;
        float elapsedTime = 0f;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        while (elapsedTime < flashTime)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(flashTime / 2f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(flashTime / 2f);
            elapsedTime += flashTime;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere to show the detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    
}
