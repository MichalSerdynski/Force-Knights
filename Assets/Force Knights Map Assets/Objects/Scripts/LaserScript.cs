using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D boxCollider;

    public AudioSource laserDoor;

    // Start is called before the first frame update
    void Start()
    {
        laserDoor = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
{
    if (GameObject.FindGameObjectWithTag("Enemy") == null)
    {
        animator.SetBool("LaserOff", true);
        boxCollider.isTrigger = true;
        StartCoroutine(PlaySoundDelayed());
    }
}

IEnumerator PlaySoundDelayed()
{
    Debug.Log("Coroutine started");
    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    Debug.Log("Coroutine waited for " + animator.GetCurrentAnimatorStateInfo(0).length + " seconds");
    laserDoor.Play();
}
}
