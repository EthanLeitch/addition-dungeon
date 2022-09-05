using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{

    public GameObject player;
    private PlayerController playerScript;


    // Start is called before the first frame update
    public void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Runs when an object collides with this object
    void OnTriggerStay2D(Collider2D col)
    {
    	if(col.gameObject.tag.Equals("Player"))
    	{
            if(gameObject.name.Contains("Health Potion"))
            {
                playerScript.health = playerScript.maxHealth;
            }
            else if(gameObject.name.Contains("Power Potion"))
            {
                playerScript.power = playerScript.maxPower;
            }

            SoundManager.PlaySound("health_potion");
            gameObject.SetActive(false);
    	}
    }
}
