using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D boxCollider;
    
    public int enemyCount = 0;
    public GameObject laser;
    [SerializeField] private BoxCollider2D otherCollider;

    // Update is called once per frame
    void Update()
    {
        if (enemyCount == 0)
        {
            animator.SetBool("LaserOff", true);
            boxCollider.isTrigger = true;
        }

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && otherCollider.OverlapPoint(other.transform.position))
        {
            enemyCount++;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && otherCollider.OverlapPoint(other.transform.position))
        {
            enemyCount--;
        }
    }
}
