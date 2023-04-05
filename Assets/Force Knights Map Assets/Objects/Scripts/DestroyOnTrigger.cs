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
        otherCollider = doorToOpen.GetComponent<BoxCollider2D>();
        otherAnimator = doorToOpen.GetComponent<Animator>();
    }
        void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            Destroy(objToDestroy);
        
        otherAnimator.SetBool("HasKey", true);
        otherCollider.isTrigger = true;
    }
}
