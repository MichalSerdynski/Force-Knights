using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioSource doorSound;  // reference to the audio source component that will play the sound

    private Animator animator;  // reference to the animator component that controls the door animation

    private bool isOpening = false;  // flag to keep track if the door is opening or not

    void Start()
    {
        animator = GetComponent<Animator>();  // get reference to the animator component on this game object
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DoorOpening") && !isOpening)
        {
            isOpening = true;
            PlayDoorSound();
        }
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("DoorOpening") && isOpening)
        {
            isOpening = false;
        }
    }

    void PlayDoorSound()
    {
        doorSound.Play();  // play the sound
    }
}
