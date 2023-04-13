using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    public Image[] heartImages; // array of heart images to display

    private PlayerControllerAI playerControllerAI; // reference to the PlayerControllerAI script

    void Start()
    {
        playerControllerAI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerAI>(); // get reference to PlayerControllerAI script
    }

    void Update()
    {
        int currentHealth = playerControllerAI.currentHealth;
        int maxHealth = playerControllerAI.maxHealth;

        // update heart images based on current health
        switch (currentHealth)
        {
            case 6:
                heartImages[0].sprite = fullHeartSprite;
                heartImages[1].sprite = fullHeartSprite;
                heartImages[2].sprite = fullHeartSprite;
                break;
            case 5:
                heartImages[0].sprite = fullHeartSprite;
                heartImages[1].sprite = fullHeartSprite;
                heartImages[2].sprite = halfHeartSprite;
                break;
            case 4:
                heartImages[0].sprite = fullHeartSprite;
                heartImages[1].sprite = fullHeartSprite;
                heartImages[2].sprite = emptyHeartSprite;
                break;
            case 3:
                heartImages[0].sprite = fullHeartSprite;
                heartImages[1].sprite = halfHeartSprite;
                heartImages[2].sprite = emptyHeartSprite;
                break;
            case 2:
                heartImages[0].sprite = fullHeartSprite;
                heartImages[1].sprite = emptyHeartSprite;
                heartImages[2].sprite = emptyHeartSprite;
                break;
            case 1:
                heartImages[0].sprite = halfHeartSprite;
                heartImages[1].sprite = emptyHeartSprite;
                heartImages[2].sprite = emptyHeartSprite;
                break;
            case 0:
                heartImages[0].sprite = emptyHeartSprite;
                heartImages[1].sprite = emptyHeartSprite;
                heartImages[2].sprite = emptyHeartSprite;
                break;
            default:
                Debug.LogError("Invalid current health value");
                break;
        }
    }

    // heart sprite images
    public Sprite fullHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite emptyHeartSprite;
}
