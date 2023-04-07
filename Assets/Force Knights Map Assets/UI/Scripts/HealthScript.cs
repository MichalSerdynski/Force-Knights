using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public int currentSprite;
    public Sprite[] spriteArray;

    void Start()
    {
        spriteRenderer.sprite = spriteArray[currentSprite];
    }

    void Update()
    {
       
    }

    public void ChangeSprite()
    {
        spriteRenderer.sprite = spriteArray[currentSprite];
        currentSprite++;
        if(currentSprite >= spriteArray.Length)
        {
            currentSprite = 0;
        }
    }
}
