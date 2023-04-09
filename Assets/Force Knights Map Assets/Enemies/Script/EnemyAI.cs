using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float detectionRadius = 5f;
    public int maxHealth = 3;
    public int damage = 1;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private int currentHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        targetPosition = GetRandomPosition();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = GetRandomPosition();
        }

        if (PlayerIsNearby())
        {
            MoveTowardsPlayer();
        }
        else
        {
            MoveTowardsTargetPosition();
        }
    }

    private Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(-6f, 16f);
        float randomY = Random.Range(-6f, 6f);
        return new Vector2(randomX, randomY);
    }

    private bool PlayerIsNearby()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
        rb.velocity = direction * walkSpeed;
    }

    private void MoveTowardsTargetPosition()
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.velocity = direction * walkSpeed;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 repelDirection = (collision.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(repelDirection * walkSpeed, ForceMode2D.Impulse);
            currentHealth -= damage;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
