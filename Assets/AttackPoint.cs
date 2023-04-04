using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        transform.position = player.position;

        // Rotate the child object based on the player's rotation
        transform.rotation = player.rotation;
    }
}
