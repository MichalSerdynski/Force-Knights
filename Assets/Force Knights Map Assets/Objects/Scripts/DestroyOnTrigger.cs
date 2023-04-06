using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    public GameObject objToDestroy;
    public GameObject doorToOpen;
    private Animator otherAnimator;
    private Collider2D otherCollider;

    

    void Start()
    {

        
        //Finds components for the door to animate and open
        otherCollider = doorToOpen.GetComponent<BoxCollider2D>();
        otherAnimator = doorToOpen.GetComponent<Animator>();
    }
        void OnTriggerEnter2D(Collider2D other)
    {
        //if player touches the object it destroys it
        if (other.gameObject.tag == "Player")
            
            Destroy(objToDestroy);

        //opens the door and makes it passable
        otherAnimator.SetBool("HasKey", true);
        otherCollider.isTrigger = true;
    }
}
