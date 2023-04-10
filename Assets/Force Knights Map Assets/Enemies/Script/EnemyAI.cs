using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    public float detectionRange = 3f;
    public float health = 1f;
    public GameObject wallsObject;
    public AudioSource saberHit;
    private Vector2 targetPosition;
    public Rigidbody2D rb;
    public float pushbackForce = 10f;
    public GameObject player;

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

    private Vector2 GetRandomPosition()
    {
        // Get the bounds of the walls object
        Collider2D wallsCollider = wallsObject.GetComponent<Collider2D>();
        Vector2 minBounds = wallsCollider.bounds.min;
        Vector2 maxBounds = wallsCollider.bounds.max;

        // Get a random position within the bounds
        Vector2 randomPosition = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

        return randomPosition;
    }

    public void TakeDamage(float damage)
    {
        saberHit.Play();
        health -= damage;
        Vector2 direction = (transform.position - player.transform.position).normalized;
        rb.AddForce(direction * pushbackForce, ForceMode2D.Impulse);

        if (health <= 0)
        {
            // Flash and destroy the game object
            StartCoroutine(DestroyEnemy());
        }
    }

    private IEnumerator PushBack(Vector2 direction)
    {
        rb.AddForce(direction * 500f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
    }

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
