using UnityEngine;

public class BossScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float idleTime = 3f;
    public float attackRange = 2f;
    public float stompRange = 3f;
    public int maxHealth = 100;
    public int damagePerHit = 10;
    public float stompForce = 10f;
    private int currentHealth;
    private bool isIdle;
    private bool isAttacking;
    private bool isStomping;
    private bool isMovingLeft;
    private bool isFacingLeft = true;
    private Animator animator;
    private Transform playerTransform;
    public LayerMask playerLayer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!isAttacking && !isStomping)
        {
            Move();
            CheckAttack();
        }
    }

    private void Move()
    {
        animator.SetTrigger("Boss_Move");
        float direction = isMovingLeft ? -1f : 1f;
        Vector3 movement = new Vector3(direction * moveSpeed * Time.deltaTime, 0f, 0f);
        transform.Translate(movement);

        if (isMovingLeft && CheckLeftWall() || !isMovingLeft && CheckRightWall())
        {
            isMovingLeft = !isMovingLeft;
            Flip();
        }
    }

    private bool CheckLeftWall()
    {
        Vector2 position = transform.position;
        position.x -= 0.5f * transform.localScale.x;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.left, 0.1f);
        return hit.collider != null && hit.collider.CompareTag("Wall");
    }

    private bool CheckRightWall()
    {
        Vector2 position = transform.position;
        position.x += 0.5f * transform.localScale.x;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.right, 0.1f);
        return hit.collider != null && hit.collider.CompareTag("Wall");
    }

    private void Flip()
    {
        isFacingLeft = !isFacingLeft;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void CheckAttack()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= attackRange)
        {
            isAttacking = true;
            animator.SetTrigger("Boss_Swipe");
        }
    }

    private void Attack()
    {
        if (!isIdle)
        {
            isIdle = true;
            Invoke("EndIdle", idleTime);
        }

        if (isAttacking)
        {
            isAttacking = false;
        }
    }
    private void EndIdle()
    {
        isIdle = false;
        animator.SetTrigger("Boss_Stomp");
        isStomping = true;
        Invoke("EndStomp", animator.GetCurrentAnimatorStateInfo(0).length);
    }
    private void Stomp()
    {
        animator.SetTrigger("Boss_Stomp");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stompRange, playerLayer);
        foreach (Collider2D hit in hits)
        {
            hit.GetComponent<Rigidbody2D>().AddForce(Vector2.down * stompForce, ForceMode2D.Impulse);
        }
    }

    private void EndStomp()
    {
        isStomping = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStomping && collision.gameObject.CompareTag("Player"))
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * stompRange, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isIdle) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Boss_Death");
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        this.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isIdle)
        {
            collision.GetComponent<PlayerControllerAI>().TakeDamage(damagePerHit);
            Attack();

        }
    }
}
