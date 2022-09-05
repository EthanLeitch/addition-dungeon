using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MathBlock : MonoBehaviour
{
    new public Rigidbody2D rigidbody;

	public int randomNumber;
	public int maxGeneratedNumber = 100;
    private TextMeshPro blockText;

    private GameObject bm;
    private GameObject exitDoor;
    private GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
    	// Generate a random number and store it in the randomNumber variable
        randomNumber = Random.Range(1, maxGeneratedNumber); 

        // Get the Text child and set its content to randomNumber
		blockText = transform.GetChild(0).GetComponent<TextMeshPro>();
        //text_tmp = GetComponent<TextMeshPro>();
        blockText.text = randomNumber.ToString();

        rigidbody = GetComponent<Rigidbody2D>();

        // Get the BlockManager game object and assign it to 'bm'
        bm = GameObject.Find("BlockManager");
        exitDoor = GameObject.Find("Exit Door");

        // Create an array called blocks for storing all the math blocks
        blocks = GameObject.FindGameObjectsWithTag("Math Block");
    }

    // Runs when an object collides with this object
    void OnCollisionEnter2D(Collision2D col)
    {
        
        // Makes sure that the item colliding with us is actually a Math Block
    	if (col.gameObject.tag == "Math Block") 
    	{
            if(col.gameObject.GetComponent<MathBlock>().randomNumber + randomNumber == bm.GetComponent<BlockManager>().solution) {
                // If the answer matches the solution, change the text color to green
                blockText.color = new Color32(110, 170, 120, 255);
                SoundManager.PlaySound("success");

                FreezeBlocksPosition();

                // Set animator bool of doorOpen to true
                exitDoor.GetComponent<Animator>().SetBool("doorOpen", true);
                SoundManager.PlaySound("door_open");

                // Shake the camera!
                ScreenShake.Instance.ShakeCamera(3f, 5f);
            } else {
                // Else, change the text color to red
                blockText.color = new Color32(154, 79, 80, 255);
                SoundManager.PlaySound("fail");

                FreezeBlocksPosition();

                // Fade to black, and load current scene (restarts level)
                StartCoroutine(GameUI.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex, 1f));
    		}
    	}
    }

    // Runs when an object stops colliding with this object
    void OnCollisionExit2D(Collision2D col) 
    {
    	if (col.gameObject.tag == "Math Block") 
    	{
    		// Change the text color back to orange (the default)
    		blockText.color = new Color32(190, 149, 92, 255);
    	}
    }

    void FreezeBlocksPosition()
    {
        // Freeze each block's rigidbody
        foreach (GameObject block in blocks) 
        {
            block.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

}