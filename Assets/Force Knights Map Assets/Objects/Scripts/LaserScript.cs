using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D boxCollider;



    

    // Update is called once per frame
    void Update()
{
    if (GameObject.FindGameObjectWithTag("Enemy") == null)
    {
        animator.SetBool("LaserOff", true);
        boxCollider.isTrigger = true;
        
    }
}

}