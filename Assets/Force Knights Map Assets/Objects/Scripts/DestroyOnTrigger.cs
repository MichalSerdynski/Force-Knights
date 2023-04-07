using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    public GameObject objToDestroy;
    public GameObject doorToOpen;
    
    private Animator otherAnimator;
    private Collider2D otherCollider;

    public AudioSource doorOpen;

    void Start()
    {
        doorOpen = GetComponent<AudioSource>();
        //Finds components for the door to animate and open
        otherCollider = doorToOpen.GetComponent<BoxCollider2D>();
        otherAnimator = doorToOpen.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if player touches the object it makes it invisible and uninteractable
        if (other.gameObject.tag == "Player")
        {
            //opens the door and makes it passable
            otherAnimator.SetBool("HasKey", true);
            otherCollider.isTrigger = true;

            // Disable the object's renderer and collider
            objToDestroy.GetComponent<Renderer>().enabled = false;
            objToDestroy.GetComponent<Collider2D>().enabled = false;
            
            // Disable the object's audio source
            AudioSource audioSource = objToDestroy.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.enabled = false;
            }
            
            // Play the doorOpen sound if the audio source is enabled
            if (doorOpen.enabled)
            {
                doorOpen.Play();
            }
        }
    }
}
