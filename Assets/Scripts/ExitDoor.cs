using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ExitDoor : MonoBehaviour
{

    public Animator animator;
	private bool exitFinished = false;

    public GameObject player;
    private PlayerController playerScript;

    // Start is called before the first frame update
    public void Start()
    {
        animator = GetComponent<Animator>();
        playerScript = player.GetComponent<PlayerController>();
    }

    // This is triggered by an Animation Event when the "Door_Open" animation finishes playing
    void ExitFinished()
    {
        exitFinished = true;
    }

    // Runs when an object collides with this object
    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.gameObject.tag.Equals("Player") && exitFinished)
    	{
    		Debug.Log("Level complete! Loading next level...");
    		GameObject gameUI = GameObject.Find("GameUI");
            StartCoroutine(GameUI.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1, 0.5f));
    	}
    }
}
