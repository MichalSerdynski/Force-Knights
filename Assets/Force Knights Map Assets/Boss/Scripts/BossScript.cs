using UnityEngine;

public class BossScript : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float idleTime = 3f;
    public float attackRange = 2f;
    public float stompRange = 3f;
    public int maxHealth = 100;
    public int damagePerHit = 2;
    public float stompForce = 500f;
    private int currentHealth;
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


    private void Start()
    {
        
        isMoving = true;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindGameObjectWithTag("Boss");
    }

    private void Update()
    {
        //Makes animator know left or right movement
        animator.SetFloat("Horizontal", direction);

        //If it is not attacking or stomping it will make the boss move
        if (!isAttacking && !isStomping)
        {
            Move();
            
        }
        
        CheckAttack();
        
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
        position.x -= 0.5f * transform.localScale.x;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.left, 0.2f);
        return hit.collider != null && hit.collider.CompareTag("Wall");
    }

    private bool CheckRightWall()
    {
        Vector2 position = transform.position;
        position.x += 0.5f * transform.localScale.x;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.right, 0.2f);
        return hit.collider != null && hit.collider.CompareTag("Wall");
    }
    //END OF MOVEMENT AND CHECKING FOR WALLS

    // Checks if player is nearby to start attack
    private void CheckAttack()
    {
        // Get a reference to the player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        // Check if the player is within attack range
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange || (!hasAttacked && !isMoving))
        {
                
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
        // Stops Boss moving
        moveSpeed = 0f;
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
        animator.ResetTrigger("Boss_Stomp");

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
    private void EndStomp()
    {
        
        moveSpeed = 3f;
    isIdle = false;
    isAttacking = false;
    isStomping = false;
    isMovingLeft = false;
    isMoving = true;
    hasAttacked = false;
    isFacingLeft = true;
        Move();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //   if (isStomping && collision.gameObject.CompareTag("Player"))
    //    {
    //        Vector2 direction = (collision.transform.position - transform.position).normalized;
    //        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * stompRange, ForceMode2D.Impulse);
    //    }
    // }

    // Takes damage only during Idle animation, starts death after health reaches 0
    public void TakeDamage(int amount)
    {
        if (!isIdle) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //Starts death animation and stops movement (if moving)
    private void Die()
    {
        moveSpeed = 0f;
        animator.SetTrigger("Boss_Death");
        
    }

    //Destroys Boss after death animation
    private void DeathAfterAnimation()
    {
        Destroy(gameObject);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
   // {
     //   if (collision.CompareTag("Player") && isIdle)
    //    {
     //       collision.GetComponent<PlayerControllerAI>().TakeDamage(damagePerHit);
    //        //Attack();
//
   //     }
  // }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere to show the detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
