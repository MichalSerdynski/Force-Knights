using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{  //set the values in the inspector
    public Transform target; //drag and stop player object in the inspector
    public float within_range;
    public float speed;
    public Vector2 startPos;
    
    public void Start()
    {
        //chooses a random position for stormtrooper to move to.
        startPos = new Vector2(Random.Range(-10f, 9f), Random.Range(-5f, 4f));
    }


    public void Update()
    {
        

        //get the distance between the player and enemy (this object)
        float dist = Vector2.Distance(target.position, transform.position);

        //check if it is within the range you set
        if (dist <= within_range)
        {
            //move to target(player) 
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed);
        }

        //else, if it is not in rage, it will not follow player and go to random place
        else if (dist > within_range)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos, speed);
        }
        }
}
