using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{  //set the values in the inspector
    public Animator animator;

    public Transform target; //drag and stop player object in the inspector
    public float within_range;
    public float speed;
    public Vector2 startPos;

    public int maxHealth = 100;
    int currentHealth;
    
    public AudioSource saberHit;

    public void Start()
    {

        saberHit = GetComponent<AudioSource> ();

        //chooses a random position for stormtrooper to move to.
        startPos = new Vector2(Random.Range(-10f, 9f), Random.Range(-5f, 4f));

         currentHealth = maxHealth;
    }


    public void Update()
    {
        

        //get the distance between the player and enemy (this object)
        float dist = Vector2.Distance(target.position, transform.position);

        //check if it is within the range you set
        if (dist <= within_range)
        {
            //move to target(player) 
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed);
        }

        //else, if it is not in rage, it will not follow player and go to random place
        else if (dist > within_range)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos, speed);
        }
        }

        public void TakeDamage(int damage)
    {
        

        saberHit.Play();

        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");

        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        

    }
    void OnDeathAnimationFinished()
    {
        Destroy(gameObject);
    }
}
