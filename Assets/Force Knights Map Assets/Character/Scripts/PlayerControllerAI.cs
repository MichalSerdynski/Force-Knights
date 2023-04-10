using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAI : MonoBehaviour
{
    public float speed = 5f;
    public float dashDistance = 3f;
    public float dashTime = 0.5f;
    public int maxHealth = 6;
    public int currentHealth;
    public float invincibilityTime = 1f;
    public GameObject footstep;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    private bool isDashing = false;
    private bool isInvincible = false;
    private float dashTimer = 0f;
    public AudioSource saberSwing;
    public LayerMask enemiesLayer;
    public float attackRange = 0.5f;
    public Transform attackPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        footstep.SetActive(false);
        saberSwing = GetComponent<AudioSource>();
    }

    private void Update()
    {
       
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
        

        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
            
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            isDashing = true;
            dashTimer = 0f;
        }
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");
        saberSwing.Play();
        // Check for enemy hit
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayer);

        // Damage enemy if hit
        foreach (Collider2D enemy in hitEnemies)
        {
            // Get the EnemyController script component from the enemy
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

            // If the enemy has an EnemyController script component
            if (enemyAI != null)
            {
                // Call the TakeDamage function on the EnemyController script component
                enemyAI.TakeDamage(1);
            }
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            Dash();
            return;
        }

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
            StartCoroutine(PushBack(pushDirection));
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            isInvincible = true;
            StartCoroutine(InvincibilityFlash());
            StartCoroutine(InvincibilityTimer());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator InvincibilityFlash()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.1f);
        }

        isInvincible = false;
    }

    private IEnumerator InvincibilityTimer()
    {
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    private IEnumerator PushBack(Vector2 direction)
    {
        rb.AddForce(direction * 500f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
    }

    private void Die()
    {
        // Handle player death here
    }

    private void Dash()
    {
        dashTimer += Time.fixedDeltaTime;
        
        if (dashTimer >= dashTime)
        {
            isDashing = false;
            return;
        }
        anim.SetTrigger("Dash");
        float distance = Mathf.Min(dashDistance, dashTime - dashTimer);
        Vector2 direction = new Vector2(movement.x, 0f).normalized;
        rb.MovePosition(rb.position + direction * distance);
        
    }
    void footsteps()
    {
        footstep.SetActive(true);
    }
    void StopFootsteps()
    {
        footstep.SetActive(false);
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
