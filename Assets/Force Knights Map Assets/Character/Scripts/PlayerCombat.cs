using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public AudioSource saberSwing;


    public void Start(){
        saberSwing = GetComponent<AudioSource> ();
    }


    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
        }
    }

    void Attack()
    {
        // Attack animation
        animator.SetTrigger("Attack");

        saberSwing.Play();

        // Detect enemies in range
       Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage enemies
        foreach(Collider2D enemy in hitEnemies)
        {
           enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    
}
