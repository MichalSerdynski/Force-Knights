using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSoundScript : MonoBehaviour
{
    public AudioSource laserSound;

    
    void PlayLaserSound()
    {
        laserSound.Play();
    }
}
