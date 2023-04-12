using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript1 : MonoBehaviour
{
    public float moveSpeed;
    public float idleTime = 3f;
    public float attackRange = 2f;
    public float stompRange = 3f;
    public int maxHealth = 100;
    public int damagePerHit = 2;
    public float stompForce = 500f;
    public int currentHealth;
    private bool isIdle;
    private bool isAttacking;
    private bool isStomping;
    private bool isMovingLeft;
    private bool isMoving;
    private bool hasAttacked;
    private bool isFacingLeft = true;
    private Animator animator;
    private Transform playerTransform;
    public LayerMask playerLayer;
    public float direction;
    public GameObject player;
    public GameObject boss;
    public float invincibilityTime = 1f;
    private bool isInvincible = false;
    private Rigidbody2D rigidbody;
    private void Start()
    {
        StartCoroutine(BossCoroutine());
        rigidbody = GetComponent<Rigidbody2D>();
        
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindGameObjectWithTag("Boss");
        moveSpeed = 3f;
    }

    private void Update()
    {
        //Makes animator know left or right movement
        animator.SetFloat("Horizontal", direction);

        //If it is not attacking or stomping it will make the boss move
        if (!isAttacking && !isStomping && !isIdle && isMoving)
        {
            Move();
            
        }

        StartCoroutine(CheckAttackCoroutine());
        
    }

    private IEnumerator BossCoroutine()
    {
        while (true)
        {
            // Boss moves left and right until player is within stomp range
            while (Vector3.Distance(transform.position, playerTransform.position) > stompRange)
            {
                isIdle = false;
                isAttacking = false;
                isStomping = false;
                isMoving = true;
                Move();
                yield return null;
            }

            // Boss stops moving and attacks once player is within attack range
            isMoving = false;
            isAttacking = true;
            StartCoroutine(CheckAttackCoroutine());
            yield return new WaitUntil(() => !isAttacking);

            // Boss goes idle for a set amount of time
            isIdle = true;
            animator.ResetTrigger("Boss_Swipe");
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(idleTime);

            // Boss stomps and pushes player back
            isIdle = false;
            isStomping = true;
            animator.SetTrigger("Boss_Stomp");
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Idle"));

            // Boss starts moving again
            isStomping = false;
            isMoving = true;
        }
    }

    //START OF MOVEMENT AND CHECKING FOR WALLS
    private void Move()
    {
        
        animator.SetTrigger("Boss_Move");
        direction = isMovingLeft ? -1f : 1f;
        Vector3 movement = new Vector3(direction * moveSpeed * Time.deltaTime, 0f, 0f);
        transform.Translate(movement);
        

        if (isMovingLeft && CheckLeftWall() || !isMovingLeft && CheckRightWall())
       {
            isMovingLeft = !isMovingLeft;
           
        }
    }

    private bool CheckLeftWall()
    {
        Vector2 position = transform.position;
        position.x -= 0.3f * transform.localScale.x;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.left, 0.2f);
        return hit.collider != null && hit.collider.CompareTag("Wall");
    }

    private bool CheckRightWall()
    {
        Vector2 position = transform.position;
        position.x += 0.3f * transform.localScale.x;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.right, 0.2f);
        return hit.collider != null && hit.collider.CompareTag("Wall");
    }
    //END OF MOVEMENT AND CHECKING FOR WALLS

    // Checks if player is nearby to start attack
    //private void CheckAttack()
    //{
    //    // Get a reference to the player GameObject
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    // Check if the player is within attack range
    //    if (Vector3.Distance(transform.position, player.transform.position) <= attackRange || (!hasAttacked && !isMoving))
    //    {
    //        // Stops Boss moving
    //        rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
    //        rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
    //        // Triggers Attack Method
    //        Attack();
    //        
    //    }
   // }

    private IEnumerator CheckAttackCoroutine()
    {
        // Get a reference to the player GameObject
        //GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Check if the player is within attack range
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange || (!hasAttacked && !isMoving))
        {
            // Stops Boss moving
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            // Wait before starting the attack
            yield return new WaitForFixedUpdate();
            // Triggers Attack Method
            Attack();

        }
    }
            // Controls animation for atack and stops movement
            private void Attack()
    {
        animator.ResetTrigger("Boss_Move");
        //Stops boss attacking over and over again
        hasAttacked = true;
        
        // Trigger the "Boss_Swipe" animation
        animator.SetTrigger("Boss_Swipe");

        
    }
    // Causes Damage to player only as the attack hits
    private void Hit()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(transform.position, player.transform.position) <= stompRange)
        {
            PlayerControllerAI playerControllerAI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerAI>();

            playerControllerAI.TakeDamage(2);
        }
    }
    // Starts Idle and opens up Boss for attack
    private void Idle()
    {
        animator.ResetTrigger("Boss_Swipe");
        animator.SetTrigger("Idle");
        isIdle = true;
    }
   
    
   // Stomps to push player back
    private void Stomp()
    {
        animator.SetTrigger("Boss_Stomp");
        isStomping = true;
        //animator.ResetTrigger("Boss_Stomp");

    }

    public void StompPlayerIfInRange()
    {
      
        // Calculate the distance between the player and the enemy
        float distance = Vector3.Distance(player.transform.position, boss.transform.position);

        // Check if the player is within stompRange of the enemy
        if (distance <= stompRange)
        {
            // Calculate the direction from the enemy to the player
            Vector3 direction = player.transform.position - boss.transform.position;
            direction.Normalize();

            // Push the player in the opposite direction
            player.GetComponent<Rigidbody2D>().AddForce(direction * -1.0f * stompForce, ForceMode2D.Impulse);
        }
    }

    // After the stomp this resets the boss to start everything again
    private IEnumerator EndStompCoroutine()
    {
        rigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        rigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        isIdle = false;
        isAttacking = false;
        isStomping = false;
        isMovingLeft = false;
        isMoving = true;
        hasAttacked = false;
        isFacingLeft = true;

        yield return new WaitForFixedUpdate(); // Wait for the next FixedUpdate to ensure the constraints are unfrozen

        Move();
    }

    // Takes damage only during Idle animation, starts death after health reaches 0
    public void TakeDamage(int amount)
    {
        if (!isIdle) return;
        if (!isInvincible)
        {
            currentHealth -= amount;
            StartCoroutine(InvincibilityFlash());
            StartCoroutine(InvincibilityTimer());
            Stomp();
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(DestroyEnemy());
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
    //Starts death animation and stops movement (if moving)
    //private void Die()
    //{
    //    moveSpeed = 0f;
    //    animator.SetTrigger("Boss_Death");
    //    
    //}

    //Destroys Boss after death animation
    //private void DeathAfterAnimation()
    //{
    //    Destroy(gameObject);
    //}

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere to show the detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

