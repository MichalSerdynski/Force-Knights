using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int damage = 1;
    HealthScript healthScript;

    // Start is called before the first frame update
    void Start()
    {
        healthScript = GameObject.FindGameObjectWithTag("Health").GetComponent<HealthScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(damage);
            healthScript.ChangeSprite();
        }
    }
}
