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
        Vector2 randomPosition = new Vector2(Random.Range(minBounds.x, maxBounds.x), Random.Range(minBounds.y, maxBounds.y));

        return randomPosition;
    }

    public void TakeDamage(float damage)
    {
        saberHit.Play();
        health -= damage;
        if (health <= 0)
        {
            // Flash and destroy the game object
            StartCoroutine(DestroyEnemy());
        }
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
